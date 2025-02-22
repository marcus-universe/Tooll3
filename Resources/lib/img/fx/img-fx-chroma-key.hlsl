//RWTexture2D<float4> outputTexture : register(u0);
Texture2D<float4> inputTexture : register(t0);
sampler texSampler : register(s0);

cbuffer ParamConstants : register(b0)
{
    float4 Colorize;
    float4 Background;
    float Exposure;

    float WeightHue;
    float WeightSaturation;
    float WeightBrightness;
    float Amplify;
}


cbuffer TimeConstants : register(b1)
{
    float globalTime;
    float time;
    float runTime;
    float beatTime;
}

struct vsOutput
{
    float4 position : SV_POSITION;
    float2 texCoord : TEXCOORD;
};


#define mod(x,y) (x-y*floor(x/y))


float3 hsb2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z < 0.5 ?
        //float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
        c.z*2 * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y)
        : lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), lerp(c.y, 0, (c.z * 2 - 1) ) );
}


float3 rgb2hsb(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(
        abs(q.z + (q.w - q.y) / (6.0 * d + e)), 
        d / (q.x + e), 
        q.x*0.5);  
}
static float PI = 3.141578;


float SCurve (float value, float amount, float correction) 
{	
    float curve = (value < 0.5)
    ?   pow(value, amount) * pow(2.0, amount) * 0.5:
    	1.0 - pow(1.0 - value, amount) * pow(2.0, amount) * 0.5; 

    return pow(curve, correction);
}


float4 psMain(vsOutput psInput) : SV_TARGET
{
    float2 uv = psInput.texCoord;
    float4 c = inputTexture.SampleLevel(texSampler, uv, 0.0);

    float3 hsb = rgb2hsb(c.rgb);

    //return float4(hsb.yyy,1);

    float3 keyColor = rgb2hsb(Colorize.rgb);
    float3 weights = float3(WeightHue, WeightSaturation, WeightBrightness);
    float distance = saturate(length(hsb * weights - keyColor * weights) * Exposure - Amplify );
    //return float4(distance.xxx,1);
    //float k = Exposure- distance;
    
    return float4(c.rgb, c.a * distance);

}
