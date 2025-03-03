Shader "Custom/Sprite2dVariant"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {} // The main sprite texture
        _Color ("Tint Color", Color) = (1, 1, 1, 1)  // Tint color
        _AlphaThreshold ("Alpha Threshold", Range(0.0, 1.0)) = 1.0 // Alpha Threshold
        _ClipOffRangeInCircle ("Clip Circle (center.xy, radius)", Vector) = (0.5, 0.5, 0.0, 0.0) // Circle clip settings
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
            Cull Off                       // Render both sides
            Lighting Off                   // No lighting for 2D sprites

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;            // The main texture
            fixed4 _Color;                 // Tint color
            float _AlphaThreshold;         // Alpha threshold
            float4 _ClipOffRangeInCircle;  // Circle clipping range (center.xy, radius)
            half4 _MainTex_TexelSize; //dunno

            struct appdata
            {
                float4 vertex : POSITION; // Vertex position
                float2 uv : TEXCOORD0;    // UV coordinates
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // Screen position
                float2 uv : TEXCOORD0;    // UV coordinates passed to fragment
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transform to clip space
                o.uv = v.uv;                           // Pass UV coordinates
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Get the size of a single texel using the built-in texel size property
                float2 texelSize = _MainTex_TexelSize.xy;

                // Snap UV coordinates to the nearest texel
                float2 snappedUV = floor(i.uv / texelSize) * texelSize + texelSize * 0.5;

                // Sample the texture using the snapped UV coordinates
                fixed4 texColor = tex2D(_MainTex, snappedUV);

                // Apply tint color
                fixed4 finalColor = texColor * _Color;

                // Check transparency
                if (finalColor.a < _AlphaThreshold)
                    discard;

                // Circle clip logic
                float dist = distance(snappedUV, _ClipOffRangeInCircle.xy);
                if (dist < _ClipOffRangeInCircle.z)
                    discard;

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Sprites/Default"
}
