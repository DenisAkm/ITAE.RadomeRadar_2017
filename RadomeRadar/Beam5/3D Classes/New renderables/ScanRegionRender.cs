using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class ScanRegionRender: Points
    {
        public ScanRegionRender(float size, string title, double startT, double finishT, double startP, double finishP, double stepT, double stepP, int scantype, int color)
            : base(size, title, startT, finishT, startP, finishP, stepT, stepP, scantype, color, 1.2f)
        { 
        
        }
        public ScanRegionRender(float size, string title, List<Point3D> points, int color)
            : base(size, title, points, color, 1.2f)
        {

        }
    }
}
