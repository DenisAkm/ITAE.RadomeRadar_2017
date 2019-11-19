using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class SourceTemplateold
    {
        public string Lable { get; set;}
        public CVector[] I { get; set; }
        public CVector[] M { get; set; }
        public int Polarization { get; set; }
        public string Distribution { get; set; }
        public bool DifferenceChanel { get; set; }
        public string DifferenceAxis { get; set; }
        public int Scanning { get; set; }
        public int SystemOfCoordinatesScan { get; set; }
        public double ThetaScanEStart { get; set; }
        public double ThetaScanEFinish { get; set; }
        public double ThetaScanEStep { get; set; }
        public double PhiScanEStart { get; set; }
        public double PhiScanEFinish { get; set; }
        public double PhiScanEStep { get; set; }

        public bool Include = true;

        public double axis1x1 { get; set; }
        public double axis1z1 { get; set; }
        public double axis1y1 { get; set; }

        public double axis1x2 { get; set; }
        public double axis1y2 { get; set; }
        public double axis1z2 { get; set; }

        public double axis2x1 { get; set; }
        public double axis2y1 { get; set; }
        public double axis2z1 { get; set; }

        public double axis2x2 { get; set; }
        public double axis2y2 { get; set; }
        public double axis2z2 { get; set; }

        public double PhiScanMStart { get; set; }
        public double PhiScanMFinish { get; set; }
        public double PhiScanMStep { get; set; }

        public double ThetaScanMStart { get; set; }
        public double ThetaScanMFinish { get; set; }
        public double ThetaScanMStep { get; set; }

        
        public SourceTemplateold(CreateApertureForm form)
        {
            Distribution = form.comboBoxDistribution.SelectedItem.ToString();
            Lable = form.textBoxApertureTitle.Text;
            if (Distribution == "Загрузить из файла")
            {
                //I = form.icurInfo;
                //M = form.mcurInfo;                
            }
            else
            {                
                Complex Ix = new Complex(Convert.ToDouble(form.textBoxAppertureIxRe.Text), Convert.ToDouble(form.textBoxAppertureIxIm.Text));
                Complex Iy = new Complex(Convert.ToDouble(form.textBoxAppertureIyRe.Text), Convert.ToDouble(form.textBoxAppertureIyIm.Text));
                Complex Iz = new Complex(Convert.ToDouble(form.textBoxAppertureIzRe.Text), Convert.ToDouble(form.textBoxAppertureIzIm.Text));
                Complex Mx = new Complex(Convert.ToDouble(form.textBoxAppertureMxRe.Text), Convert.ToDouble(form.textBoxAppertureMxIm.Text));
                Complex My = new Complex(Convert.ToDouble(form.textBoxAppertureMyRe.Text), Convert.ToDouble(form.textBoxAppertureMyIm.Text));
                Complex Mz = new Complex(Convert.ToDouble(form.textBoxAppertureMzRe.Text), Convert.ToDouble(form.textBoxAppertureMzIm.Text));

                I = new CVector[1];
                M = new CVector[1];

                for (int i = 0; i < 1; i++)
                {
                    I[i] = new CVector(Ix, Iy, Iz);
                    M[i] = new CVector(Mx, My, Mz);
                }
            }
            
            
            //Polarization = form.comboBoxPolarization.SelectedIndex;

            
            if (form.radioButtonChannel1.Checked)
            {
                DifferenceChanel = false;
            }
            else
            {
                DifferenceChanel = true;
            }
            DifferenceAxis = form.comboBoxDiffAxis.SelectedItem.ToString();
            Scanning = form.comboBoxScanning.SelectedIndex;
            SystemOfCoordinatesScan = form.comboBoxSysOfCoordScan.SelectedIndex;
            ThetaScanEStart = Convert.ToDouble(form.textBoxScanThetaStart.Text);
            ThetaScanEFinish = Convert.ToDouble(form.textBoxScanThetaFinish.Text);
            ThetaScanEStep = Convert.ToDouble(form.textBoxScanThetaStep.Text);

            PhiScanEStart = Convert.ToDouble(form.textBoxScanPhiStart.Text);
            PhiScanEFinish = Convert.ToDouble(form.textBoxScanPhiFinish.Text);
            PhiScanEStep = Convert.ToDouble(form.textBoxScanPhiStep.Text);            

            axis1x1 = Convert.ToDouble(form.textBoxRotAxis1X1.Text);
            axis1y1 = Convert.ToDouble(form.textBoxRotAxis1Y1.Text);
            axis1z1 = Convert.ToDouble(form.textBoxRotAxis1Z1.Text);

            axis1x2 = Convert.ToDouble(form.textBoxRotAxis1X2.Text);
            axis1y2 = Convert.ToDouble(form.textBoxRotAxis1Y2.Text);
            axis1z2 = Convert.ToDouble(form.textBoxRotAxis1Z2.Text);

            axis2x1 = Convert.ToDouble(form.textBoxRotAxis2X1.Text);
            axis2y1 = Convert.ToDouble(form.textBoxRotAxis2Y1.Text);
            axis2z1 = Convert.ToDouble(form.textBoxRotAxis2Z1.Text);

            axis2x2 = Convert.ToDouble(form.textBoxRotAxis2X2.Text);
            axis2y2 = Convert.ToDouble(form.textBoxRotAxis2Y2.Text);
            axis2z2 = Convert.ToDouble(form.textBoxRotAxis2Z2.Text);

            PhiScanMStart = Convert.ToDouble(form.textBoxMAngle2Start.Text);
            PhiScanMFinish = Convert.ToDouble(form.textBoxMAngle2Finish.Text);
            PhiScanMStep = Convert.ToDouble(form.textBoxMAngle2Step.Text);

            ThetaScanMStart = Convert.ToDouble(form.textBoxMAngle1Start.Text);
            ThetaScanMFinish = Convert.ToDouble(form.textBoxMAngle1Finish.Text);
            ThetaScanMStep = Convert.ToDouble(form.textBoxMAngle1Step.Text);   
        }
        public SourceTemplateold(string lable, CVector[] i, CVector[] m, int polariztion, string distribution, bool differenceChanel, string differenceAxis, int scanning, 
            int systemOfCoordinatesScan, double thetaScanStart, double thetaScanFinish, double thetaScanStep, double phiScanStart, double phiScanFinish, double phiScanStep, bool include, double _axis1x1, 
            double _axis1y1, double _axis1z1, double _axis1x2, double _axis1y2, double _axis1z2, double _axis2x1, double _axis2y1, double _axis2z1, 
            double _axis2x2, double _axis2y2, double _axis2z2, double _scanMPhiStart, double _scanMPhiFinish, double _scanMPhiStep, double _scanMThetaStart, 
            double _scanMThetaFinish, double _scanMThetaStep)
        {
            Lable = lable;
            I = i;
            M = m;
            Polarization = polariztion;
            Distribution = distribution;
            DifferenceChanel = differenceChanel;
            DifferenceAxis = differenceAxis;
            Scanning = scanning;
            SystemOfCoordinatesScan = systemOfCoordinatesScan;
            ThetaScanEStart = thetaScanStart;
            ThetaScanEFinish = thetaScanFinish;
            ThetaScanEStep = thetaScanStep;

            PhiScanEStart = phiScanStart;
            PhiScanEFinish = phiScanFinish;
            PhiScanEStep = phiScanStep;
            
            Include = include;

            axis1x1 = _axis1x1;
            axis1y1 = _axis1y1;
            axis1z1 = _axis1z1;
            axis1x2 = _axis1x2;
            axis1y2 = _axis1y2;
            axis1z2 = _axis1z2;
            axis2x1 = _axis2x1;
            axis2y1 = _axis2y1;
            axis2z1 = _axis2z1;
            axis2x2 = _axis2x2;
            axis2y2 = _axis2y2;
            axis2z2 = _axis2z2;
            PhiScanMStart = _scanMPhiStart;
            PhiScanMFinish = _scanMPhiFinish;
            PhiScanMStep = _scanMPhiStep;
            ThetaScanMStart = _scanMThetaStart;
            ThetaScanMFinish = _scanMThetaFinish;
            ThetaScanMStep = _scanMThetaStep;
        }

        public SourceTemplateold Copy()
        {
            string lable = this.Lable + "_копия";
            CVector[] i = this.I;
            CVector[] m = this.M;
            int polatization = this.Polarization;
            string distribution = this.Distribution;
            bool differenceChanel = this.DifferenceChanel;
            string differenceAxis = this.DifferenceAxis;
            int scanning = this.Scanning;
            int systemOfCoordinatesScan = this.SystemOfCoordinatesScan;
            double thetaScanStart = this.ThetaScanEStart;
            double thetaScanFinish = this.ThetaScanEFinish;
            double thetaScanStep = this.ThetaScanEStep;

            double phiScanStart = this.PhiScanEStart;
            double phiScanFinish = this.PhiScanEFinish;
            double phiScanStep = this.PhiScanEStep;
            bool include = this.Include;

            double _axis1x1 = this.axis1x1;
            double _axis1y1 = this.axis1y1;
            double _axis1z1 = this.axis1z1;
            double _axis1x2 = this.axis1x2;
            double _axis1y2 = this.axis1y2;
            double _axis1z2 = this.axis1z2;
            double _axis2x1 = this.axis2x1;
            double _axis2y1 = this.axis2y1;
            double _axis2z1 = this.axis2z1;
            double _axis2x2 = this.axis2x2;
            double _axis2y2 = this.axis2y2;
            double _axis2z2 = this.axis2z2;
            double _scanMPhiStart = this.PhiScanMStart;
            double _scanMPhiFinish = this.PhiScanMFinish;
            double _scanMPhiStep = this.PhiScanMStep;
            double _scanMThetaStart = this.ThetaScanMStart;
            double _scanMThetaFinish = this.ThetaScanMFinish;
            double _scanMThetaStep = this.ThetaScanMStep;

            return new SourceTemplateold(lable, i, m, polatization, distribution, differenceChanel, differenceAxis, scanning, 
                systemOfCoordinatesScan, thetaScanStart, thetaScanFinish, thetaScanStep, phiScanStart, phiScanFinish, phiScanStep, 
                include, _axis1x1, _axis1y1, _axis1z1, _axis1x2, _axis1y2, _axis1z2, _axis2x1, _axis2y1, _axis2z1, _axis2x2, _axis2y2, _axis2z2, 
                _scanMPhiStart, _scanMPhiFinish, _scanMPhiStep, _scanMThetaStart, _scanMThetaFinish, _scanMThetaStep);
        }

        
    }    
}
