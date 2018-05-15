Shader "Custom/3DText" {
	Properties {
		_Color ("Text Color", Color) = (1,1,1,1)
		_MainTex ("Font Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True"}
		//Lighting Off
		Cull Off
		ZWrite Off
		Fog {Mode Off}
		Blend SrcAlpha OneMinusSrcAlpha
		Pass {
			Lighting Off
			Color [_Color]
			SetTexture [_MainTex] {
				combine primary, texture*primary
			}
		}
	}
}
