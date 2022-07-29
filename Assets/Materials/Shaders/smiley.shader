Shader "Custom/Smiley"
{
		SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"RenderPipeline" = "UniversalPipeline"
			"Queue" = "Geometry"
		}

		HLSLINCLUDE
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		float4 _BaseColor;

		ENDHLSL

		Pass
		{
			
			HLSLPROGRAM
			#pragma vertex vertShader
			#pragma fragment fragShader
			#pragma shader_feature CUSTOM_INPUT
			#pragma shader_feature RESULT_GRAPH

				struct Attributes
				{
					float4 positionOS	: POSITION;
					float2 uv		    : TEXCOORD0;
				};

				struct Varyings
				{
					float4 positionCS 	: SV_POSITION;
					float2 uv		    : TEXCOORD0;
				};

				Varyings vertShader(Attributes IN)
				{
					Varyings OUT;
					VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
					OUT.positionCS = positionInputs.positionCS;
					OUT.uv = IN.uv;
					return OUT;
				}

				half4 fragShader(Varyings IN) : SV_Target
				{
					half2 uv = IN.uv - 0.5;
					
					float h = round(1.0 - length(uv) - 0.1);
					float eL = round(length(half2(uv.x - 0.15, uv.y -0.1)) + 0.4);
					float eR = round(length(half2(uv.x + 0.15, uv.y -0.1)) + 0.4);

					uv *= 1.1;
					uv = half2(uv.x, uv.y - 0.1);
					float m = 	round(length(uv) +0.15) *
								round(1.0 - length(half2(uv.x, uv.y +0.1)) - 0.15);

					half3 col = half3(h,h,h) * eL * eR -m;
					
					return half4(col.x,col.x-0.2,0.0,1.0);

				}
				ENDHLSL
			}
	}
}
