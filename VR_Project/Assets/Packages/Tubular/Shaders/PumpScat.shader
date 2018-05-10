Shader "Tubular/PumpScat"
{
	Properties
	{
		_Color ("Color", Color) = (1,0.5,0.5,0.7)
		_Radius ("Radius", Range(0.001, 1)) = 1
		_Offset ("Offset", Float) = 20.0
		_Speed ("Speed", Float) = 1.0

		_Expansion ("Expansion", Float) = 1.0
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 100
			

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : VIEWDIR;
				float2 uv : TEXCOORD0;
			};

			float4 _Color;

			half _Radius, _Offset, _Speed;
			half _Expansion;

			float weight(float x) {
				return 1 - pow(2 * x - 1, 2);
			}

			v2f vert (appdata v)
			{
				v2f o;

				float2 uv = v.uv;
				float3 center = v.vertex.xyz;

				float wave = (sin(_Time.y * _Speed + uv.y * _Offset) + 1.0) * 0.5 * _Expansion;
				v.vertex.xyz += v.normal * (_Radius + wave)  * weight(uv.y);

				/*float torsion = sin(_Time.y * _Speed + uv.y * _Offset) * _Torsion;
				float4 q = rotate_angle_axis(torsion, v.tangent.xyz);
				v.vertex.xyz = rotate_vector_at(v.vertex.xyz, center, q);*/

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float dot(i.normal,)
				float d = max(0.0,dot(i.normal,i.viewDir));
				return float4(_Color.rgb,(1.1-d)*weight(i.uv.y*1.01)*2);
			}
			ENDCG
		}
	}
}
