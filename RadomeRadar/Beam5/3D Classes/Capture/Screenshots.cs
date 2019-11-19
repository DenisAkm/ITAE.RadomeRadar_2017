using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX;

namespace Apparat
{
  public class Screenshots
  {
    string fileNamePrefix = "Screenshot.";
    string fileNameFormatPostFix = "png";
    string defaultScreenshotFileName = "Screenshot.png";

    /// <summary>
    /// Makes a Screenshot with default parameters.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    public void MakeScreenshot(DeviceManager dm)
    {
      try
      {
        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context,
          dm.renderTarget.Resource,
          SlimDX.Direct3D11.ImageFileFormat.Jpg,
          defaultScreenshotFileName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="fileName">Filename.</param>
    public void MakeScreenshot(DeviceManager dm, String fileName)
    {
      try
      {
        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context, dm.renderTarget.Resource, SlimDX.Direct3D11.ImageFileFormat.Png, fileName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }


    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="format">Image file format (bmp, jpg, png)</param>
    public void MakeScreenshot(DeviceManager dm, SlimDX.Direct3D11.ImageFileFormat format)
    {
      try
      {
        String fileName = fileNamePrefix + format.ToString();
        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context, dm.renderTarget.Resource, format, fileName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="format">Image file format (bmp, jpg, png)</param>
    /// <param name="fileName">Filename.</param>
    public void MakeScreenshot(DeviceManager dm, SlimDX.Direct3D11.ImageFileFormat format, String fileName)
    {
      try
      {
        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context, dm.renderTarget.Resource, format, fileName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }


    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="scene">Scene to render.</param>
    /// <param name="width">Width of the screenshot in pixel.</param>
    /// <param name="height">Height of the screenshot in pixel.</param>
    public void MakeScreenshot(DeviceManager dm, Scene scene, int width, int height)
    {
      try
      {
        Viewport OriginalViewport = dm.viewport;
        dm.Resize(width, height);
        dm.context.ClearDepthStencilView(dm.depthStencil, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
        dm.context.ClearRenderTargetView(dm.renderTarget, new Color4(0.75f, 0.75f, 0.75f));
        scene.Render();

        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context, dm.renderTarget.Resource, SlimDX.Direct3D11.ImageFileFormat.Png, defaultScreenshotFileName);

        dm.Resize((int)OriginalViewport.Width, (int)OriginalViewport.Height);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }


    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="scene">Scene to render.</param>
    /// <param name="width">Width of the screenshot in pixel.</param>
    /// <param name="height">Height of the screenshot in pixel.</param>
    /// <param name="fileName">Filename.</param>
    public void MakeScreenshot(DeviceManager dm, Scene scene, int width, int height, string fileName)
    {
      try
      {
        Viewport OriginalViewport = dm.viewport;
        dm.Resize(width, height);
        dm.context.ClearDepthStencilView(dm.depthStencil, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
        dm.context.ClearRenderTargetView(dm.renderTarget, new Color4(0.75f, 0.75f, 0.75f));
        scene.Render();

        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context, dm.renderTarget.Resource, SlimDX.Direct3D11.ImageFileFormat.Png, fileName);

        dm.Resize((int)OriginalViewport.Width, (int)OriginalViewport.Height);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    /// <summary>
    /// Makes a Screenshot.
    /// </summary>
    /// <param name="dm">DeviceManager.</param>
    /// <param name="scene">Scene to render.</param>
    /// <param name="width">Width of the screenshot in pixel.</param>
    /// <param name="height">Height of the screenshot in pixel.</param>
    /// <param name="format">Image file format (bmp, jpg, png)</param>
    /// <param name="fileName">Filename.</param>
    public void MakeScreenshot(DeviceManager dm, Scene scene, int width, int height, SlimDX.Direct3D11.ImageFileFormat format, string fileName)
    {
      try
      {
        Viewport OriginalViewport = dm.viewport;
        dm.Resize(width, height);
        dm.context.ClearDepthStencilView(dm.depthStencil,
          DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
          1.0f, 0);
        dm.context.ClearRenderTargetView(dm.renderTarget,
          new Color4(0.75f, 0.75f, 0.75f));
        scene.Render();

        SlimDX.Direct3D11.Resource.SaveTextureToFile(dm.context,
          dm.renderTarget.Resource,
          format,
          fileName +  "." + format.ToString());

        dm.Resize((int)OriginalViewport.Width, (int)OriginalViewport.Height);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }





  }
}
