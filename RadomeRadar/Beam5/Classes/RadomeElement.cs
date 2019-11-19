using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class RadomeElement : Mesh
    {

        public string Lable { get; set; }
        public Color Color { get; set; }
        Stenka structure = new Stenka("[Пусто]");
        public string Tag { get; set; }
        public bool Include { get; set; }

        public RadomeElement(string _lable, Color _color, List<double> _x, List<double> _y, List<double> _z, List<Int32> _ind1, List<Int32> _ind2, List<Int32> _ind3, bool includeParam = true)
            : base(ref _x, ref _y, ref _z, ref _ind1, ref _ind2, ref _ind3)
        {
            Color = _color;
            Lable = _lable;
            Include = includeParam;
            Tag = Randomizer.RandomString(32);
        }

        public RadomeElement(string _lable, Color _color, Point3D[] points, double[] area, DVector[] norma, bool includeParam = true)
            : base(points, area, norma)
        {
            Color = _color;
            Lable = _lable;
            Include = includeParam;
            Tag = Randomizer.RandomString(32);
        }

        public RadomeElement(Mesh radome)
        {
            // TODO: Complete member initialization
            elements = radome.elements;
            ListX = radome.ListX;
            ListY = radome.ListY;
            ListZ = radome.ListZ;

            ListI1 = radome.ListI1;
            ListI2 = radome.ListI2;
            ListI3 = radome.ListI3;
        }


        public double[] DiagolalParameters
        {
            get
            {
                return new double[] { XMax, XMin, YMax, YMin, ZMax, ZMin };
            }
        }



        public Stenka Structure
        {
            get
            {
                return structure;
            }
            set
            {
                structure = value;
            }
        }



        public new static RadomeElement Copy(RadomeElement rel)
        {
            Mesh mesh = Mesh.Copy(rel);
            RadomeElement el = new RadomeElement(mesh);
            el.Color = rel.Color;
            el.Lable = rel.Lable;
            el.Include = rel.Include;
            el.Tag = rel.Tag;
            el.Structure = rel.Structure;
            el.ListX = rel.ListX;
            el.ListY = rel.ListY;
            el.ListZ = rel.ListZ;
            el.ListI1 = rel.ListI1;
            el.ListI2 = rel.ListI2;
            el.ListI3 = rel.ListI3;

            return el;
        }
    }
}
