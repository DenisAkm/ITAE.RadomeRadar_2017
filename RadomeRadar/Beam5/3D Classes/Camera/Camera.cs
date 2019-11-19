using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;

namespace Apparat
{
    public abstract class Camera
    {
        public Vector3 eye;
        public Vector3 target;
        public Vector3 up;

        public Matrix view = Matrix.Identity;
        public Matrix perspective = Matrix.Identity;
        public Matrix viewPerspective = Matrix.Identity;
        public float sizeObject;
        public Matrix View
        {
            get { return view; }
        }

        public float SizeObject
        {
            set
            {
                sizeObject = value;
            }
        }

        public void SetPerspective(float fov, float aspect, float znear, float zfar)
        {
            perspective = Matrix.PerspectiveFovLH(fov, aspect, znear, zfar);
        }

        public void SetView(Vector3 eye, Vector3 target, Vector3 up)
        {
            view = Matrix.LookAtLH(eye, target, up);
        }

        public Matrix Perspective
        {
            get { return perspective; }
        }

        public Matrix ViewPerspective
        {
            get { return view * perspective; }
        }

        public bool dragging = false;
        public int startX = 0;
        public int deltaX = 0;

        public int startY = 0;
        public int deltaY = 0;

        public abstract void MouseUp(object sender, MouseEventArgs e);
        public abstract void MouseDown(object sender, MouseEventArgs e);
        public abstract void MouseMove(object sender, MouseEventArgs e);
        public abstract void MouseWheel(object sender, MouseEventArgs e);

        public abstract void KeyPress(object sender, KeyPressEventArgs e);
        public abstract void KeyDown(object sender, KeyEventArgs e);
        public abstract void KeyUp(object sender, KeyEventArgs e);
    }
}
