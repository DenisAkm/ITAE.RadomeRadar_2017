matrix gWVP;
Texture2D Texture;

struct VS_IN
{
	float4 position : POSITION;
	float2 TexCoord : TEXCOORD;
};

struct PS_IN
{
	float4 position : SV_POSITION;
	float2 TexCoord : TEXCOORD0;
};

PS_IN VShader(VS_IN input)
{
	PS_IN output;

	output.position = mul(input.position, gWVP);
	output.TexCoord = input.TexCoord;

	return output;
}

SamplerState stateLinear
{
	Filter = MIN_MAG_MIP_POINT;
	AddressU = Wrap;
	AddressV = Wrap;
};

float4 PShader(PS_IN input) : SV_TARGET
{
	return Texture.Sample(stateLinear, input.TexCoord);
}


RasterizerState SolidState
{
	FillMode = Solid;
};

technique10 Render
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VShader()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PShader()));
		SetRasterizerState(SolidState);
	}
}