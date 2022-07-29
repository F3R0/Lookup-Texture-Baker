Shader "Custom/Quad"
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
					half2 uv = IN.uv;
					uv *= 1.0;
					half x = uv.x;
					half y = uv.y;
					half col = x;
					x = 
x
						;

#if defined(RESULT_GRAPH)
					
					col = clamp(smoothstep(0.016, 0.0, abs(x-y)),0.0, 1.0);
#else
					
					col = clamp(x, 0.0, 1.0);
#endif

					return half4(col,col,col,1.0);

				}
				ENDHLSL
			}
	}
}
