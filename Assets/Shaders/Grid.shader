Shader "Stufco/MeterGridSurface"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _LineColor ("Line Color", Color) = (0,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _LineThickness ("Line Thickness", Range(0, 1)) = 0.1
        _LineThicknessY ("Line Thickness Y", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha:fade

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
            float3 worldNormal;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _LineColor;
        half _LineThickness;
        half _LineThicknessY;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = fixed4(1,1,1,1);//tex2D (_MainTex, IN.uv_MainTex) * _Color;
            half offset = 0.5 + _LineThickness * 0.5;
            c -= abs(dot(IN.worldNormal, float3(1,0,0))) >= 0.99 ? 0 : (IN.worldPos.x + offset - floor(IN.worldPos.x + offset)) > _LineThickness ? 0 : 1;//abs(dot(IN.worldNormal, float3(1,0,0))) < 0.9 ? 0 : 1;
            c -= abs(dot(IN.worldNormal, float3(0,1,0))) >= 0.99 ? 0 : (IN.worldPos.y - floor(IN.worldPos.y)) > _LineThicknessY ? 0 : 1;//abs(dot(IN.worldNormal, float3(0,1,0))) < 0.9 ? 0 : 1;
            c -= abs(dot(IN.worldNormal, float3(0,0,1))) >= 0.99 ? 0 : (IN.worldPos.z + offset - floor(IN.worldPos.z + offset)) > _LineThickness ? 0 : 1;//abs(dot(IN.worldNormal, float3(0,0,1))) < 0.9 ? 0 : 1;
            c.rgb = c.r < 0.5 ? _LineColor : _Color;
            c.a = c.r < 0.5 ? _LineColor.a : 0;
            //TEXTURE STUFF NOT CURRENTLY WORKING!!!!!!
            fixed4 tex = abs(dot(IN.worldNormal, float3(1,0,0))) > 0.01 ? tex2D(_MainTex, IN.worldPos.xy) : 
                            abs(dot(IN.worldNormal, float3(0,1,0))) > 0.01 ? tex2D(_MainTex, IN.worldPos.xz) :
                            tex2D(_MainTex, IN.worldPos.yz);
            o.Albedo = c.rgb * tex;//tex2D(_MainTex, IN.uv_MainTex);//
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}