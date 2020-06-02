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
using Color = System.Drawing.Color;
using SharpDX.Direct3D;

namespace Apparat
{
    public class CoordSystem : Renderable
    {
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
        
        int ColorRed = Color.FromArgb(220, 40, 20).ToArgb();
        int ColorBlue = Color.FromArgb(63, 72, 204).ToArgb();
        int ColorGreen = Color.FromArgb(20, 170, 50).ToArgb();
        

        public CoordSystem(float size)
        {
            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = 6;
            vertexBufferSizeInBytes = vertexStride * numVertices;

            vertices = new DataStream(vertexBufferSizeInBytes, true, true);

            float length = 1;
            length = length * size;
            float pushUp = 0;
            vertices.Write(new PositionColoredVertex(new Vector3(0, 0 + pushUp, 0), ColorRed));
            vertices.Write(new PositionColoredVertex(new Vector3(0, length + pushUp, 0), ColorRed));  //красная ось Z
            vertices.Write(new PositionColoredVertex(new Vector3(0, 0 + pushUp, 0), ColorGreen));
            vertices.Write(new PositionColoredVertex(new Vector3(0, 0 + pushUp, length), ColorGreen));  //зеленая ось Y
            vertices.Write(new PositionColoredVertex(new Vector3(0, 0 + pushUp, 0), ColorBlue));
            vertices.Write(new PositionColoredVertex(new Vector3(length, 0 + pushUp, 0), ColorBlue));  //синяя ось Х

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

            numIndices = 2 * numVertices;
            indexStride = Marshal.SizeOf(typeof(short)); // 2 bytes
            indexBufferSizeInBytes = numIndices * indexStride;

            indices = new DataStream(indexBufferSizeInBytes, true, true);

            //прямая сторона
            indices.WriteRange(new short[] { (short)0, (short)1 });
            indices.WriteRange(new short[] { (short)2, (short)3 });
            indices.WriteRange(new short[] { (short)4, (short)5 });

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

        EffectWrapperColorEffectWireframe ew = ShaderManager.Instance.colorEffectWireframe;

        public override void Render()
        {
            Matrix ViewPerspective = CameraManager.Instance.ViewPerspective;
            Matrix WorldViewPerspective = this.transform * ViewPerspective;
            ew.tmat.SetMatrix(WorldViewPerspective);

            DeviceManager.Instance.context.InputAssembler.InputLayout = ew.layout;
            DeviceManager.Instance.context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
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


