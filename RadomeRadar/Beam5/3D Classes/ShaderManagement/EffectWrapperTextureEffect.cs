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

namespace Apparat.ShaderManagement
{
    public class EffectWrapperTextureEffect
    {
        public ShaderBytecode effectByteCode;
        public Effect effect;
        public ShaderBytecode inputSignature;
        public EffectTechnique technique;
        public EffectPass pass;
        public InputLayout layout;

        public EffectMatrixVariable tmat;
        public EffectVectorVariable wireFrameColor;
        public EffectShaderResourceVariable textureResourceVariable;


        InputElement[] elements = new[] { 
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0) 
            };


        public void Load()
        {
            try
            {
                string path = Path.Combine(ShaderManager.ShadaresFolder, "textureEffect.fx");
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

                tmat = effect.GetVariableByName("gWVP").AsMatrix();
                textureResourceVariable = effect.GetVariableByName("Texture").AsShaderResource();

                layout = new InputLayout(DeviceManager.Instance.device, inputSignature, elements);


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
