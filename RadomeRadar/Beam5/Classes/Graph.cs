using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Apparat
{
    public partial class Graph : ZedGraphControl
    {
        private System.ComponentModel.IContainer components = null;

        public List<GraphData> Curves = new List<GraphData>();
        Color[] uniqueColors = new Color[100];
        public GraphPane myPane = new GraphPane();        

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        public Graph()
        {
            InitializeComponent();

            // !!! Подпишемся на событие, которое будет возникать перед тем, 
            // как будет показано контекстное меню.
            this.ContextMenuBuilder +=
                new ZedGraphControl.ContextMenuBuilderEventHandler(zedGraph_ContextMenuBuilder);

        }

        private void zedGraph_ContextMenuBuilder(ZedGraphControl sender, System.Windows.Forms.ContextMenuStrip menuStrip, Point mousePt, ContextMenuObjectState objState)
        {
            // !!! 
            // Переименуем (переведем на русский язык) некоторые пункты контекстного меню
            menuStrip.Items[0].Text = "Копировать";
            menuStrip.Items[1].Text = "Сохранить как картинку…";
            menuStrip.Items[2].Text = "Параметры страницы…";
            menuStrip.Items[3].Text = "Печать…";
            menuStrip.Items[4].Text = "Показывать значения в точках…";
            menuStrip.Items[5].Text = "Вернуть";
            menuStrip.Items[6].Text = "Вернуть начальное масштабирование";
            menuStrip.Items[7].Text = "Установить масштаб по умолчанию…";
        }
        private void InitializeComponent()
        {
            
        }

        private void CustomizeGraph()
        {
            string[] setOfcolors = new string[]{"#e5194a","#27509e","#20252a","#fbbe18","#29b297","#26295a","#4ebbbc","#ec6593","#a79258",
                "#ef7c52","#e4524f","#32bce9","#3b3b3b","#f3e737","#e83b6e","#3baa36","#e6322a","#19365f","#fadd13","#202230","#eb6626",
                "#e41d32","#1ba1a5","#1a3c55","#040506","#6bba7c","#d72125","#c6ccd2","#f8af42","#715291","#e95254","#1a65ae","#b8ce36",
                "#bc7b37","#fdd816","#784f1e","#2c73b8","#e8534f","#daa43f","#d4cc3c","#f3cc5a","#9059a0","#11a9dd","#e41c2a","#fade1f",
                "#238b94","#39bcdd","#f4cf39","#109338","#e67a5c","#fbf0a3","#22b6ea","#82bb26","#212a31","#8ac4cd","#ede439","#e94a54","#1c9675"};

            for (int i = 0; i < setOfcolors.Length; i++)
            {
                uniqueColors[i] = ColorTranslator.FromHtml(setOfcolors[i]);//
            }



            //ColorList = new List<Color>(){ColorTotalA,ColorTotalRadome,ColorThetaRadome,ColorPhiRadome,ColorTotalRef,ColorThetaRef,ColorPhiRef,ColorTotalRefInt,
            //    ColorThetaRefInt,ColorPhiRefInt,ColorTotalRadomeRef,ColorThetaRadomeRef,ColorPhiRadomeRef,ColorTotalRadomeRefInt,ColorThetaRadomeRefInt,ColorPhiRadomeRefInt};            

            myPane = this.GraphPane;
            // Set the titles and axis labels
            //myPane.Chart.Border.Color = System.Drawing.SystemColors.Control;
            //myPane.Fill.Type = FillType.Solid;
            myPane.Fill.Color = System.Drawing.SystemColors.Control;
            myPane.Chart.Fill.Color = System.Drawing.SystemColors.Control;

            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "";
            myPane.Legend.IsVisible = false;

            //myPane.YAxis.Cross = 0.0;
            // Turn off the axis frame and all the opposite side tics
            myPane.Chart.Border.IsVisible = false;
            myPane.XAxis.MajorTic.IsOpposite = false;
            myPane.XAxis.MinorTic.IsOpposite = false;
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;



            // Включаем отображение сетки напротив крупных рисок по оси X
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Задаем вид пунктирной линии для крупных рисок по оси X:
            // Длина штрихов равна 10 пикселям, ... 
            myPane.XAxis.MajorGrid.DashOn = 10;

            // затем 5 пикселей - пропуск
            myPane.XAxis.MajorGrid.DashOff = 5;


            // Включаем отображение сетки напротив крупных рисок по оси Y
            myPane.YAxis.MajorGrid.IsVisible = true;

            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            myPane.YAxis.MajorGrid.DashOn = 10;
            myPane.YAxis.MajorGrid.DashOff = 5;


            // Включаем отображение сетки напротив мелких рисок по оси X
            myPane.YAxis.MinorGrid.IsVisible = true;

            // Задаем вид пунктирной линии для крупных рисок по оси Y: 
            // Длина штрихов равна одному пикселю, ... 
            myPane.YAxis.MinorGrid.DashOn = 1;

            // затем 2 пикселя - пропуск
            myPane.YAxis.MinorGrid.DashOff = 2;

            // Включаем отображение сетки напротив мелких рисок по оси Y
            myPane.XAxis.MinorGrid.IsVisible = true;

            // Аналогично задаем вид пунктирной линии для крупных рисок по оси Y
            myPane.XAxis.MinorGrid.DashOn = 1;
            myPane.XAxis.MinorGrid.DashOff = 2;

            BorderStyle = System.Windows.Forms.BorderStyle.None;
            myPane.Border.IsVisible = false;

            // Зделаем чуть помальче шрифт, чтобы уместилось побольше меток
            myPane.XAxis.Scale.FontSpec.Size = 10;
            myPane.YAxis.Scale.FontSpec.Size = 10;

            // Разрешим выбор кривых
            IsEnableSelection = true;

            // Выбирать кривые будем с помощью левой кнопки мыши
            SelectButtons = MouseButtons.Left;
            // При этом клавиши нажимать никакие не надо
            SelectModifierKeys = Keys.Shift;

            // Отключим масштабирование, так как по умолчанию 
            // оно тоже использует левую кнопку мыши без дополнительных клавиш
            //zedGraphControl1.IsEnableZoom = false;

            //цвет полотна графика
            //zedGraphControl1.GraphPane.Fill.Color = Color.White;
            //zedGraphControl1.GraphPane.Chart.Fill.Color = Color.White;

            //myPane.CurveList.Clear();


            //for (int i = 0; i < CurveNames.Count; i++)
            //{
            //    ListPointPairs.Add(new PointPairList());
            //}
            //for (int i = 0; i < CurveNames.Count; i++)
            //{
            //    LineItem curve = myPane.AddCurve(CurveNames[i], ListPointPairs[i], ColorList[i], SymbolType.Circle);
            //    curve.Line.Width = 2F;
            //    curve.Symbol.Size = 2F;
            //}

            //for (int i = 0; i < CurveNames.Count; i++)
            //{
            //    myPane.CurveList[i].IsVisible = false;
            //}
            //myPane.CurveList[0].IsVisible = true;
            //myPane.CurveList[1].IsVisible = true;
            //myPane.CurveList[4].IsVisible = true;
            //myPane.CurveList[7].IsVisible = true;
            //myPane.CurveList[10].IsVisible = true;
            //myPane.CurveList[13].IsVisible = true;
            Invalidate();
        }
        public void Clear()
        {
            Curves.Clear();
            this.GraphPane.CurveList.Clear();
            InitializeComponent();            
        }


        public void Add(string title, FarFieldC farField, Aperture ap, string Component, string Type)
        {   
            int count = farField.Count;
            double[] x = new double[count];
            double[] y = new double[count];
            double val = 0;
            for (int j = 0; j < count; j++)
            {
                if (Form1.Instance.radioButtonX1.Checked)
                {
                    x[j] = farField[j].LocalTheta; 
                }
                else
                {
                    x[j] = farField[j].LocalPhi;                        
                }
                
                if (Type == "Модуль")
                {
                    switch (Component)
                    {
                        case "Total":
                            val = farField[j].Etotal;
                            break;
                        case "Theta":
                            val = farField[j].EdirectMagnitude(ap);
                            break;
                        case "Phi":
                            val = farField[j].EcrossMagnitude(ap);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (Component)
                    {                        
                        case "Theta":
                            val = farField[j].EdirectPhase(ap);
                            break;
                        case "Phi":
                            val = farField[j].EcrossPhase(ap);
                            break;
                        default:
                            break;
                    }
                }
                
                y[j] = val;              
            }
            //ListPointPairs.Add(new PointPairList(x, y));
            PointPairList ppl = new PointPairList(x, y);
            //Random rnd = new Random();
            Color color = FindUniqueColor();
            Curves.Add(new GraphData(title, x, y, Component, Type, color));
            //if (key != 0)
            //{
            //    int stop = 0;
            //    CurveItem ci = myPane.CurveList.Find(v => v.Label.Text == CurveNames[key-1]);
            //    Color b = ci.Color;
                
            //    while ((a.R - b.R) * (a.R - b.R) + (a.G - b.G) * (a.G - b.G) + (a.B - b.B) * (a.B - b.B) < 0.25 * 3 * 255 * 255)
            //    {
            //        a = uniqueColors[rnd.Next(0, 57)];
            //        stop++;
            //        if (stop > 100)
            //        {
            //            break;
            //        }
            //    }
            //}

            LineItem curve = myPane.AddCurve(title, ppl, color, SymbolType.None);
            curve.Line.Width = 2F;
            curve.Symbol.Size = 2F;

            AxisChange();
            Refresh();
        }

        private Color FindUniqueColor()
        {
            Color ans = uniqueColors[0];
            int k = 1;
            for (int i = 0; i < Curves.Count; i++)
            {
                while (ans.ToArgb() == Curves[i].Color.ToArgb())
                {
                    ans = uniqueColors[k];
                    k++;
                }
            }
            return ans;
        }
        public void Remove(string title)
        {
            int removeKey = Curves.FindIndex(x => x.Title == title);
            Curves.RemoveAt(removeKey);
            //ListPointPairs.RemoveAt(removeKey);            
            myPane.CurveList.RemoveAt(removeKey);
            Refresh();
        }        
        internal void Draw(FarFieldC farField, string c1)
        {
            //int k1 = CurveNames.FirstOrDefault(x => x.Value == c1).Key;
            //int count = farField.Count;
            //for (int j = 0; j < count; j++)
            //{
            //    FarFieldElementC ffe = farField[j];
            //    //ListPointPairs[k1].Add(ffe.LocalTheta, ffe.Etotal);
            //}
            //AxisChange();
            //Refresh();
        }
        internal void Draw(FarFieldC farField, string c1, string c2, string c3)
        {
            //int k1 = CurveNames.FirstOrDefault(x => x.Value == c1).Key;
            //int k2 = CurveNames.FirstOrDefault(x => x.Value == c2).Key;
            //int k3 = CurveNames.FirstOrDefault(x => x.Value == c3).Key;

            //int count = farField.Count;
            //for (int j = 0; j < count; j++)
            //{
            //    FarFieldElementC ffe = farField[j];
            //    //ListPointPairs[k1].Add(ffe.LocalTheta, ffe.Etotal);
            //    //ListPointPairs[k2].Add(ffe.LocalTheta, ffe.Etheta);
            //    //ListPointPairs[k3].Add(ffe.LocalTheta, ffe.Ephi);

            //    if (j % count / 6 == 0)
            //    {
            //        AxisChange();
            //        Refresh();
            //    }
            //}
            //AxisChange();
            //Refresh();
        }
    }

    public class GraphData
    {
        public string Title;
        public double[] x;
        public double[] y;
        public string Component;
        public string Type;
        public Color Color;
        public GraphData(string _title, double[] _x, double[] _y, string _Component, string _Type, Color _color)
        {
            Title = _title;
            x = _x;
            y = _y;
            Component = _Component;
            Type = _Type;
            Color = _color;
        }
    }

}
