using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpDX.D3DCompiler;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;
using Apparat.ShaderManagement;
using Color = SharpDX.Color;
using SharpDX.Direct3D;

namespace Apparat
{
    public class Points: Renderable
    {
        public string Title { get; set; }
        public double StartTheta { get; set; }
        public double FinishTheta { get; set; }
        public double PhiStart { get; set; }
        public double PhiFinish { get; set; }
        public int Scantype { get; set; }

        public double ThetaStep { get; set; }
        public double PhiStep { get; set; }

        double pi = Math.PI;

        SharpDX.Direct3D11.Buffer vertexBuffer;
        SharpDX.Direct3D11.Buffer indexBuffer;
        DataStream vertices;
        DataStream indices;

        int vertexStride = 0;
        int numVertices = 0;
        int indexStride = 0;
        int numIndices = 0;

        int vertexBufferSizeInBytes = 0;
        int indexBufferSizeInBytes = 0;

        EffectWrapperColorEffectWireframe ew = ShaderManager.Instance.colorEffectWireframe;

        public Points(float size, string title, double startT, double finishT, double startP, double finishP, double stepT, double stepP, int scantype, int color, float disLevel)
        {
            Title = title;
            StartTheta = startT;
            FinishTheta = finishT;
            PhiStart = startP;
            PhiFinish = finishP;
            Scantype = scantype;
            ThetaStep = stepT;
            PhiStep = stepP;            

            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = Convert.ToInt32((Convert.ToInt32(finishT - startT)) / stepT) + 1;
            if (numVertices > 500)
            {
                numVertices = 500;
                stepT = (finishT - startT) / (numVertices - 1);
            }
            
            
            vertexBufferSizeInBytes = vertexStride * numVertices;
            vertices = new DataStream(vertexBufferSizeInBytes, true, true);

            double Rho = disLevel * size;
            for (int i = 0; i < numVertices; i++)
            {
                double a, b, c;
                double alpha = Convert.ToInt32(startT) + i * stepT;
                if (scantype == 0)
                {
                    a = Rho * Math.Sin(alpha * pi / 180) * Math.Cos(startP * pi / 180);
                    b = Rho * Math.Sin(alpha * pi / 180) * Math.Sin(startP * pi / 180);
                    c = Rho * Math.Cos(alpha * pi / 180);
                    vertices.Write(new PositionColoredVertex(new Vector3(Convert.ToSingle(a), Convert.ToSingle(c), Convert.ToSingle(b)), color));
                }
                else if (scantype == 1)
                {
                    c = Rho * Math.Sin(alpha * pi / 180) * Math.Cos(startP * pi / 180);
                    a = Rho * Math.Sin(alpha * pi / 180) * Math.Sin(startP * pi / 180);
                    b = Rho * Math.Cos(alpha * pi / 180);

                    vertices.Write(new PositionColoredVertex(new Vector3(Convert.ToSingle(a), Convert.ToSingle(c), Convert.ToSingle(b)), color));
                }
                else if (scantype == 2)
                {
                    b = Rho * Math.Sin(alpha * pi / 180) * Math.Cos(startP * pi / 180);
                    c = Rho * Math.Sin(alpha * pi / 180) * Math.Sin(startP * pi / 180);
                    a = Rho * Math.Cos(alpha * pi / 180);
                    vertices.Write(new PositionColoredVertex(new Vector3(Convert.ToSingle(a), Convert.ToSingle(c), Convert.ToSingle(b)), color));
                }
            }

            vertices.Position = 0;

            vertexBuffer = new SharpDX.Direct3D11.Buffer(
               DeviceManager.Instance.device,
               vertices,
               vertexBufferSizeInBytes,
               ResourceUsage.Default,
               BindFlags.VertexBuffer,
               CpuAccessFlags.None,
               ResourceOptionFlags.None,
               0);

            numIndices = numVertices;
            indexStride = Marshal.SizeOf(typeof(short)); // 2 bytes
            indexBufferSizeInBytes = numIndices * indexStride;

            indices = new DataStream(indexBufferSizeInBytes, true, true);

            for (int i = 0; i < numVertices; i++)
            {
                indices.Write((short)i);    
            }                    

            indices.Position = 0;

            indexBuffer = new SharpDX.Direct3D11.Buffer(
                DeviceManager.Instance.device,
                indices,
                indexBufferSizeInBytes,
                ResourceUsage.Default,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0);
        }

        public Points(float size, string title, List<Point3D> pointList, int color, float disLevel)
        {
            Title = title;
            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = pointList.Count;            

            vertexBufferSizeInBytes = vertexStride * numVertices;
            vertices = new DataStream(vertexBufferSizeInBytes, true, true);

            double Rho = disLevel * size;
            for (int i = 0; i < numVertices; i++)
            {
                vertices.Write(new PositionColoredVertex(new Vector3(Convert.ToSingle(Rho * pointList[i].X), Convert.ToSingle(Rho * pointList[i].Z), Convert.ToSingle(Rho * pointList[i].Y)), color));
            }

            vertices.Position = 0;

            vertexBuffer = new SharpDX.Direct3D11.Buffer(
               DeviceManager.Instance.device,
               vertices,
               vertexBufferSizeInBytes,
               ResourceUsage.Default,
               BindFlags.VertexBuffer,
               CpuAccessFlags.None,
               ResourceOptionFlags.None,
               0);

            numIndices = numVertices;
            indexStride = Marshal.SizeOf(typeof(short)); // 2 bytes
            indexBufferSizeInBytes = numIndices * indexStride;

            indices = new DataStream(indexBufferSizeInBytes, true, true);

            for (int i = 0; i < numVertices; i++)
            {
                indices.Write((short)i);
            }

            indices.Position = 0;

            indexBuffer = new SharpDX.Direct3D11.Buffer(
                DeviceManager.Instance.device,
                indices,
                indexBufferSizeInBytes,
                ResourceUsage.Default,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0);
        }
        public override void Render()
        {
            Matrix ViewPerspective = CameraManager.Instance.ViewPerspective;
            Matrix WorldViewPerspective = this.transform * ViewPerspective;
            ew.tmat.SetMatrix(WorldViewPerspective);

            DeviceManager.Instance.context.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;            
            DeviceManager.Instance.context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexStride, 0));
            DeviceManager.Instance.context.InputAssembler.SetIndexBuffer(indexBuffer, Format.R16_UInt, 0);

            ew.technique = ew.effect.GetTechniqueByName("Render");

            EffectTechniqueDescription techDesc;
            techDesc = ew.technique.Description;

            for (int p = 0; p < techDesc.PassCount; ++p)
            {
                ew.technique.GetPassByIndex(p).Apply(DeviceManager.Instance.context);
                DeviceManager.Instance.context.DrawIndexed(numIndices, 0, 0);
            }
        }

        public override void Dispose()
        {
            vertexBuffer.Dispose();
        }
        public override Matrix Transform
        {
            get
            {
                return this.transform;
            }
            set
            {
                this.transform = value;
            }
        }
    }
}
