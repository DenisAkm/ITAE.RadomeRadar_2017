using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using System.Runtime.InteropServices;
using Apparat.ShaderManagement;

namespace Apparat
{
    public class AntennaMesh : Renderable
    {
        SlimDX.Direct3D11.Buffer vertexBuffer;
        SlimDX.Direct3D11.Buffer indexBuffer;
        DataStream vertices;
        DataStream indices;

        int vertexStride = 0;
        int numVertices = 0;
        int indexStride = 0;
        int numIndices = 0;

        int vertexBufferSizeInBytes = 0;
        int indexBufferSizeInBytes = 0;

        public string Lable { get; set; }
        public Color Color { get; set; }
        public double DiagSize{get; set;}

        public AntennaMesh(string lable, List<double> x, List<double> y, List<double> z, List<int> P1, List<int> P2, List<int> P3, Color color, double diagSize)
        {
            Lable = lable;
            Color = color;
            DiagSize = diagSize;

            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = x.Count;
            vertexBufferSizeInBytes = vertexStride * numVertices;

            vertices = new DataStream(vertexBufferSizeInBytes, true, true);


            float a, b, c;                        
            int arbgColor = color.ToArgb();

            for (int i = 0; i < x.Count; i++)
            {
                a = Convert.ToSingle(x[i]);
                b = Convert.ToSingle(y[i]);
                c = Convert.ToSingle(z[i]);
                vertices.Write(new PositionColoredVertex(new Vector3(a, c, b), arbgColor));
            }            

            vertices.Position = 0;

            vertexBuffer = new SlimDX.Direct3D11.Buffer(
               DeviceManager.Instance.device,
               vertices,
               vertexBufferSizeInBytes,
               ResourceUsage.Default,
               BindFlags.VertexBuffer,
               CpuAccessFlags.None,
               ResourceOptionFlags.None,
               0);

            numIndices = P1.Count * 6;
            indexStride = Marshal.SizeOf(typeof(short)); // 2 bytes
            indexBufferSizeInBytes = numIndices * indexStride;

            indices = new DataStream(indexBufferSizeInBytes, true, true);

            for (int i = 0; i < P1.Count; i++)
            {

                short d1 = (short)(Convert.ToDouble(P1[i]) - 1);
                short d2 = (short)(Convert.ToDouble(P2[i]) - 1);
                short d3 = (short)(Convert.ToDouble(P3[i]) - 1);

                //прямая сторона
                indices.WriteRange(new short[] { d1, d2, d3 });
                ////обратная сторона
                indices.WriteRange(new short[] { d1, d3, d2 });
            }            

            indices.Position = 0;

            indexBuffer = new SlimDX.Direct3D11.Buffer(
                DeviceManager.Instance.device,
                indices,
                indexBufferSizeInBytes,
                ResourceUsage.Default,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0);

        }

        public AntennaMesh(string lable, Point3D[] points, Color color)
        {
            Lable = lable;
            Color = color;            

            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = points.Length;
            vertexBufferSizeInBytes = vertexStride * numVertices;

            vertices = new DataStream(vertexBufferSizeInBytes, true, true);


            float a, b, c;
            int arbgColor = color.ToArgb();

            for (int i = 0; i < numVertices; i++)
            {
                a = Convert.ToSingle(points[i].X);
                b = Convert.ToSingle(points[i].Y);
                c = Convert.ToSingle(points[i].Z);
                vertices.Write(new PositionColoredVertex(new Vector3(a, c, b), arbgColor));
            }

            vertices.Position = 0;

            vertexBuffer = new SlimDX.Direct3D11.Buffer(
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

            for (int i = 0; i < numIndices; i++)
            {
                indices.Write((short)i);
                //short d1 = (short)(Convert.ToDouble(P1[i]) - 1);
                //short d2 = (short)(Convert.ToDouble(P2[i]) - 1);
                //short d3 = (short)(Convert.ToDouble(P3[i]) - 1);

                ////прямая сторона
                //indices.WriteRange(new short[] { d1, d2, d3 });
                //////обратная сторона
                //indices.WriteRange(new short[] { d1, d3, d2 });
            }

            indices.Position = 0;

            indexBuffer = new SlimDX.Direct3D11.Buffer(
                DeviceManager.Instance.device,
                indices,
                indexBufferSizeInBytes,
                ResourceUsage.Default,
                BindFlags.IndexBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0);

        }

        EffectWrapperColorEffectWireframe ew = ShaderManager.Instance.colorEffectWireframe;

        public override void Render()
        {
            Matrix ViewPerspective = CameraManager.Instance.ViewPerspective;
            Matrix WorldViewPerspective = this.transform * ViewPerspective;
            ew.tmat.SetMatrix(WorldViewPerspective);            

            DeviceManager.Instance.context.InputAssembler.InputLayout = ew.layout;
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
