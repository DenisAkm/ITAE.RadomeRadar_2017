using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SharpDX;
using System.IO;
using Color = SharpDX.Color;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace Apparat
{
    public partial class RenderControl : UserControl
    {
        public RenderControl()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(RenderControl_MouseWheel);
            FrameCounter.Instance.FPSCalculatedEvent += new FrameCounter.FPSCalculatedHandler(Instance_FPSCalculatedEvent);
            
        }

        readonly List<RadomeMesh> RadomMeshCollection = new List<RadomeMesh>();
        AntennaMesh Antenna;
        readonly List<FarFieldRender> FarFieldArcs = new List<FarFieldRender>();
        readonly List<ScanRegionRender> ScanningPoints = new List<ScanRegionRender>();


        Grid Ground;
        CoordSystem Arrows;

        const float phi = 30;
        const float theta = 50;

        Vector3[] LastLoadedLook = new Vector3[3];

        delegate void setFPS(string fps);
        void Instance_FPSCalculatedEvent(string fps)
        {
            if (this.InvokeRequired)
            {
                setFPS d = new setFPS(Instance_FPSCalculatedEvent);
                this.Invoke(d, new object[] { fps });
            }
            else
            {
                this.DebugTextLabel.Text = fps;
            }
        }

        public void Pause()
        {
            
            RenderManager.Instance.Pause();                        
        }
        internal void Play()
        {            
            RenderManager.Instance.Init();                        
        }
        public void Init()
        {
            DeviceManager.Instance.Init(this);

            ShaderManager.Instance.Init();            
            
            ShaderManager.ShadaresFolder = Path.Combine(Environment.CurrentDirectory, "Shaders");
            ShaderManager.Instance.LoadShaders();

            RenderManager.Instance.Init();

            Arrows = new CoordSystem(1.3f);
            Ground = new Grid(1f);

            Scene.Instance.addRenderObject(Ground);
            Scene.Instance.addRenderObject(Arrows);
        }
        public void ShutDown()
        {
            RenderManager.Instance.ShutDown();
            ShaderManager.Instance.ShutDown();
            DeviceManager.Instance.ShutDown();
        }



        public float ScaleFactor { get; set; }



        //Методы отрисовки объектов

        #region ScanRegion
        /// <summary>
        /// Отрисовка точек сканировния
        /// </summary>
        /// <param name="lable"></param>
        /// <param name="startTheta"></param>
        /// <param name="finishTheta"></param>
        /// <param name="stepTheta"></param>
        /// <param name="startPhi"></param>
        /// <param name="finishPhi"></param>
        /// <param name="stepPhi"></param>
        /// <param name="sys"></param>
        public void Draw(string lable, double startTheta, double finishTheta, double stepTheta, double startPhi, double finishPhi, double stepPhi, int sys, Color color)
        {
            List<Point3D> list = new List<Point3D>();

            if (stepPhi < 1)
            {
                stepPhi = 1;
            }

            int pointsThetaCount = Convert.ToInt32((finishTheta - startTheta) / stepTheta) + 1;
            int pointsPhiCount = Convert.ToInt32((finishPhi - startPhi) / stepPhi) + 1;

            for (int iTheta = 0; iTheta < pointsThetaCount; iTheta++)
            {
                for (int iPhi = 0; iPhi < pointsPhiCount; iPhi++)
                {
                    double thetaLocal = startTheta + iTheta * stepTheta;
                    double phiLocal = startPhi + iPhi * stepPhi;

                    double thetaG = Logic.GetThetaGlobal(phiLocal, thetaLocal, sys);
                    double phiG = Logic.GetPhiGlobal(phiLocal, thetaLocal, sys);

                    double a = Math.Sin(thetaG * Math.PI / 180) * Math.Cos(phiG * Math.PI / 180);
                    double b = Math.Sin(thetaG * Math.PI / 180) * Math.Sin(phiG * Math.PI / 180);
                    double c = Math.Cos(thetaG * Math.PI / 180);

                    list.Add(new Point3D(a, b, c));
                }
            }

            int count = ScanningPoints.Count;
            bool match = false;

            for (int i = 0; i < count; i++)
            {
                if (ScanningPoints[i].Title == lable)
                {
                    Scene.Instance.removeRenderObject(ScanningPoints[i]);
                    ScanningPoints[i] = new ScanRegionRender(Logic.Instance.RenderScale, lable, list, color);
                    Scene.Instance.addRenderObject(ScanningPoints[i]);
                    match = true;
                }
            }
            if (!match)
            {
                ScanRegionRender points = new ScanRegionRender(Logic.Instance.RenderScale, lable, list, color);
                ScanningPoints.Add(points);
                Scene.Instance.addRenderObject(points);
            }
        }

        public void Draw(string lable, Point3D center, DVector n, bool include1, Point3D a1p1, Point3D a1p2, double angle1Start, double angle1Finish, double angle1Step, bool include2, Point3D a2p1, Point3D a2p2, double angle2Start, double angle2Finish, double angle2Step, Color color)
        {
            bool match = false;
            List<Point3D> scannigPointsList = new List<Point3D>();
            int numberAngle1 = Convert.ToInt32((angle1Finish - angle1Start) / angle1Step) + 1;
            int numberAngle2 = Convert.ToInt32((angle2Finish - angle2Start) / angle2Step) + 1;


            for (int i = 0; i < numberAngle1; i++)
            {
                for (int j = 0; j < numberAngle2; j++)
                {
                    Point3D scPoint = new Point3D(1 * n.X, 1 * n.Y, 1 * n.Z);
                    double Angle1 = angle1Start + i * angle1Step;
                    double Angle2 = angle2Start + j * angle2Step;
                    Point3D a2p1_rot = new Point3D(a2p1);
                    Point3D a2p2_rot = new Point3D(a2p2);
                    if (include1)
                    {
                        scPoint = Logic.Instance.RotateElement(scPoint, Angle1, a1p1.X, a1p1.Y, a1p1.Z, a1p2.X, a1p2.Y, a1p2.Z);
                    }
                    if (include2)
                    {
                        if (include1)
                        {
                            a2p1_rot = Logic.Instance.RotateElement(a2p1, Angle1, a1p1.X, a1p1.Y, a1p1.Z, a1p2.X, a1p2.Y, a1p2.Z);
                            a2p2_rot = Logic.Instance.RotateElement(a2p2, Angle1, a1p1.X, a1p1.Y, a1p1.Z, a1p2.X, a1p2.Y, a1p2.Z);
                        }

                        scPoint = Logic.Instance.RotateElement(scPoint, Angle2, a2p1_rot.X, a2p1_rot.Y, a2p1_rot.Z, a2p2_rot.X, a2p2_rot.Y, a2p2_rot.Z);
                    }
                    scannigPointsList.Add(scPoint);
                }
            }

            int count = ScanningPoints.Count;
            for (int i = 0; i < count; i++)
            {
                if (ScanningPoints[i].Title == lable)
                {
                    Scene.Instance.removeRenderObject(ScanningPoints[i]);
                    ScanningPoints[i] = new ScanRegionRender(Logic.Instance.RenderScale, lable, scannigPointsList, color);
                    Scene.Instance.addRenderObject(ScanningPoints[i]);
                    match = true;
                }
            }
            if (!match)
            {
                ScanRegionRender points = new ScanRegionRender(Logic.Instance.RenderScale, lable, scannigPointsList, color);
                ScanningPoints.Add(points);
                Scene.Instance.addRenderObject(points);
            }

        }
        public void removingscanRegion(string name)
        {
            for (int i = 0; i < ScanningPoints.Count; i++)
            {
                if (ScanningPoints[i].Title == name)
                {
                    Scene.Instance.removeRenderObject(ScanningPoints[i]);
                    ScanningPoints.Remove(ScanningPoints[i]);
                }
            }
        }
        #endregion

        #region FarField
        /// <summary>
        /// Отрисовка области расчёта поля в дальней зоне
        /// </summary>
        /// <param name="title"></param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="inclineAngle"></param>
        /// <param name="scantype"></param>
        public void Draw(string title, double start, double finish, double inclineAngle, double step, int scantype)
        {
            int count = FarFieldArcs.Count;
            bool match = false;
            for (int i = 0; i < count; i++)
            {
                if (FarFieldArcs[i].Title == title)
                {
                    Scene.Instance.removeRenderObject(FarFieldArcs[i]);
                    FarFieldArcs[i] = new FarFieldRender(Logic.Instance.RenderScale, title, start, finish, inclineAngle, step, scantype);
                    Scene.Instance.addRenderObject(FarFieldArcs[i]);
                    match = true;
                }
            }
            if (!match)
            {
                FarFieldRender arc = new FarFieldRender(Logic.Instance.RenderScale, title, start, finish, inclineAngle, step, scantype);
                FarFieldArcs.Add(arc);
                Scene.Instance.addRenderObject(arc);
            }
        }

        public void removingArc(string title)
        {
            for (int i = 0; i < FarFieldArcs.Count; i++)
            {
                if (FarFieldArcs[i].Title == title)
                {
                    Scene.Instance.removeRenderObject(FarFieldArcs[i]);
                    FarFieldArcs.Remove(FarFieldArcs[i]);
                }
            }
        }

        internal void removingFarField(string name)
        {
            for (int i = 0; i < FarFieldArcs.Count; i++)
            {
                if (FarFieldArcs[i].Title == name)
                {
                    Scene.Instance.removeRenderObject(FarFieldArcs[i]);
                    FarFieldArcs.Remove(FarFieldArcs[i]);
                }
            }
        }
        #endregion        

        #region Radome
        /// <summary>
        /// Отрисовка геометрии обтекателя
        /// </summary>
        /// <param name="surfaceGeometry"></param>
        public void Draw(RadomeElement obj)
        {
            string name = obj.Tag;

            List<double> vertexX = new List<double>();
            List<double> vertexY = new List<double>();
            List<double> vertexZ = new List<double>();

            for (int i = 0; i < obj.Count; i++)
            {
                vertexX.Add(obj[i].Center.X);
                vertexY.Add(obj[i].Center.Y);
                vertexZ.Add(obj[i].Center.Z);
            }




            if (RadomeLableExists(name))
            {
                RemoveRadome(obj);
            }

            Color color = obj.Color;
            RadomeMesh Radom = new RadomeMesh(name, vertexX, vertexY, vertexZ, color);//int1, int2, int3, 
            RadomMeshCollection.Add(Radom);
            Scene.Instance.addRenderObject(Radom);


        }
        private bool RadomeLableExists(string name)
        {
            bool answer = false;
            for (int i = 0; i < RadomMeshCollection.Count; i++)
            {
                if (RadomMeshCollection[i].Lable == name)
                {
                    answer = true;
                    break;
                }
            }
            return answer;
        }

        public void RemoveRadome(RadomeElement radomeEl)
        {
            string radomeName = radomeEl.Tag;
            RadomeMesh trs = RadomMeshCollection.Find(x => x.Lable == radomeName);
            RadomMeshCollection.Remove(trs);
            Scene.Instance.removeRenderObject(trs);
        }

        public void ClearRadome()
        {
            for (int i = 0; i < RadomMeshCollection.Count; i++)
            {
                Scene.Instance.removeRenderObject(RadomMeshCollection[i]);
            }
            RadomMeshCollection.Clear();
        }
        #endregion

        #region Antenna
        /// <summary>
        /// Отрисовка геометрии антенны
        /// </summary>
        /// <param name="surfaceGeometry"></param>
        public void Draw(Aperture surfaceGeometry)
        {
            List<double> vertexX = surfaceGeometry.ListX;
            List<double> vertexY = surfaceGeometry.ListY;
            List<double> vertexZ = surfaceGeometry.ListZ;



            Point3D[] points = new Point3D[surfaceGeometry.Count];

            for (int i = 0; i < surfaceGeometry.Count; i++)
            {
                points[i] = surfaceGeometry[i].Center;
            }

            //if (ScaleFactor < surfaceGeometry.DiagonalSize)
            //{
            //    ScaleFactor = (float)surfaceGeometry.DiagonalSize;
            //}

            if (Antenna != null)
            {
                removingAntenna();
            }
            Color color = new Color(50, 100, 100);
            Antenna = new AntennaMesh("", points, color);
            Scene.Instance.addRenderObject(Antenna);
        }

        public void removingAntenna()
        {
            Scene.Instance.removeRenderObject(Antenna);
            Antenna.Dispose();
            Antenna = null;
        }
        #endregion

        #region CameraPosition

        internal void InitialLook()
        {
            Vector3 eye = new Vector3(4, 2, 5);
            Vector3 target = new Vector3(0, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);
            LastLoadedLook = new Vector3[3] { eye, target, up };

            CameraManager.Instance.returnCamera(0).eye = eye;
            CameraManager.Instance.returnCamera(0).target = target;
            CameraManager.Instance.returnCamera(0).up = up;
            CameraManager.Instance.returnCamera(0).SetView(eye, target, up);
        }
        public void LookAt(Radome obj)
        {
            double r = obj.DiagonalSize * 3;
            Vector3 up = new Vector3(0, 1, 0);
            float mmfactor = 1;
            Vector3 target = new Vector3((float)obj.Center.X * mmfactor, (float)obj.Center.Z * mmfactor, (float)obj.Center.Y * mmfactor);
            float x = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180));
            float y = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180));
            float z = (float)(r * Math.Cos(theta * Math.PI / 180));

            Vector3 vect = new Vector3(x, z, y);
            Vector3 eye = vect;
            CameraManager.Instance.CameraAt(0);

            CameraManager.Instance.returnCamera(0).eye = eye;
            CameraManager.Instance.returnCamera(0).target = target;
            CameraManager.Instance.returnCamera(0).up = up;
            CameraManager.Instance.returnCamera(0).SetView(eye, target, up);

            LastLoadedLook = new Vector3[3] { eye, target, up };
            CameraManager.Instance.returnCamera(0).SizeObject = (float)obj.DiagonalSize;
        }
        public void LookAt(Aperture obj)
        {
            double r = Logic.Instance.RenderScale * 3;
            Vector3 up = new Vector3(0, 1, 0);
            float mmfactor = (float)(1.0 / Logic.Instance.RenderScale);
            Vector3 target = new Vector3((float)obj.Center.X * mmfactor, (float)obj.Center.Z * mmfactor, (float)obj.Center.Y * mmfactor);
            float x = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180));
            float y = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180));
            float z = (float)(r * Math.Cos(theta * Math.PI / 180));

            Vector3 vect = new Vector3(x, z, y);
            Vector3 eye = vect;// + target;
            CameraManager.Instance.CameraAt(0);

            CameraManager.Instance.returnCamera(0).eye = eye;
            CameraManager.Instance.returnCamera(0).target = target;
            CameraManager.Instance.returnCamera(0).up = up;
            CameraManager.Instance.returnCamera(0).SetView(eye, target, up);

            LastLoadedLook = new Vector3[3] { eye, target, up };
            //ReScaleGridAndArrows(1f / 2f * (float)obj.DiagonalSize);
            CameraManager.Instance.returnCamera(0).SizeObject = (float)obj.DiagonalSize;
        }
        public void LookAt(Aperture Aperture, Radome Radome)
        {
            double r = Radome.DiagonalSize * 3;

            Vector3 up = new Vector3(0, 1, 0);
            Vector3 target = new Vector3((float)Aperture.Center.X, (float)Aperture.Center.Z, (float)Aperture.Center.Y);

            float x = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Cos(phi * Math.PI / 180));
            float y = (float)(r * Math.Sin(theta * Math.PI / 180) * Math.Sin(phi * Math.PI / 180));
            float z = (float)(r * Math.Cos(theta * Math.PI / 180));

            Vector3 vect = new Vector3(x, y, z);
            Vector3 eye = vect + target;
            CameraManager.Instance.CameraAt(0);

            CameraManager.Instance.returnCamera(0).eye = eye;
            CameraManager.Instance.returnCamera(0).target = target;
            CameraManager.Instance.returnCamera(0).up = up;
            CameraManager.Instance.returnCamera(0).SetView(eye, target, up);

            //ReScaleGridAndArrows(1f / 2f * (float)Radome.DiagonalSize);
            LastLoadedLook = new Vector3[3] { eye, target, up };
            CameraManager.Instance.returnCamera(0).SizeObject = (float)Radome.DiagonalSize;
        }
        #endregion

        public void ClearAll()
        {
            //ClearRadome
            for (int i = 0; i < RadomMeshCollection.Count; i++)
            {
                Scene.Instance.removeRenderObject(RadomMeshCollection[i]);
            }
            RadomMeshCollection.Clear();

            //ClearAntenna
            if (Antenna != null)
            {
                removingAntenna();
            }

            //ClearBluePoints
            for (int i = 0; i < ScanningPoints.Count; i++)
            {
                Scene.Instance.removeRenderObject(ScanningPoints[i]);
            }
            ScanningPoints.Clear();

            //Clear FarField
            for (int i = 0; i < FarFieldArcs.Count; i++)
            {
                Scene.Instance.removeRenderObject(FarFieldArcs[i]);
            }
            FarFieldArcs.Clear();
        }

        //Методы камеры
        public void ChangeCamera(int c)
        {
            Vector3 eye = new Vector3();
            Vector3 target = new Vector3();
            Vector3 up = new Vector3();
            float r = 4;

            if (c == 0)
            {
                CameraManager.Instance.CameraAt(c);
            }
            else
            {
                if (c == 1)
                {
                    CameraManager.Instance.CameraAt(c);
                    eye = new Vector3(r, 0, 0);
                    target = new Vector3(0, 0, 0);
                    up = new Vector3(0, 1, 0);
                    CameraManager.Instance.returnCamera(c).SetView(eye, target, up);
                }
                else if (c == 2)
                {
                    CameraManager.Instance.CameraAt(c);
                    eye = new Vector3(0, 0, r);
                    target = new Vector3(0, 0, 0);
                    up = new Vector3(1, 0, 0);
                    CameraManager.Instance.returnCamera(c).SetView(eye, target, up);
                }
                else if (c == 3)
                {
                    CameraManager.Instance.CameraAt(c);
                    eye = new Vector3(0, r, 0);
                    target = new Vector3(0, 0, 0);
                    up = new Vector3(0, 0, 1);
                }
                CameraManager.Instance.returnCamera(c).eye = eye;
                CameraManager.Instance.returnCamera(c).target = target;
                CameraManager.Instance.returnCamera(c).up = up;
                CameraManager.Instance.returnCamera(c).SetView(eye, target, up);
            }
        }

        #region Events
        private void RenderControl_MouseUp(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseUp(sender, e);
        }
        private void RenderControl_MouseDown(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseDown(sender, e);
        }
        private void RenderControl_MouseMove(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseMove(sender, e);
        }
        void RenderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            CameraManager.Instance.currentCamera.MouseWheel(sender, e);
        }
        private void RenderControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                CameraManager.Instance.CycleCameras();
            }
            else if (e.KeyCode == Keys.F2)
            {
                RenderManager.Instance.SwitchSyncInterval();
            }
            else if (e.KeyCode == Keys.F12)
            {
                RenderManager.Instance.makeScreenshot = true;
            }

            CameraManager.Instance.currentCamera.KeyUp(sender, e);
        }
        private void RenderControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            CameraManager.Instance.currentCamera.KeyPress(sender, e);
        }
        private void RenderControl_KeyDown(object sender, KeyEventArgs e)
        {
            CameraManager.Instance.currentCamera.KeyDown(sender, e);
        }
        private void RenderControl_Resize(object sender, EventArgs e)
        {
            RenderManager.Instance.resize = true;
        }

        internal async void RunPresentation()
        {
            try
            {
                OrbitPanCamera cam = (OrbitPanCamera)CameraManager.Instance.currentCamera;                            
                
                if (cam.IsPresentationRunning)
                {
                    await Task.Run(() => cam.StopRotationY());
                }
                else
                {
                    await Task.Run(() => cam.RunRotationY());            
                }
            }
            catch (Exception)
            {   
            }
        }

        public void SetPriviousView()
        {
            CameraManager.Instance.currentCamera.SetView(LastLoadedLook[0], LastLoadedLook[1], LastLoadedLook[2]);            
        }
        #endregion










    }
}

