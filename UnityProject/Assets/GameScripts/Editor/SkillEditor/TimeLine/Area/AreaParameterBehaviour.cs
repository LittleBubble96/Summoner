using GameLogic.Game;
using UnityEngine;
using UnityEngine.Playables;

[SerializeField]
public class AreaParameterBehaviour : PlayableBehaviour
{
    public AreaType AreaType;

    public Vector3 Origin;
    // 球形/扇形共用参数
    public float Radius;
    // 盒形参数
    public Vector3 BoxSize;
    // 扇形专属参数
    public float SectorAngle; // 角度（0-360）
    public Vector3 Direction; // 朝向

    private AreaParams _params;
    
    public override void OnGraphStart(Playable playable)
    {
        _params = AreaParams.New(AreaType)
            .SetOrigin(Origin)
            .SetRadius(Radius)
            .SetBoxSize(BoxSize)
            .SetSectorAngle(SectorAngle)
            .SetDirection(Direction);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        AreaUtil.DrawGizmos(_params);
    }
    
}