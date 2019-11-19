using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class FarFieldRender: Points
    {
        public FarFieldRender(float size, string title, double start, double finish, double inclineAngle, double step, int scantype)
            : base(size, title, start, finish, inclineAngle, inclineAngle, step, 0, scantype, Color.FromArgb(225, 250, 0).ToArgb(), 1.05f)
        { 
        
        }
    }
}
