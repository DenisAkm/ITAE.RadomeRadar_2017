matrix gWVP;
matrix inv_world_matrix;
float4 LightPosition;
float4 color;

struct VOut
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

VOut VShader(float4 position : POSITION, float3 inNormal : NORMAL)
{
	VOut output;

	output.position = mul(position, gWVP);
	float4 col = color;
		//float4 col = float4(1.0, 0.0, 1.0, 0.0);
		inNormal = normalize(inNormal);

	// Determine the light vector
	// first get the light vector in object space
	vector obj_light = mul(LightPosition, inv_world_matrix);
	vector LightDir = normalize(obj_light - position);

	// Diffuse using Lambert
	float DiffuseAttn = max(0, dot(inNormal, LightDir));

	// Compute final lighting
	// assume white light
	vector light = { 0.7, 0.7, 0.7, 1 };
	col += light*DiffuseAttn;

	output.color = col;
	return output;
}

float4 PShader(float4 position : SV_POSITION, float4 color : COLOR) : SV_TARGET
{
	return color;
}

RasterizerState WireframeState
{
	FillMode = Solid;
	CullMode = None;
	FrontCounterClockwise = false;
};

technique10 Render
{
	pass P0
	{
		SetVertexShader(CompileShader(vs_4_0, VShader()));
		SetGeometryShader(NULL);
		SetPixelShader(CompileShader(ps_4_0, PShader()));
		SetRasterizerState(WireframeState);

	}
}