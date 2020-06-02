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
  public class EffectWrapperColorEffectWireframe
  {
    public ShaderBytecode effectByteCode;
    public Effect effect;
    public ShaderBytecode inputSignature;
    public EffectTechnique technique;
    public EffectPass pass;
    public InputLayout layout;

    public EffectMatrixVariable tmat;
    public EffectVectorVariable wireFrameColor;


    InputElement[] elements = new[] { 
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
                new InputElement("COLOR", 0, Format.B8G8R8A8_UNorm, 12, 0) 
            };


    public void Load()
    {
        try
        {
            string path = Path.Combine(ShaderManager.ShadaresFolder, "colorEffectWireframe.fx");
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
            wireFrameColor = effect.GetVariableByName("wireFrameColor").AsVector();

            //Vector4 col = new Vector4(0, 0, 0, 1);
            //wireFrameColor.Set(col);
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
