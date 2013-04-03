sampler TextureSampler : register(s0);

float2 Center = { 0.5, 0.5 }; ///center of the screen (could be any place)
float BlurStart = 1.0f; /// blur offset
float BlurWidth = -0.1; ///how big it should be
int nsamples = 10;

float4 PS_RadialBlur(float2 UV : TEXCOORD0 ) : COLOR
{
    UV -= Center;
    float4 c = 0;
    for(int i = 0; i < 20; i++) 
 {
     float scale = BlurStart + BlurWidth * ( i /(float) (nsamples-1));
     c += tex2D(TextureSampler, UV * scale + Center );
    }
    c = c / nsamples;
    return c;
}

technique GaussianBlur2
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PS_RadialBlur();
    }
}