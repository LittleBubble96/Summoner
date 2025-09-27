Shader "Custom/HexWireframe"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (0.5,0.5,0.5,1)
        _HighlightColor ("Highlight Color", Color) = (1,1,0,1)
        _HighlightIndex ("Highlight Index", Int) = -1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                uint id : SV_VertexID;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                uint id : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            fixed4 _LineColor;
            fixed4 _HighlightColor;
            int _HighlightIndex;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.id = v.id;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 计算六边形索引（每6个顶点一个六边形）
                int hexIndex = i.id / 6;
                
                // 检查是否属于高亮的六边形
                if (hexIndex == _HighlightIndex)
                {
                    return _HighlightColor;
                }
                
                return _LineColor;
            }
            ENDCG
        }
    }
}