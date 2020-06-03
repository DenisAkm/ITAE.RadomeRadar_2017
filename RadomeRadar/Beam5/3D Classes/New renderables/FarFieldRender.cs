using SharpDX;

namespace Apparat
{
    public class FarFieldRender: Points
    {
        public FarFieldRender(float size, string title, double start, double finish, double inclineAngle, double step, int scantype)
            : base(size, title, start, finish, inclineAngle, inclineAngle, step, 0, scantype, new Color(225, 250, 0), 1.05f)
        { 
        
        }
    }
}
