using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Game
{
    public static class AreaUtil
    {
        // ------------------------------
        // 对外接口：按范围类型提供专用检测方法
        // ------------------------------

        /// <summary>
        /// 球形范围检测
        /// </summary>
        public static List<Collider> DetectSphere(AreaParams param , int layerMask)
        {
            if (param.AreaType != AreaType.Sphere)
                throw new ArgumentException("参数类型必须为球形");

            var colliders = Physics.OverlapSphere(param.Origin, param.Radius, layerMask);
            return new List<Collider>(colliders);
        }

        /// <summary>
        /// 盒形范围检测
        /// </summary>
        public static List<Collider> DetectBox(AreaParams param , int layerMask)
        {
            if (param.AreaType != AreaType.Box)
                throw new ArgumentException("参数类型必须为盒形");

            Vector3 halfExtents = param.BoxSize / 2;
            var colliders = Physics.OverlapBox(param.Origin, halfExtents, Quaternion.identity, layerMask);
            return new List<Collider>(colliders);
        }

        /// <summary>
        /// 扇形范围检测
        /// </summary>
        public static List<Collider> DetectSector(AreaParams param, int layerMask)
        {
            if (param.AreaType != AreaType.Sector)
                throw new ArgumentException("参数类型必须为扇形");

            var results = new List<Collider>();
            if (param.SectorAngle <= 0) return results;

            // 先通过球形过滤
            var sphereColliders = Physics.OverlapSphere(param.Origin, param.Radius, layerMask);
            float halfAngle = param.SectorAngle / 2f;
            Vector3 dirNormalized = param.Direction.normalized;

            foreach (var collider in sphereColliders)
            {
                Vector3 toTarget = collider.transform.position - param.Origin;
                if (toTarget.sqrMagnitude < 0.01f) continue; // 忽略自身

                float angle = Vector3.Angle(dirNormalized, toTarget.normalized);
                if (angle <= halfAngle)
                    results.Add(collider);
            }
            return results;
        }

        /// <summary>
        /// 通用检测入口（自动根据参数类型选择检测方式）
        /// </summary>
        public static List<Collider> Detect(AreaParams param, int layerMask)
        {
            return param.AreaType switch
            {
                AreaType.Sphere => DetectSphere(param,layerMask),
                AreaType.Box => DetectBox(param,layerMask),
                AreaType.Sector => DetectSector(param,layerMask),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // ------------------------------
        // Gizmos 绘制（按参数类型绘制）
        // ------------------------------
        public static void DrawGizmos(AreaParams param, Color color = default)
        {
            if (color == default) color = Color.red;
            Gizmos.color = color;

            switch (param.AreaType)
            {
                case AreaType.Sphere:
                    Gizmos.DrawWireSphere(param.Origin, param.Radius);
                    break;
                case AreaType.Box:
                    Gizmos.DrawWireCube(param.Origin, param.BoxSize);
                    break;
                case AreaType.Sector:
                    DrawSectorGizmos(param.Origin, param.Direction, param.Radius, param.SectorAngle);
                    break;
            }
        }

        private static void DrawSectorGizmos(Vector3 origin, Vector3 direction, float radius, float angle)
        {
            if (angle >= 360)
            {
                // 360度扇形等同于圆形
                Gizmos.DrawWireSphere(origin, radius);
                return;
            }

            direction = direction.normalized;
            float halfAngle = angle / 2f * Mathf.Deg2Rad; // 转为弧度

            // 计算扇形左右边界角度
            Quaternion leftRot = Quaternion.Euler(0, -halfAngle * Mathf.Rad2Deg, 0);
            Quaternion rightRot = Quaternion.Euler(0, halfAngle * Mathf.Rad2Deg, 0);
            Vector3 leftDir = leftRot * direction;
            Vector3 rightDir = rightRot * direction;

            // 绘制两条边
            Gizmos.DrawLine(origin, origin + leftDir * radius);
            Gizmos.DrawLine(origin, origin + rightDir * radius);

            // 绘制弧线（用多段线段模拟）
            int segments = 20; // 弧线分段数（越多越平滑）
            Vector3 prevPoint = origin + direction * radius;

            for (int i = 1; i <= segments; i++)
            {
                float currentAngle = -halfAngle + (halfAngle * 2f) * (i / (float)segments);
                Quaternion rot = Quaternion.Euler(0, currentAngle * Mathf.Rad2Deg, 0);
                Vector3 currentDir = rot * direction;
                Vector3 currentPoint = origin + currentDir * radius;

                Gizmos.DrawLine(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
        }
    }

    public enum AreaType
    {
        Sphere,   // 球形
        Box,      // 盒形
        Sector    // 扇形
    }
}