Shader "Mobile Buildings" {
    Properties {  
      _MainTex ("Texture", 2D) = "white" {} 
      _Cube ("Cubemap", CUBE) = "" {}
      _WindowColor ("Window Color", Color) = (.5,.5,.5,1)  
      _ReflStrength("Reflection Strength", Range(0.0, 5.0)) = 1.5   
    }  
    SubShader {  
      Tags { "RenderType" = "Opaque" }    
      CGPROGRAM   
      #pragma surface surf Lambert  
      fixed _ReflStrength;

      struct Input
      {  
		  float2 uv_MainTex; 
          float3 worldRefl;
          float3 viewDir; 
          float3 color : COLOR; 
      };  
      sampler2D _MainTex;   
      samplerCUBE _Cube; 
      float4 _WindowColor; 

      void surf (Input IN, inout SurfaceOutput o) {   
      
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);    	  	
		o.Albedo = lerp( c.rgb, _WindowColor.rgb * c.rgb, c.a);                      
		o.Emission = texCUBE (_Cube, IN.worldRefl).rgb * _ReflStrength * lerp( c.a, _WindowColor.rgb * 1.5/*c.rgb*/, c.a);      
      }  
      ENDCG  
    }   
    Fallback "Diffuse"  
}


