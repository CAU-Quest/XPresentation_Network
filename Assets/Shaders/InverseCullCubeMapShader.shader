
Shader "Unlit/InverseCullCubeMapShader"
{
    Properties
    {
        _CubeMap( "Cube Map", Cube ) = "white" {}
    }
    SubShader
    {
        Pass 
        {
            Tags { "DisableBatching" = "True" }

            Cull Front
            
            ZTest Less
            Zwrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
        
            samplerCUBE _CubeMap;
        
            struct v2f 
            {
                float4 pos : SV_Position;
                half3 uv : TEXCOORD0;
            };
        
            v2f vert( appdata_img v )
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv = v.vertex.xyz * half3(-1,1,1); // mirror so cubemap projects as expected
                return o;
            }
        
            fixed4 frag( v2f i ) : SV_Target 
            {
                return texCUBE( _CubeMap, i.uv );
            }
            ENDCG
        }
        
        Pass
        {
            Tags { "LightMode" = "DepthOnly" }
            Cull Front
            ZTest Always
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return 1;
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode" = "DepthNormals" }
            Cull Front
            ZTest Always
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return 1;
            }
            ENDCG
        }

    }
}


