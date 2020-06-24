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

        double pi = Math.PI;

        List<Vector3> vertices;         

        public VPoint(float size, string title, double T, double P, Color color, float disLevel)
        {
            Title = title;
            Theta = T;            
            Phi = P;                                    

            
            int PnumVertices = 16;
            int VnumVertices = 16;
            
            double Rho = disLevel * size;
            
            double a_center = Rho * Math.Sin(Theta * pi / 180) * Math.Cos(Phi * pi / 180);
            double b_center = Rho * Math.Sin(Theta * pi / 180) * Math.Sin(Phi * pi / 180);
            double c_center = Rho * Math.Cos(Theta * pi / 180);

            double r = 20;

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
    }
}
