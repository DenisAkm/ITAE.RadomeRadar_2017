using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//
//Класс описывающий структуру стенки обтекателя
//

//Size(i) - толщина i-го слоя
//Thickness - толщина стенки из i слоёв

namespace Apparat
{
    public class Stenka
    {

        public List<Layer> Layers = new List<Layer>();            // последовательность слоёв начиная от антенны        
        public string Lable { get; set; }

        public Stenka(string name)
        {
            Lable = name;
            Layers.Add(new Layer());
        }
        public Stenka(string name, List<Complex> epsArr, List<Complex> muArr, List<Single> tArr)
        {
            Lable = name;
            for (int i = 0; i < tArr.Count; i++)
            {
                Layers.Add(new Layer(epsArr[i], muArr[i], tArr[i]));
            }
        }
        public Layer this[int index]
        {
            get
            {
                return Layers[index];
            }
        }
        public float GeneralThickness
        {
            get
            {
                Single tickness = 0;
                for (int i = 0; i < Layers.Count; i++)
                {
                    tickness += Layers[i].Tickness;
                }
                return tickness;
            }
        }
        public void Remove(int i)
        {
            Layers.RemoveAt(i);
        }
        public void Add(Complex Eps, Complex Mu, Single t)
        {
            Layers.Add(new Layer(Eps, Mu, t));
        }

        public Complex Eps(int i)
        {
            return Layers[i].Permittivity;
        }
        public Complex Mu(int i)
        {
            return Layers[i].Permeability;
        }
        public float Size(int i)
        {
            return Layers[i].Tickness;
        }
        public void Add(Layer layer)
        {
            Layers.Add(layer);
        }
        public void Rewrite(int n, Complex Eps, Complex Mu, Single t)
        {
            Layers[n].Permittivity = Eps;
            Layers[n].Permeability = Mu;
            Layers[n].Tickness = t;
        }
        public void Clear()
        {
            Layers.Clear();
        }
        /// <summary>
        /// Количество слоёв
        /// </summary>
        public int Count
        {
            get { return Layers.Count; }
        }

        public Stenka Copy()
        {
            string name = Lable + "_копия";
            List<Complex> eps = new List<Complex>();
            List<Complex> mu = new List<Complex>();
            List<Single> t = new List<float>();

            for (int i = 0; i < Count; i++)
            {
                eps.Add(this[i].Permittivity);
                mu.Add(this[i].Permeability);
                t.Add(this[i].Tickness);
            }
            return new Stenka(name, eps, mu, t);
        }
    }
    public class Layer
    {
        const double pi = Math.PI;
        //const double Mu_0 = 4.0f * pi * 1.0e-7f;                                    // 1.0 / (c * c * E_0) Гн/м     magnetic constant    магнитная постоянная
        //const double E_0 = 8.85418781761e-12f;                                      // 8.85e-12 Ф/м        electric constant     электрическая постоянная

        /// <summary>
        /// Диэлектрическая проницаемость
        /// </summary>
        public Complex Permittivity;
        /// <summary>
        /// Магнитная проницаемость
        /// </summary>
        public Complex Permeability;
        /// <summary>
        /// Толщина слоя
        /// </summary>
        public float Tickness;         //толищна
        public Complex Ea;

        public Complex Mua;
        public Layer()
        {
            Permittivity = new Complex(1, 0);   //значение диэлектрической проницаемости слоя по умолчанию
            Permeability = new Complex(1, 0);   //значение магнитной проницаемости слоя по умолчанию
            Ea = Permittivity * CV.E_0;
            Mua = Permeability * CV.Mu_0;
            Tickness = 0.001f;                    //значение толщины слоя по умолчанию
        }
        public Layer(Complex Eps, Complex Mu, Single t)
        {
            Permittivity = Eps;   //значение диэлектрической проницаемости слоя
            Permeability = Mu;   //значение магнитной проницаемости слоя

            Ea = Eps * CV.E_0;
            Mua = Mu * CV.Mu_0;
            Tickness = t;                    //значение толщины слоя
        }
    }
}
