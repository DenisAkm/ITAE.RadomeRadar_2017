matrix gWVP;
matrix gWVInv;
matrix gWV;
Texture3D Texture;

struct VS_IN
{
	float4 position : POSITION;
	float3 TexCoord : TEXCOORD;
};

struct PS_IN
{
	float4 position : SV_POSITION;
	float3 TexCoord : TEXCOORD0;
	float4 posView  : TEXCOORD1;
};

PS_IN VShader(VS_IN input)
{
	PS_IN output;

	output.position = mul(input.position, gWVP);
	output.TexCoord = input.TexCoord;
	output.posView = mul(input.position, gWV);

	return output;
}

SamplerState stateLinear
{
	Filter = MIN_MAG_MIP_POINT;
	//Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
	AddressW = Mirror;
	//BorderColor = float4(0,0,0,0);
};

float4 PShader(PS_IN input) : SV_TARGET
{
	//float3 vDir = float3(0,0,0.05);
	//float4 vDir4 = normalize( input.posView ) * 0.02;
	float4 vDir4 = float4(input.posView.x, input.posView.y, input.posView.z, 1);
	vDir4 = mul(vDir4, gWVInv);
	float3 vDir = float3(vDir4.x, vDir4.y, vDir4.z);
		vDir = normalize(vDir) * 0.007;

	vDir.y *= -1;

	float3 currTexCoord = input.TexCoord;

		float4 col = float4(0, 0, 0, 0);
		bool found = false;

	float4 colSum = float4(0, 0, 0, 0);
		uint i;
	[loop]
	for (i = 0; i < 200; i++)
	{
		if (currTexCoord.x > 0 && currTexCoord.x < 1 &&
			currTexCoord.y > 0 && currTexCoord.y < 1 &&
			currTexCoord.z > 0 && currTexCoord.z < 1)
		{

			col = Texture.Sample(stateLinear, currTexCoord);




			if (col[3] > 0.5 && !found)
			{
				found = true;
				colSum = col;
			}
			//colSum += col;
			//break;
		}
		currTexCoord += vDir;
	}

	if (!found)
		discard;

	return colSum;


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