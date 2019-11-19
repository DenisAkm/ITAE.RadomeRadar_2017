using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class Radome: List<RadomeElement>
    {
        public static string DefaultLable = "Часть ";
        public void Exclude(string removeName)
        {
            this.Remove(this.Find(x => x.Lable == removeName));            
        }
        
        
        public double DiagonalSize
        {
            get                
            {
                //double XMax = 0;
                //double XMin = 0;
                //double YMax = 0;
                //double YMin = 0;
                //double ZMax = 0;
                //double ZMin = 0;
                //if (Count != 0)
                //{
                //    double[] param = this[0].DiagolalParameters;
                //    XMax = param[0];
                //    XMin = param[1];
                //    YMax = param[2];
                //    YMin = param[3];
                //    ZMax = param[4];
                //    ZMin = param[5];

                //    for (int i = 1; i < Count; i++)
                //    {
                //        param = this[i].DiagolalParameters;

                //        if (param[0] > XMax)
                //        {
                //            XMax = param[0];
                //        }
                //        if (param[1] < XMin)
                //        {
                //            XMin = param[1];
                //        }
                //        if (param[2] > YMax)
                //        {
                //            YMax = param[2];
                //        }
                //        if (param[3] < YMin)
                //        {
                //            YMin = param[3];
                //        }
                //        if (param[4] > ZMax)
                //        {
                //            ZMax = param[4];
                //        }
                //        if (param[5] < ZMin)
                //        {
                //            ZMin = param[5];
                //        }
                //    }
                //}
                return Math.Sqrt((XMax - XMin) * (XMax - XMin) + (YMax - YMin) * (YMax - YMin) + (ZMax - ZMin) * (ZMax - ZMin));
            }
        }

        public double XMax
        {
            get 
            {
                double xMax = 0;
                
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;
                    xMax = param[0];                    
                    
                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;

                        if (param[0] > xMax)
                        {
                            xMax = param[0];
                        }                        
                    }
                }
                return xMax;
            }
        }
        public double YMax
        {
            get
            {                
                double yMax = 0;
             
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;                    
                    yMax = param[2];
                    

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;
                        
                        if (param[2] > yMax)
                        {
                            yMax = param[2];
                        }                        
                    }
                }
                return yMax;
            }
        }
        public double ZMax
        {
            get
            {                
                double zMax = 0;
                
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;                    
                    zMax = param[4];
                    

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;

                        
                        if (param[4] > zMax)
                        {
                            zMax = param[4];
                        }                        
                    }
                }
                return zMax;
            }
        }
        public double XMin
        {
            get
            {                
                double xMin = 0;
                
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;
                    
                    xMin = param[1];                    

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;
                    
                        if (param[1] < xMin)
                        {
                            xMin = param[1];
                        }                    
                    }
                }
                return xMin;
            }
        }
        public double YMin
        {
            get
            {                
                double yMin = 0;
                
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;
                    
                    yMin = param[3];                    

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;
                        
                        if (param[3] < yMin)
                        {
                            yMin = param[3];
                        }                        
                    }
                }
                return yMin;
            }
        }
        public double ZMin
        {
            get
            {                
                double zMin = 0;
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;                    
                    zMin = param[5];

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;
                        
                        if (param[5] < zMin)
                        {
                            zMin = param[5];
                        }
                    }
                }
                return zMin;
            }
        }
        

        /// <summary>
        /// Длина большей стороны
        /// </summary>
        public float DimentionSize
        {
            get
            {
                float xSize = (float)(XMax - XMin);
                float ySize = (float)(YMax - YMin);
                float zSize = (float)(ZMax - ZMin);

                float factor = 1;
                if (xSize > ySize)
                {
                    if (xSize > zSize)
                    {
                        factor = xSize;
                    }
                    else
                    {
                        factor = zSize;
                    }
                }
                else
                {
                    if (ySize > zSize)
                    {
                        factor = ySize;
                    }
                    else
                    {
                        factor = zSize;
                    }
                }                
                return factor;
            }
        }
        public Point3D Center
        {
            get 
            {
                double XMax = 0;
                double XMin = 0;
                double YMax = 0;
                double YMin = 0;
                double ZMax = 0;
                double ZMin = 0;
                if (Count != 0)
                {
                    double[] param = this[0].DiagolalParameters;
                    XMax = param[0];
                    XMin = param[1];
                    YMax = param[2];
                    YMin = param[3];
                    ZMax = param[4];
                    ZMin = param[5];

                    for (int i = 1; i < Count; i++)
                    {
                        param = this[i].DiagolalParameters;

                        if (param[0] > XMax)
                        {
                            XMax = param[0];
                        }
                        if (param[1] < XMin)
                        {
                            XMin = param[1];
                        }
                        if (param[2] > YMax)
                        {
                            YMax = param[2];
                        }
                        if (param[3] < YMin)
                        {
                            YMin = param[3];
                        }
                        if (param[4] > ZMax)
                        {
                            ZMax = param[4];
                        }
                        if (param[5] < ZMin)
                        {
                            ZMin = param[5];
                        }
                    }
                }
                return new Point3D((XMax + XMin) / 2, (YMax + YMin) / 2, (ZMax + ZMin) / 2);
            }
        }

        /// <summary>
        /// Суммарное количество элементов обтекателя
        /// </summary>
        public int CountElements
        {
            get 
            {
                int count = 0;
                for (int i = 0; i < this.Count; i++)
                {
                    count += this[i].Count;
                }
                return count;
            }
        }

        public List<string> lablesCollection
        {
            get 
            {
                List<string> collection = new List<string>();

                for (int i = 0; i < this.Count; i++)
                {
                    collection.Add(this[i].Lable);
                }
                return collection;
            }
        }
        public string GetUniqueLable(int count)
        {

            string uniqueLable;
            bool match = false;
            int k = lablesCollection.Count;

            do
            {
                match = false;
                k++;
                uniqueLable = String.Concat(DefaultLable, k, " [", count, "]");


                for (int i = 0; i < lablesCollection.Count; i++)
                {
                    if (uniqueLable == lablesCollection[i])
                    {
                        match = true;
                        break;
                    }
                }
            }
            while (match);
            return uniqueLable;
        }
    }
}
