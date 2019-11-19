using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class SurfaceCurrent
    {
        public Current I;
        public Current M;


        public SurfaceCurrent(Current i, Current m)
        {
            I = i;
            M = m;
        }

        
        //public static SurfaceCurrent Calculation(Aperture app, Radome finalGeom, double freq, float dim, int proc)
        //{
        //    NearFieldC field = NearFieldC.SurfaceCurToGeometryC(app, finalGeom, freq, dim, proc);
        //    return SurfaceCurrent.CurrentsExitetion(field, finalGeom, Direction.Outside);
        //}
        

        public int Count
        {
            get
            {
                return I.Count;
            }
        }
    }
}

