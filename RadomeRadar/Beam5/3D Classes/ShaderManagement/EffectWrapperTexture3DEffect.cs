﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Runtime.InteropServices;
using System.IO;

namespace Apparat
{
    public class EffectWrapperTexture3DEffect
    {
        public ShaderBytecode effectByteCode;
        public Effect effect;
        public ShaderSignature inputSignature;
        public EffectTechnique technique;
        public EffectPass pass;
        public InputLayout layout;

        public EffectMatrixVariable tmat;
        public EffectMatrixVariable WVInvmat;
        public EffectMatrixVariable WVmat;
        public EffectVectorVariable wireFrameColor;
        public EffectResourceVariable textureResourceVariable;


        InputElement[] elements = new[] { 
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32B32_Float, 12, 0) 
            };


        public void Load()
        {
            try
            {
                string path = Path.Combine(ShaderManager.ShadaresFolder, "texture3DEffect.fx");
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
                WVInvmat = effect.GetVariableByName("gWVInv").AsMatrix();
                WVmat = effect.GetVariableByName("gWV").AsMatrix();
                textureResourceVariable = effect.GetVariableByName("Texture").AsResource();

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