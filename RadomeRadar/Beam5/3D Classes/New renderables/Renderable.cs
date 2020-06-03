using SharpDX;

namespace Apparat
{
    public abstract class Renderable
    {
        public Matrix transform = Matrix.Identity;
        public abstract void Render();
        public abstract void Dispose();

        public abstract Matrix Transform
        {
            get;
            set;
        }

        public int ToArbg(Color color)
        {
            return new Color(color.A, color.R, color.G, color.B).ToAbgr();
        }
    }
}
