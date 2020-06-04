using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Apparat
{
    public class OrbitPanCamera : Camera
    {
        #region Constructor
        public OrbitPanCamera()
        {
            eye = new Vector3(4, 2, 5);
            target = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);

            view = Matrix.LookAtLH(eye, target, up);
            perspective = Matrix.PerspectiveFovLH((float)Math.PI / 4, CameraManager.aspectRatio, CameraManager.znearPlane, CameraManager.zfarPlane);

            sizeObject = 1;
        }


        #endregion

        float rotY = 0;
        public bool IsPresentationRunning { get; set; } = false;

        public void rotateY(float value)
        {
            rotY = (value / 100.0f);
            Vector3 eyeLocal = eye - target;

            Matrix rotMat = Matrix.RotationY(rotY);
            eyeLocal = Vector3.TransformCoordinate(eyeLocal, rotMat);
            eye = eyeLocal + target;

            SetView(eye, target, up);
        }
        float rotOrtho = 0;

        public void rotateOrtho(int value)
        {            
            Vector3 viewDir = target - eye;
            Vector3 orhto = Vector3.Cross(viewDir, up);
            orhto.Normalize();

            rotOrtho = (value / 100.0f);
            Matrix rotOrthoMat = Matrix.RotationAxis(orhto, rotOrtho);

            Vector3 eyeLocal = eye - target;
            eyeLocal = Vector3.TransformCoordinate(eyeLocal, rotOrthoMat);
            Vector3 newEye = eyeLocal + target;
            Vector3 newViewDir = target - newEye;
            float cosAngle = Vector3.Dot(newViewDir, up) / (newViewDir.Length() * up.Length());
            if (cosAngle < 0.999f && cosAngle > -0.999f)
            {
                eye = eyeLocal + target;
                SetView(eye, target, up);
            }
        }

        public void panX(int value)
        {
            float scaleFactor = 0.0f;
            if (value > 1)
            {
                scaleFactor = -0.05f;
            }
            else if (value < -1)
            {
                scaleFactor = 0.05f;
            }
            Vector3 viewDir = target - eye;
            Vector3 orhto = Vector3.Cross(viewDir, up);
            orhto.Normalize();
            scaleFactor = scaleFactor * (float)Math.Sqrt(viewDir.Length()) * 0.5f;
            Matrix scaling = Matrix.Scaling(scaleFactor, scaleFactor, scaleFactor);
            orhto = Vector3.TransformCoordinate(orhto, scaling);

            target = target + orhto;
            eye = eye + orhto;
            SetView(eye, target, up);
        }

        public void panY(int value)
        {
            float scaleFactor = 0.00f;
            if (value > 1)
            {
                scaleFactor = -0.05f;
            }
            else if (value < -1)
            {
                scaleFactor = 0.05f;
            }
            Vector3 viewDir = target - eye;
            scaleFactor = scaleFactor * (float)Math.Sqrt(viewDir.Length()) * 0.5f;
            viewDir.Y = 0.0f;
            viewDir.Normalize();
            Matrix scaling = Matrix.Scaling(scaleFactor, scaleFactor, scaleFactor);
            viewDir = Vector3.TransformCoordinate(viewDir, scaling);

            target = target + viewDir;
            eye = eye + viewDir;
            SetView(eye, target, up);
        }


        float maxZoom = 0.05f;
        public void zoom(int value)
        {
            Vector3 viewDir = eye - target;

            float scaleFactor = sizeObject;
            if (value > 0)
            {
                scaleFactor *= 0.4f;
            }
            else
            {
                if (viewDir.Length() > maxZoom)
                    scaleFactor *= 0.3f;
            }

            Matrix scale = Matrix.Scaling(scaleFactor, scaleFactor, scaleFactor);
            viewDir.Normalize();
            viewDir = Vector3.TransformCoordinate(viewDir, scale);
            if (value > 0)
            {
                eye = eye + viewDir;
            }
            else
            {
                eye = eye - viewDir;
            }

            SetView(eye, target, up);
        }

        public override void MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dragging = false;
        }

        public override void MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            dragging = true;
            startX = e.X;
            startY = e.Y;
        }

        public override void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (dragging)
            {
                int currentX = e.X;
                deltaX = startX - currentX;
                startX = currentX;

                int currentY = e.Y;
                deltaY = startY - currentY;
                startY = currentY;

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.rotateY(-deltaX);
                    this.rotateOrtho(deltaY);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    this.zoom(deltaY);
                    //this.panX(deltaX);
                    //this.panY(deltaY);
                }
            }
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int delta = e.Delta;
            this.zoom(delta);
        }

        public override void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string input = e.KeyChar.ToString();
            if (input == "a" || input == "ф")
            {
                rotateY(-2);
            }
            if (input == "d" || input == "в")
            {
                rotateY(2);
            }
            if (input == "w" || input == "ц")
            {
                rotateOrtho(1);
            }
            if (input == "s" || input == "ы")
            {
                rotateOrtho(-1);
            }            
        }

        public override void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {   
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {            
        }

        public void RunRotationY()
        {
            IsPresentationRunning = true;
            while (IsPresentationRunning)
            {                
                rotateY(0.1f);
                Thread.Sleep(10);
            }
        }

        internal void StopRotationY()
        {
            IsPresentationRunning = false;
        }
    }
}
