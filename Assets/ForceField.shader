Shader "Custom/ForceField" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Intensity("Intensity", Range(0, 10)) = 1
        _Speed("Speed", Range(0, 10)) = 1
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Standard alpha

            struct Input {
                float3 worldPos;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _Intensity;
            float _Speed;
            float _Cutoff;

            void surf(Input IN, inout SurfaceOutputStandard o) {
                // Calculate a noise value based on the world position and time
                float noise = tex2D(_MainTex, IN.worldPos.xz * _Speed + _Time.y * 0.1).r;

                // Calculate the strength of the force field based on the noise value and the intensity property
                float strength = pow(noise, 3) * _Intensity;

                // Set the surface color based on the color property and the strength of the force field
                fixed4 c = _Color;
                c.a = strength;
                o.Albedo = c.rgb;
                o.Alpha = c.a;

                // Set the cutoff for transparency based on the cutoff property
                clip(c.a - _Cutoff);
            }
            ENDCG
        }
            FallBack "Diffuse"
}