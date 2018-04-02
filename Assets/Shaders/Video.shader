// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/Video"
{
	Properties
	{
		_MainTex("Render Texture (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Alpha("Alpha", Range(0,1)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
		}

		CGPROGRAM

		#pragma surface surf Unlit alpha:fade
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _Color;
		float _Alpha;

		// *** STRUCTS
		
		struct Input
		{
			float2 uv_MainTex;
		};

		// *** SURF FUNCTION

		void surf(Input IN, inout SurfaceOutput output)
		{
			// Texture info
			float4 texInfo = tex2D(_MainTex, IN.uv_MainTex);
			// Output setup
			output.Albedo = texInfo.rgb * _Color;
			output.Alpha = _Alpha;
		}

		// *** LIGHTING MODEL

		float4 LightingUnlit(SurfaceOutput output, float3 lightDirection, float3 lightAttem)
		{
			// Color calculation
			float4 col;
			
			col.rgb = output.Albedo * 1;
			col.a = output.Alpha;
			
			return col;
		}

		ENDCG
	}

	FallBack "Diffuse"
}
