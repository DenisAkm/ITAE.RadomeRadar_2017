matrix gWVP;

struct VOut
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

VOut VShader(float4 position : POSITION0, float4 color : COLOR, float4 posInstance : POSITION1, float angle : PSIZE2)
{
	VOut output;
	matrix rotMatrixY;
	float angleCos = cos(angle);
	float angleSin = sin(angle);

	rotMatrixY._11 = angleCos;
	rotMatrixY._12 = 0;
	rotMatrixY._13 = -angleSin;
	rotMatrixY._14 = 0;

	rotMatrixY._21 = 0;
	rotMatrixY._22 = 1;
	rotMatrixY._23 = 0;
	rotMatrixY._24 = 0;

	rotMatrixY._31 = angleSin;
	rotMatrixY._32 = 0;
	rotMatrixY._33 = angleCos;
	rotMatrixY._34 = 0;

	rotMatrixY._41 = 0;
	rotMatrixY._42 = 0;
	rotMatrixY._43 = 0;
	rotMatrixY._44 = 1;

	//matrix transform = mul( gWVP, rotMatrixY);
	posInstance.w = 0;
	matrix translation;

	translation._11 = 1;
	translation._12 = 0;
	translation._13 = 0;
	translation._14 = 0;

	translation._21 = 0;
	translation._22 = 1;
	translation._23 = 0;
	translation._24 = 0;

	translation._31 = 0;
	translation._32 = 0;
	translation._33 = 1;
	translation._34 = 0;

	translation._41 = posInstance.x;
	translation._42 = posInstance.y;
	translation._43 = posInstance.z;
	translation._44 = 1;

	matrix transform = mul(rotMatrixY, translation);
	matrix res = mul(transform, gWVP);




	//float4 newPos = position + posInstance;
	//float4 rotPos = mul( newPos, rotMatrixY);

	output.position = mul(position, res);
	output.color = color;

	return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
	return color;
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