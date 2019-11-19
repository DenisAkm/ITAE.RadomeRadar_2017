using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Resource = SlimDX.Direct3D11.Resource;
using Device = SlimDX.Direct3D11.Device;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Collections.ObjectModel;


namespace Apparat
{
    public class DeviceManager
    {

        #region Singleton Pattern
        private static DeviceManager instance = null;
        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceManager();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private DeviceManager() { }
        #endregion

        public Device device;
        public SwapChain swapChain;
        public Viewport viewport;
        public RenderTargetView renderTarget;
        public DepthStencilView depthStencil;

        public DeviceContext context;
        
        public System.Windows.Forms.Control form;

        public void getCaps()
        {
            Factory1 fac = new Factory1();
            int numAdapters = fac.GetAdapterCount1();
            List<Adapter1> adapts = new List<Adapter1>();
            for (int i = 0; i < numAdapters; i++)
            {
                adapts.Add(fac.GetAdapter1(i));
            }

            Output outp = adapts[0].GetOutput(0);

            List<Format> formats = new List<Format>();
            foreach (Format format in Enum.GetValues(typeof(Format)))
            {
                formats.Add(format);
            }

            List<ReadOnlyCollection<ModeDescription>> ll = new List<ReadOnlyCollection<ModeDescription>>();

            for (int i = 0; i < formats.Count - 1; i++)
            {
                ReadOnlyCollection<ModeDescription> mdl;
                mdl = outp.GetDisplayModeList(formats[i], DisplayModeEnumerationFlags.Interlaced);
                ll.Add(mdl);
                if (mdl != null)
                {
                    Console.WriteLine(formats[i].ToString());
                }
            }
        }

        public void Init(System.Windows.Forms.Control form)
        {
            CreateDeviceAndSwapChain(form);
            CreateDepthStencilBuffer(form);
        }

        public void CreateDeviceAndSwapChain(System.Windows.Forms.Control form)
        {
            this.form = form;

            var description = new SwapChainDescription()
            {
                BufferCount = 1,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = form.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, description, out device, out swapChain);

            // create a view of our render target, which is the backbuffer of the swap chain we just created
            resource = Resource.FromSwapChain<Texture2D>(swapChain, 0);
            renderTarget = new RenderTargetView(device, resource);

            // setting a viewport is required if you want to actually see anything
            context = device.ImmediateContext;
            viewport = new Viewport(0.0f, 0.0f, form.ClientSize.Width, form.ClientSize.Height);
            context.OutputMerger.SetTargets(renderTarget);
            context.Rasterizer.SetViewports(viewport);

            // prevent DXGI handling of alt+enter, which doesn't work properly with Winforms
            using (var factory = swapChain.GetParent<Factory>())
                factory.SetWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);

            // handle alt+enter ourselves
            form.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                {
                    swapChain.IsFullScreen = !swapChain.IsFullScreen;
                }
            };
        }

        DepthStencilState depthStencilStateNormal;

        public void CreateDepthStencilBuffer(System.Windows.Forms.Control form)
        {
            CreateDepthStencilBuffer(form.Width, form.Height);
        }

        Texture2D DSTexture;
        DepthStencilState dss;
        public void CreateDepthStencilBuffer(int width, int height)
        {
            if (DSTexture != null)
                DSTexture.Dispose();

            DSTexture = new Texture2D(
                device,
                new Texture2DDescription()
                {
                    ArraySize = 1,
                    MipLevels = 1,
                    Format = Format.D32_Float,
                    Width = width,
                    Height = height,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default
                }
            );

            if (depthStencil != null)
                depthStencil.Dispose();

            depthStencil = new DepthStencilView(
               device,
               DSTexture,
               new DepthStencilViewDescription()
               {
                   ArraySize = 0,
                   FirstArraySlice = 0,
                   MipSlice = 0,
                   Format = Format.D32_Float,
                   Dimension = DepthStencilViewDimension.Texture2D
               }
           );

            if (dss != null)
                dss.Dispose();

            dss = DepthStencilState.FromDescription(
               device,
               new DepthStencilStateDescription()
               {
                   DepthComparison = Comparison.Always,
                   DepthWriteMask = DepthWriteMask.All,
                   IsDepthEnabled = true,
                   IsStencilEnabled = false
               }
           );

            context.OutputMerger.DepthStencilState = dss;

            context.OutputMerger.SetTargets(depthStencil, renderTarget);

            DepthStencilStateDescription dssd = new DepthStencilStateDescription
            {
                IsDepthEnabled = true,
                IsStencilEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
            };


            depthStencilStateNormal = DepthStencilState.FromDescription(DeviceManager.Instance.device, dssd);
            DeviceManager.Instance.context.OutputMerger.DepthStencilState = depthStencilStateNormal;
        }

        public void ShutDown()
        {
            renderTarget.Dispose();
            depthStencil.Dispose();
            swapChain.Dispose();
            device.Dispose();
        }

        Texture2D resource;

        internal void Resize()
        {
            try
            {
                if (device == null)
                    return;

                float aspectRatio = (float)form.Width / (float)form.Height;
                CameraManager.Instance.currentCamera.SetPerspective((float)Math.PI / 4, aspectRatio, CameraManager.znearPlane, CameraManager.zfarPlane);

                // Dispose before resizing.
                if (renderTarget != null)
                    renderTarget.Dispose();

                if (resource != null)
                    resource.Dispose();

                if (depthStencil != null)
                    depthStencil.Dispose();

                swapChain.ResizeBuffers(1,
                  form.ClientSize.Width,
                  form.ClientSize.Height,
                  Format.R8G8B8A8_UNorm,
                  SwapChainFlags.AllowModeSwitch);

                resource = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
                renderTarget = new RenderTargetView(device, resource);

                CreateDepthStencilBuffer(form);

                viewport = new Viewport(0.0f, 0.0f, form.ClientSize.Width, form.ClientSize.Height);
                context.Rasterizer.SetViewports(viewport);
                context.OutputMerger.SetTargets(depthStencil, renderTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal void Resize(int width, int height)
        {
            try
            {
                if (device == null)
                    return;

                float aspectRatio = (float)width / (float)height;
                CameraManager.Instance.currentCamera.SetPerspective((float)Math.PI / 4, aspectRatio, CameraManager.znearPlane, CameraManager.zfarPlane);

                // Dispose before resizing.
                if (renderTarget != null)
                    renderTarget.Dispose();

                if (resource != null)
                    resource.Dispose();

                if (depthStencil != null)
                    depthStencil.Dispose();

                swapChain.ResizeBuffers(1,
                  width,
                  height,
                  Format.R8G8B8A8_UNorm,
                  SwapChainFlags.AllowModeSwitch);

                resource = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
                renderTarget = new RenderTargetView(device, resource);

                CreateDepthStencilBuffer(width, height);

                viewport = new Viewport(0.0f, 0.0f, width, height);
                context.Rasterizer.SetViewports(viewport);
                context.OutputMerger.SetTargets(depthStencil, renderTarget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
