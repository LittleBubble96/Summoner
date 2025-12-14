using System;
using System.Diagnostics;
using UnityGameFramework.Runtime;
using Debug = UnityEngine.Debug;

namespace GameLogic.Game.Common
{
    public static class AnalysisHelper
    {
        /// <summary>
        /// 监控方法执行时长、内存占用变化
        /// </summary>
        /// <param name="keyLog">日志标识（区分不同监控点）</param>
        /// <param name="action">要执行并监控的方法</param>
        public static void Watch(string keyLog, Action action)
        {
#if UNITY_EDITOR
            // 空值保护
            if (string.IsNullOrEmpty(keyLog))
            {
                Debug.LogWarning("[AnalysisHelper] keyLog 不能为空！");
                return;
            }

            if (action == null)
            {
                Debug.LogWarning("[AnalysisHelper] 要执行的 Action 不能为空！");
                return;
            }

            // ========== 执行前准备 ==========
            // 1. 记录开始时间（高精度计时器）
            var stopwatch = Stopwatch.StartNew();

            // 2. 记录执行前内存（Unity 专用内存统计）
            // 总分配内存（MB）
            long beforeTotalAllocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024;
            // 总保留内存（MB）
            long beforeTotalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1024 / 1024;
            // 未使用保留内存（MB）
            long beforeTotalUnusedReserved =
                UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1024 / 1024;

            // ========== 执行目标方法 ==========
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Log.Error($"[AnalysisHelper] 执行 {keyLog} 时发生异常：{e.Message}\n{e.StackTrace}");
                return;
            }
            finally
            {
                // 停止计时器
                stopwatch.Stop();
            }

            // ========== 执行后统计 ==========
            // 1. 内存统计
            long afterTotalAllocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024;
            long afterTotalReserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1024 / 1024;
            long afterTotalUnusedReserved =
                UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1024 / 1024;

            // 2. 计算内存变化（+ = 新增分配，- = 回收）
            long deltaAllocated = afterTotalAllocated - beforeTotalAllocated;
            long deltaReserved = afterTotalReserved - beforeTotalReserved;
            long deltaUnusedReserved = afterTotalUnusedReserved - beforeTotalUnusedReserved;

            // ========== 打印日志 ==========
            var logContent = $@"
[AnalysisHelper] 【{keyLog}】执行统计：
├─ 执行时长：{stopwatch.Elapsed.TotalMilliseconds:F2} 毫秒（{stopwatch.Elapsed.TotalSeconds:F3} 秒）
├─ 内存变化（MB）：
│  ├─ 总分配内存：{beforeTotalAllocated} → {afterTotalAllocated}（{(deltaAllocated >= 0 ? "+" : "")}{deltaAllocated}）
│  ├─ 总保留内存：{beforeTotalReserved} → {afterTotalReserved}（{(deltaReserved >= 0 ? "+" : "")}{deltaReserved}）
│  └─ 未使用保留内存：{beforeTotalUnusedReserved} → {afterTotalUnusedReserved}（{(deltaUnusedReserved >= 0 ? "+" : "")}{deltaUnusedReserved}）
└─ 备注：内存单位为 MB，+ 表示新增占用，- 表示回收
";
            Log.Warning(logContent);
#endif
        }
    }
}