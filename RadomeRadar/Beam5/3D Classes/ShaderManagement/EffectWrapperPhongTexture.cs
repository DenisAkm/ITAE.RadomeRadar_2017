using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.D3DCompiler;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;
using System.IO;

namespace Apparat
{
    public class EffectWrapperPhongTexture
    {
        public ShaderBytecode effectByteCode;
        public Effect effect;
        public ShaderBytecode inputSignature;
        public EffectTechnique technique;
        public EffectPass pass;
        public InputLayout layout;


        public EffectMatrixVariable World;
        public EffectMatrixVariable View;
        public EffectMatrixVariable Projection;
        public EffectVectorVariable CameraPosition;
        public EffectVectorVariable LightDirection;

        // Colors
        public EffectVectorVariable AmbientColor;
        public EffectVectorVariable DiffuseColor;
        public EffectVectorVariable SpecularColor;
        public EffectVectorVariable LightColor;

        public EffectScalarVariable SpecularPower;

        // Texture
        public EffectShaderResourceVariable TextureVariable;

        InputElement[] elements = new[] { 
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 24, 0) 
            };


        public void Load()
        {
            try
            {
                string path = Path.Combine(ShaderManager.ShadaresFolder, "PhongTextured.fx");
                using (ShaderBytecode effectByteCode = ShaderBytecode.CompileFromFile(
                    path,
                    "Render",
                    "fx_5_0",
                    ShaderFlags.EnableStrictness,
                    EffectFlags.None))
                {
                    effect = new Effect(DeviceManager.Instance.device, effectByteCode);
                    technique = effect.GetTechniqueByIndex(0);
                    pass = technique.GetPassByIndex(0);
                    inputSignature = pass.Description.Signature;
                }

                layout = new InputLayout(DeviceManager.Instance.device, inputSignature, elements);

                //WorldViewProjection = effect.GetVariableByName("matWorldViewProj").AsMatrix();
                World = effect.GetVariableByName("World").AsMatrix();
                View = effect.GetVariableByName("View").AsMatrix();
                Projection = effect.GetVariableByName("Projection").AsMatrix();

                //LightDir = effect.GetVariableByName("vecLightDir").AsVector();
                CameraPosition = effect.GetVariableByName("CameraPosition").AsVector();
                LightDirection = effect.GetVariableByName("LightDirection").AsVector();

                DiffuseColor = effect.GetVariableByName("DiffuseColor").AsVector();
                AmbientColor = effect.GetVariableByName("AmbientColor").AsVector();
                SpecularColor = effect.GetVariableByName("SpecularColor").AsVector();
                LightColor = effect.GetVariableByName("LightColor").AsVector();

                SpecularPower = effect.GetVariableByName("SpecularPower").AsScalar();

                TextureVariable = effect.GetVariableByName("Texture").AsShaderResource();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            if (effect != null) effect.Dispose();
            if (layout != null) layout.Dispose();
        }



    }
}
