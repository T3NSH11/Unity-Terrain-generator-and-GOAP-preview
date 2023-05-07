Shader "Unlit/Shady"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_BaseColor("Color", Color) = (1, 1, 1, 1)
		_DiffuseColor("DIffuse Color", Color) = (1, 1, 1, 1)
		_SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
		_Glossiness("Glossiness", float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog

				#include "UnityCG.cginc"
				#include "UnityLightingCommon.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float2 uv : TEXCOORD0;
					float3 viewDir : TEXCOORD1;
					float3 worldNormal : TEXCOORD2;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float3 viewDir : TEXCOORD1;
					float3 worldNormal : TEXCOORD2;
				};

				sampler2D _MainTex;
				float _Glossiness;
				float4 _MainTex_ST;
				float4 _BaseColor;
				float4 _AmbientColor;
				float4 _DiffuseColor;
				float4 _SpecularColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.viewDir = WorldSpaceViewDir(v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					float3 normalizedNormal = normalize(i.worldNormal);

					float Dot = dot(_WorldSpaceLightPos0, i.worldNormal);
					float NdotL = max(0, Dot);

					fixed4 ambientLight = _BaseColor * UNITY_LIGHTMODEL_AMBIENT;
					fixed4 diffusedLight = fixed4((_LightColor0.rgb * NdotL), 1);

					float3 viewDir = normalize(i.viewDir);
					float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
					float NdotH = dot(normalizedNormal, halfVector);

					float4 specularLight = _SpecularColor * pow(NdotH, _Glossiness * _Glossiness);

					fixed4 light = ambientLight + diffusedLight + specularLight;

					return light;
				}
				ENDCG
			}
		}
}
