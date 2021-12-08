Shader "Custom/3D_Mask" 
{
    SubShader
    {
        Tags { "Queue" = "Transparent+1" }
        
        Pass 
        {
            Blend Zero One
        }
    }
    //FallBack "Diffuse"
}