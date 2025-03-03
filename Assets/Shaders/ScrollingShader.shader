Shader "Custom/ScrollingTextureShader"
{
    Properties
    {
        _MainTex ("Texture Atlas", 2D) = "white" {} // Main texture
        _ScrollSpeed ("Scroll Speed (X, Y)", Float) = 0.1 // Scrolling speed
        _AtlasCut ("Cut Atlas (width, height)", Vector) = (1.0, 1.0, 0, 0) // Number of cuts (width, height)
        _TestTime ("Time sample in second", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency
            Cull Off                       // Render both sides (useful for 2D)
            Lighting Off                   // No lighting for simplicity

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;           // Sprite texture
            float _ScrollSpeed;           // Speed of scrolling (X and Y)
            float4 _AtlasCut;             // Number of atlas rows and columns
            float _TestTime;              // Time for testing purposes

            struct appdata
            {
                float4 vertex : POSITION; // Vertex position
                float2 uv : TEXCOORD0;    // Texture UV coordinates
            };

            struct v2f
            {
                float4 pos : SV_POSITION; // Screen position
                float2 uv : TEXCOORD0;    // UV coordinates passed to the fragment shader
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Convert object position to clip space
                o.uv = v.uv;                           // Pass UV coordinates through
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate sub-region position in the texture atlas
                int subRegionQueue = int((_TestTime / _ScrollSpeed) % (_AtlasCut.x * _AtlasCut.y));
                
                // Use int2 for subRegionPos
                int2 subRegionPos = int2(subRegionQueue % (_AtlasCut.x), subRegionQueue / (_AtlasCut.x));

                // Get size of each sub-region
                float2 regionSize = float2(1.0 / _AtlasCut.x, 1.0 / _AtlasCut.y);

                // Transform the UV coordinates to the sub-region
                float2 subRegionMin = subRegionPos * regionSize;
                float2 subRegionMax = subRegionMin + regionSize;

                // Scale and offset the UVs
                float2 uv = lerp(subRegionMin, subRegionMax, i.uv);

                // Sample the texture at the transformed UV coordinates
                fixed4 texColor = tex2D(_MainTex, uv);

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
