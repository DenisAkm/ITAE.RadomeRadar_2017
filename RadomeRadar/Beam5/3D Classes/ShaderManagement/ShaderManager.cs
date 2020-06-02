using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apparat.ShaderManagement;
using System.IO;

namespace Apparat
{
  public class ShaderManager
  {
      //public string shaderPath = Path.Combine(Environment.CurrentDirectory, "Shaders");
        #region Singleton Pattern
    public static string ShadaresFolder { get; set; }
    private static ShaderManager instance = null;
    public static ShaderManager Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new ShaderManager();
        }
        return instance;
      }
    }
    #endregion

    #region Constructor
    private ShaderManager() { }
    #endregion

    //public EffectWrapperTransformEffectWireframe transformEffectWireFrame;
    public EffectWrapperTransformEffect transformEffect;    
    public EffectWrapperColorEffectWireframe colorEffectWireframe;
    //public EffectWrapperTextureEffect textureEffect;
    //public EffectWrapperTexture3DEffect texture3DEffect;
    //public EffectWrapperColorEffectInstanced colorEffectInstanced;
    //public EffectWrapperColorEffectShaded colorEffectShaded;
    //public EffectWrapperEffectShaded effectShaded;
    //public EffectWrapperPhongBlinn effectPhongBlinn;
    //public EffectWrapperPhongTexture effectPhongTexture;
    //public EffectWrapperColorEffectFlat colorEffectFlat;

    public void Init()
    {
            //transformEffectWireFrame = new EffectWrapperTransformEffectWireframe();
            transformEffect = new EffectWrapperTransformEffect();
            colorEffectWireframe = new EffectWrapperColorEffectWireframe();
            //textureEffect = new EffectWrapperTextureEffect();
            //texture3DEffect = new EffectWrapperTexture3DEffect();
            //colorEffectInstanced = new EffectWrapperColorEffectInstanced();
            //colorEffectShaded = new EffectWrapperColorEffectShaded();
            //effectShaded = new EffectWrapperEffectShaded();
            //effectPhongBlinn = new EffectWrapperPhongBlinn();
            //effectPhongTexture = new EffectWrapperPhongTexture();
            //colorEffectFlat = new EffectWrapperColorEffectFlat();
        }

    public void ShutDown()
    {
            //if (transformEffectWireFrame != null) transformEffectWireFrame.Dispose();
            if (transformEffect != null) transformEffect.Dispose();
            if (colorEffectWireframe != null) colorEffectWireframe.Dispose();
            //if (textureEffect != null) textureEffect.Dispose();
            //if (texture3DEffect != null) texture3DEffect.Dispose();
            //if (colorEffectInstanced != null) colorEffectInstanced.Dispose();
            //if (colorEffectShaded != null) colorEffectShaded.Dispose();
            //if (effectShaded != null) effectShaded.Dispose();
            //if (effectPhongBlinn != null) effectPhongBlinn.Dispose();
            //if (effectPhongTexture != null) effectPhongTexture.Dispose();
            //if (colorEffectFlat != null) colorEffectFlat.Dispose();
        }



    public void LoadShaders()
    {
            //transformEffectWireFrame.Load();
            transformEffect.Load();
            colorEffectWireframe.Load();
            //textureEffect.Load();
            //texture3DEffect.Load();
            //colorEffectInstanced.Load();
            //colorEffectShaded.Load();
            //effectShaded.Load();
            //effectPhongBlinn.Load();
            //effectPhongTexture.Load();
            //colorEffectFlat.Load();
        }

  }
}
