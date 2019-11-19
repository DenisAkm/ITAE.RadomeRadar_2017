using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class FarFieldRequestTemplate
    {
        public double ThetaStart { get; set; }
        public double ThetaFinish { get; set; }
        public double PhiStart { get; set; }        
        public double Delta { get; set; }
        public int SystemOfCoordinates { get; set; }
        public bool AntenaField { get; set; }
        public bool RadomeField { get; set; }
        public bool ReflactedField { get; set; }
        public bool ReflactedItField { get; set; }

        public string Direction { get; set; }

        public double BodyAngle { get; set; }

        public double BodyAngleStep { get; set; }


        public int FarFieldType { get; set; }
        public bool Include { get; set; }

        public string Lable { get; set; }

        public FarFieldRequestTemplate(FarFieldRequestForm form)
        {
            ThetaStart = FarFieldRequestForm.ThetaStart;
            ThetaFinish = FarFieldRequestForm.ThetaFinish;
            PhiStart = FarFieldRequestForm.PhiStart;            
            Delta = FarFieldRequestForm.Delta;
            Direction = FarFieldRequestForm.Direction;
            BodyAngle = FarFieldRequestForm.BodyAngle;
            BodyAngleStep = FarFieldRequestForm.BodyAngleStep;
            SystemOfCoordinates = FarFieldRequestForm.SystemOfCoordinates;
            Lable = form.textBoxTitle.Text;
            FarFieldType = FarFieldRequestForm.FarFieldType;

            AntenaField = FarFieldRequestForm.AntenaField;
            RadomeField = FarFieldRequestForm.RadomeField;
            ReflactedField = FarFieldRequestForm.ReflactedField;
            ReflactedItField = FarFieldRequestForm.ReflactedItField;
            Include = true;
        }
        public FarFieldRequestTemplate(double thetaStart, double thetaFinish, double phiStart, double delta, string direction, double bodyAngle, double bodyAngleStep, int systemOfCoordinates, string lable, bool antenaField, bool radomeField, bool reflactedField, bool reflactedItField, bool include, int farFieldType)
        {
            ThetaStart = thetaStart;
            ThetaFinish = thetaFinish;
            PhiStart = phiStart;
            Delta = delta;
            SystemOfCoordinates = systemOfCoordinates;
            Direction = direction;
            BodyAngle = bodyAngle;
            BodyAngleStep = bodyAngleStep;
            Lable = lable;
            FarFieldType = farFieldType;

            AntenaField = antenaField;
            RadomeField = radomeField;
            ReflactedField = reflactedField;
            ReflactedItField = reflactedItField;
            Include = include;
            SetCurrentParametersAsDefaults();
        }

        private void SetCurrentParametersAsDefaults()
        {
            FarFieldRequestForm.ThetaStart = ThetaStart;
            FarFieldRequestForm.ThetaFinish = ThetaFinish;
            FarFieldRequestForm.PhiStart = PhiStart;
            FarFieldRequestForm.Delta = Delta;
            FarFieldRequestForm.Direction = Direction;
            FarFieldRequestForm.BodyAngle = BodyAngle;
            FarFieldRequestForm.BodyAngleStep = BodyAngleStep;
            FarFieldRequestForm.SystemOfCoordinates = SystemOfCoordinates;

            FarFieldRequestForm.FarFieldType = FarFieldType;

            FarFieldRequestForm.AntenaField = AntenaField;
            FarFieldRequestForm.RadomeField = RadomeField;
            FarFieldRequestForm.ReflactedField = ReflactedField;
            FarFieldRequestForm.ReflactedItField = ReflactedItField;            
        }
        public FarFieldRequestTemplate Copy()
        {
            double thetaStart = this.ThetaStart;
            double thetaFinish = this.ThetaFinish;
            double phiStart = this.PhiStart;
            double delta = this.Delta;
            string direction = this.Direction;
            double bodyangle = this.BodyAngle;
            double bodyanglestep = this.BodyAngleStep;
            int systemOfCoordinates = this.SystemOfCoordinates;
            string lable = this.Lable + "_копия";
            int farFiledType = FarFieldType;

            bool antenaField = this.AntenaField;
            bool radomeField = this.RadomeField;
            bool reflactedField = this.ReflactedField;
            bool reflactedItField = this.ReflactedItField;
            bool include = this.Include;
            return new FarFieldRequestTemplate(thetaStart, thetaFinish, phiStart, delta, direction, bodyangle, bodyanglestep, systemOfCoordinates, lable, antenaField, radomeField, reflactedField, reflactedItField, include, farFiledType);
        }
    }
}
