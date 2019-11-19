using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Runtime.InteropServices;
using System.IO;

namespace Apparat.ShaderManagement
{
  public class EffectWrapperTransformEffect
  {
    public ShaderSignature inputSignature;
    public EffectTechnique technique;
    public EffectPass pass;

    public Effect effect;

    public InputLayout layout;
    public EffectMatrixVariable tmat;
    public EffectVectorVariable mCol;

    InputElement[] elements = new[] { new InputElement("POSITION", 0, Format.R32G32B32_Float, 0) };


    public void Load()
    {
        try
        {
            string path = Path.Combine(ShaderManager.ShadaresFolder, "transformEffect.fx");
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

      tmat = effect.GetVariableByName("gWVP").AsMatrix();
      mCol = effect.GetVariableByName("colorSolid").AsVector();
      layout = new InputLayout(DeviceManager.Instance.device, inputSignature, elements);      
    }

    public void Dispose()
    {
        if (effect != null) effect.Dispose();
        if (layout != null) layout.Dispose();
    }
  }
}
