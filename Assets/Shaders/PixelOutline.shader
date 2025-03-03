Shader "Unlit/PixelOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {} // Main texture
        _Color ("Tint", Color) = (1, 1, 1, 1)       // Outline color
        _Radius ("Radius", Range(0, 10)) = 1        // Radius of the outline
        [Toggle] _OutlineOnly("Outline Only", float) = 0 // Toggle outline
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        ZTest Off             // Disable depth testing
        Blend SrcAlpha OneMinusSrcAlpha // Alpha blending for transparency
        Cull Off              // Render both sides of polygons
        ZWrite Off            // Disable depth writing
        LOD 100               // Shader complexity level

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // Include Unity's common functions

            struct appdata
            {
                float4 vertex : POSITION; // Object-space vertex position
                fixed4 color : COLOR;     // Vertex color (not used here)
                float2 uv : TEXCOORD0;    // UV coordinates for texture mapping
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;    // UV coordinates passed to fragment shader
                float4 color : COLOR;     // Color passed to fragment shader
                float4 vertex : SV_POSITION; // Clip-space vertex position
            };

            sampler2D _MainTex;            // Main texture
            float4 _MainTex_ST;           // Texture transform (scale and offset)
            float4 _MainTex_TexelSize;    // Texture texel size (1 / width, 1 / height)
            float4 _Color;                // Outline color
            float _Radius;                // Outline radius
            float _OutlineOnly;             //outline only

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // Transform to clip space
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);      // Transform UV with texture scaling and offset
                o.color = v.color;                        // Pass color through
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float na = 0; // Accumulated alpha from surrounding pixels
                float r = _Radius; // Outline radius

                // Loop through the surrounding pixels
                for (int nx = -r; nx <= r; nx++)
                {
                    for (int ny = -r; ny <= r; ny++)
                    {
                        if (nx * nx + ny * ny <= r * r) // Check if within radius
                        {
                            // Sample neighboring pixels and accumulate alpha
                            fixed4 nc = tex2D(_MainTex, i.uv + float2(_MainTex_TexelSize.x * nx, _MainTex_TexelSize.y * ny));
                            na += ceil(nc.a); // Add 1 if alpha is > 0
                        }
                    }
                }

                na = clamp(na, 0, 1); // Clamp accumulated alpha to range [0, 1]

                fixed4 c = tex2D(_MainTex, i.uv); // Sample the original texture color
                na -= ceil(c.a); // Subtract the alpha of the current pixel

                // Lerp between the original color and the outline color
                if (_OutlineOnly == 0)
                {
                    return lerp(c, _Color, na);
                }
                else
                {
                    return na > 0 ? _Color : fixed4(0, 0, 0, 0);
                }
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
