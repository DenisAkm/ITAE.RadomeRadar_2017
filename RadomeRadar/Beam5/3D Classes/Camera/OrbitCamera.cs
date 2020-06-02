using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Apparat
{
    public class OrbitCamera : Camera
    {
        #region Constructor
        public OrbitCamera()
        {
            eye = new Vector3(4, 2, 0);
            target = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);

            view = Matrix.LookAtLH(eye, target, up);
            perspective = Matrix.PerspectiveFovLH((float)Math.PI / 4, CameraManager.aspectRatio, CameraManager.znearPlane, CameraManager.zfarPlane);
        }
        #endregion

        float rotY = 0;

        public void rotateY(int value)
        {
            rotY = (value / 100.0f);
            Matrix rotMat = Matrix.RotationY(rotY);
            eye = Vector3.TransformCoordinate(eye, rotMat);
            SetView(eye, target, up);
        }
        float rotOrtho = 0;

        public void rotateOrtho(int value)
        {
            Vector3 viewDir = target - eye;
            Vector3 orhto = Vector3.Cross(viewDir, up);

            rotOrtho = (value / 100.0f);
            Matrix rotOrthoMat = Matrix.RotationAxis(orhto, rotOrtho);

            Vector3 eyeLocal = eye - target;
            eyeLocal = Vector3.TransformCoordinate(eyeLocal, rotOrthoMat);
            Vector3 newEye = eyeLocal + target;
            Vector3 newViewDir = target - newEye;
            float cosAngle = Vector3.Dot(newViewDir, up) / (newViewDir.Length() * up.Length());
            if (cosAngle < 0.9f && cosAngle > -0.9f)
            {
                eye = eyeLocal + target;
                SetView(eye, target, up);
            }
        }


        float maxZoom = 3.0f;
        public void zoom(int value)
        {
            float scaleFactor = 1.0f;
            if (value > 0)
            {
                scaleFactor = 1.1f;
            }
            else
            {
                if ((eye - target).Length() > maxZoom)
                    scaleFactor = 0.9f;
            }

            Matrix scale = Matrix.Scaling(scaleFactor, scaleFactor, scaleFactor);
            eye = Vector3.TransformCoordinate(eye, scale);
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
            }
        }

        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int delta = e.Delta;
            this.zoom(delta);
        }

        public override void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

        public override void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
    }
}
