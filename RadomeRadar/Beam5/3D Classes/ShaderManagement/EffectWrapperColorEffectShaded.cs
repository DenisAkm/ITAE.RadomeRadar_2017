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
    public class EffectWrapperColorEffectShaded
    {
        public ShaderBytecode effectByteCode;
        public Effect effect;
        public ShaderBytecode inputSignature;
        public EffectTechnique technique;
        public EffectPass pass;
        public InputLayout layout;

        public EffectMatrixVariable tmat;
        public EffectMatrixVariable invW;
        public EffectVectorVariable lightPos;


        InputElement[] elements = new[] { 
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
                new InputElement("COLOR", 0, Format.B8G8R8A8_UNorm, 12, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 28, 0)
            };


        public void Load()
        {
            try
            {
                string path = Path.Combine(ShaderManager.ShadaresFolder, "ColorEffectShaded.fx");
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

                tmat = effect.GetVariableByName("gWVP").AsMatrix();
                invW = effect.GetVariableByName("inv_world_matrix").AsMatrix();
                lightPos = effect.GetVariableByName("LightPosition").AsVector();
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
