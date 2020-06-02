using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SharpDX;
using SharpDX.DXGI;
using System.Diagnostics;
using SharpDX.Direct3D11;
using Apparat;
using System.Drawing;
using Color = SharpDX.Color;

namespace Apparat
{
    public class RenderManager
    {
        #region Singleton Pattern
        private static RenderManager instance = null;
        public static RenderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderManager();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private RenderManager() { }
        #endregion
        
        Thread renderThread;
       
        int syncInterval = 1;

        public void SwitchSyncInterval()
        {
                if (syncInterval == 0)
                {
                    syncInterval = 1;
                }
                else if (syncInterval == 1)
                {
                    syncInterval = 0;
                }
        }

        FrameCounter fc = FrameCounter.Instance;
        Screenshots screenShots = new Screenshots();

        public bool resize = false;
        public bool makeScreenshot = false;

        public void RenderScene()
        {
            while (true)
            {
                if (resize)
                {
                    DeviceManager.Instance.Resize();
                    resize = false;
                }

                fc.Count();

                DeviceManager dm = DeviceManager.Instance;
                dm.context.ClearDepthStencilView(dm.depthStencil,
                  DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
                  1.0f,
                  0);

                dm.context.ClearRenderTargetView(dm.renderTarget,
                  new Color4(Color3.Black));//0.75f, 0.75f, 0.75f

                Scene.Instance.Render();

                dm.swapChain.Present(syncInterval, PresentFlags.None);

                if (makeScreenshot)
                {
                    //screenShots.MakeScreenshot(DeviceManager.Instance, ImageFileFormat.Jpg);
                    // screenShots.MakeScreenshot(DeviceManager.Instance, Scene.Instance, 3000, 2000, ImageFileFormat.Jpg, "screenShot");
                    makeScreenshot = false;
                }
            }
        }

        public void Init()
        {
            renderThread = new Thread(new ThreadStart(RenderScene));
            renderThread.Start();            
        }

        public void Pause()
        {
            if (renderThread != null)
            {
                renderThread.Abort();
            }        
        }

        public void ShutDown()
        {
            renderThread.Abort();
        }

       
    }
}
