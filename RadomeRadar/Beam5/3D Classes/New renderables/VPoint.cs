using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;
using Apparat.ShaderManagement;
using SharpDX.Direct3D;

namespace Apparat
{
    public class VPoint
    {
        public string Title { get; set; }
        public double Theta { get; set; }        
        public double Phi { get; set; }        

        static double pi = Math.PI;

        List<Vector3> vertices;
        
        public Vector3 this[int index]
        {
            get
            {
                return vertices[index];
            }
        }

        public int Count
        {
            get 
            {
                return vertices.Count;
            }
        }
        
        public VPoint(Point3D point)
        {
            vertices = new List<Vector3>();

            int PnumVertices = 16;
            int VnumVertices = 16;

            double a_center = point.X;
            double b_center = point.Y;
            double c_center = point.Z;

            double r = 0.01;

            for (int i = 0; i < PnumVertices; i++)
            {
                double i_step = 360 / PnumVertices;
                for (int j = 0; j < VnumVertices; j++)
                {
                    double j_step = 180 / PnumVertices;

                    double a = r * Math.Sin(i * i_step * pi / 180) * Math.Cos(j * j_step * pi / 180) + a_center;
                    double b = r * Math.Sin(i * i_step * pi / 180) * Math.Sin(j * j_step * pi / 180) + b_center;
                    double c = r * Math.Cos(i * i_step * pi / 180) + c_center;
                    vertices.Add(new Vector3(Convert.ToSingle(a), Convert.ToSingle(c), Convert.ToSingle(b)));
                }
            }
        }

        internal static List<VPoint> CreateList(List<Point3D> point3DList)
        {
            List<VPoint> vPointList = new List<VPoint>();

            for (int k = 0; k < point3DList.Count; k++)
            {
                vPointList.Add(new VPoint(point3DList[k]));
            }
            return vPointList;
        }
    }
}
