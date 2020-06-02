using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Apparat
{
    public class EgoCamera : Camera
    {
        Vector3 look;

        public EgoCamera()
        {
            look = new Vector3(1, 0, 0);
            up = new Vector3(0, 1, 0);
            eye = new Vector3(0, 1, 0);
            target = eye + look;

            view = Matrix.LookAtLH(eye, target, up);
            perspective = Matrix.PerspectiveFovLH((float)Math.PI / 4, CameraManager.aspectRatio, CameraManager.znearPlane, CameraManager.zfarPlane);
        }

        new public Matrix  ViewPerspective
        {
            get
            {
                if (strafingLeft)
                    strafe(1);

                if (strafingRight)
                    strafe(-1);

                if (movingForward)
                    move(1);

                if (movingBack)
                    move(-1);
                
                return view * perspective;
            }
        }

        public void yaw(int x)
        {
            Matrix rot = Matrix.RotationY(x / 100.0f);
            look = Vector3.TransformCoordinate(look, rot);

            target = eye + look;
            view = Matrix.LookAtLH(eye, target, up);
        }


        float pitchVal = 0.0f;
        public void pitch(int y)
        {
            Vector3 axis = Vector3.Cross(up, look);
            float rotation = y / 100.0f;
            pitchVal = pitchVal + rotation;

            float halfPi = (float)Math.PI / 2.0f;

            if (pitchVal < -halfPi)
            {
                pitchVal = -halfPi;
                rotation = 0;
            }
            if (pitchVal > halfPi)
            {
                pitchVal = halfPi;
                rotation = 0;
            }

            Matrix rot = Matrix.RotationAxis(axis, rotation);

            look = Vector3.TransformCoordinate(look, rot);
            
            look.Normalize();
            
            target = eye + look;
            view = Matrix.LookAtLH(eye, target, up);
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
                    pitch(deltaY);
                    yaw(-deltaX);
                }
            }
        }

        public void strafe(int val)
        {
            Vector3 axis = Vector3.Cross(look, up);
            Matrix scale = Matrix.Scaling(0.1f, 0.1f, 0.1f);
            axis = Vector3.TransformCoordinate(axis, scale);

            if (val > 0)
            {
                eye = eye + axis;
            }
            else
            {
                eye = eye - axis;
            }
            
            target = eye + look;
            view = Matrix.LookAtLH(eye, target, up);
        }

        public void move(int val)
        {
            Vector3 tempLook = look;
            Matrix scale = Matrix.Scaling(0.1f, 0.1f, 0.1f);
            tempLook = Vector3.TransformCoordinate(tempLook, scale);


            if (val > 0)
            {
                eye = eye + tempLook;
            }
            else
            {
                eye = eye - tempLook;
            }
            
            target = eye + look;
            view = Matrix.LookAtLH(eye, target, up);
        }

        // Nothing to do here
        public override void MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int delta = e.Delta;
            this.zoom(delta);        
        }
        float maxZoom = 3.0f;
        public void zoom(int value)
        {
            Vector3 viewDir = eye - target;

            float scaleFactor = 100.0f;
            if (value > 0)
            {
                scaleFactor = 110.0f;
            }
            else
            {
                if (viewDir.Length() > maxZoom)
                    scaleFactor = 90.0f;
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
        public override void KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

        bool strafingLeft = false;
        bool strafingRight = false;
        bool movingForward = false;
        bool movingBack = false;

        public override void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.W)
            {
                movingForward = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.S)
            {
                movingBack = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.A)
            {
                strafingLeft = true;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.D)
            {
                strafingRight = true;
            }
        }

        public override void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.W)
            {
                movingForward = false;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.S)
            {
                movingBack = false;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.A)
            {
                strafingLeft = false;
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.D)
            {
                strafingRight = false;
            }
        }
    }
}
