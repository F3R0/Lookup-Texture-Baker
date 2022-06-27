Shader "Custom/Unlit-Template"
{
	Properties
	{
		_MainTexture("Example Texture", 2D) = "white" {}
		_BaseColor("Example Colour", Color) = (0, 0.66, 0.73, 1) }

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

			CBUFFER_START(UnityPerMaterial)

			float4 _MainTexture_ST;
			float4 _BaseColor;

			CBUFFER_END
			ENDHLSL

			Pass
			{
				Name "Unlit"

				HLSLPROGRAM
				#pragma vertex vertShader
				#pragma fragment fragShader
#pragma debug

				struct Attributes
				{
					float4 positionOS	: POSITION;
					float2 uv		    : TEXCOORD0;
					float4 color		: COLOR;
				};

				struct Varyings
				{
					float4 positionCS 	: SV_POSITION;
					float2 uv		    : TEXCOORD0;
					float4 color		: COLOR;
				};

				TEXTURE2D(_MainTexture);
				SAMPLER(sampler_MainTexture);

				Varyings vertShader(Attributes IN) 
				{
					Varyings OUT;
					VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
					OUT.positionCS = positionInputs.positionCS;
					OUT.uv = TRANSFORM_TEX(IN.uv, _MainTexture);
					OUT.color = IN.color;
					return OUT;
				}

				half4 fragShader(Varyings IN) : SV_Target
				{
					half4 baseMap = SAMPLE_TEXTURE2D(_MainTexture, sampler_MainTexture, IN.uv);
					return baseMap * _BaseColor * IN.color;
				}
				ENDHLSL
			}
		}
}