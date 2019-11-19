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
    public class ColorCube : Renderable
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



        public ColorCube(Vector3 pos, float size)
        {
            

            vertexStride = Marshal.SizeOf(typeof(PositionColoredVertex)); // 16 bytes
            numVertices = 8;
            vertexBufferSizeInBytes = vertexStride * numVertices;

            vertices = new DataStream(vertexBufferSizeInBytes, true, true);

            // half length of an edge
            float offset = 0.5f * size;
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X + offset, pos.Y + offset, pos.Z + offset), Color.FromArgb(255, 255, 255).ToArgb())); // 0
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X + offset, pos.Y + offset, pos.Z - offset), Color.FromArgb(255, 255, 000).ToArgb())); // 1
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X - offset, pos.Y + offset, pos.Z - offset), Color.FromArgb(000, 255, 000).ToArgb())); // 2
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X - offset, pos.Y + offset, pos.Z + offset), Color.FromArgb(000, 255, 255).ToArgb())); // 3

            vertices.Write(new PositionColoredVertex(new Vector3(pos.X - offset, pos.Y - offset, pos.Z + offset), Color.FromArgb(000, 000, 255).ToArgb())); // 4
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X + offset, pos.Y - offset, pos.Z + offset), Color.FromArgb(255, 000, 255).ToArgb())); // 5
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X + offset, pos.Y - offset, pos.Z - offset), Color.FromArgb(255, 000, 000).ToArgb())); // 6
            vertices.Write(new PositionColoredVertex(new Vector3(pos.X - offset, pos.Y - offset, pos.Z - offset), Color.FromArgb(000, 000, 000).ToArgb())); // 7

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

            numIndices = 36;
            indexStride = Marshal.SizeOf(typeof(short)); // 2 bytes
            indexBufferSizeInBytes = numIndices * indexStride;

            indices = new DataStream(indexBufferSizeInBytes, true, true);

            // Cube has 6 sides: top, bottom, left, right, front, back
            // top
            indices.WriteRange(new short[] { 0, 1, 2 });
            indices.WriteRange(new short[] { 2, 3, 0 });

            // right
            indices.WriteRange(new short[] { 0, 5, 6 });
            indices.WriteRange(new short[] { 6, 1, 0 });

            // left
            indices.WriteRange(new short[] { 2, 7, 4 });
            indices.WriteRange(new short[] { 4, 3, 2 });

            // front
            indices.WriteRange(new short[] { 1, 6, 7 });
            indices.WriteRange(new short[] { 7, 2, 1 });

            // back
            indices.WriteRange(new short[] { 3, 4, 5 });
            indices.WriteRange(new short[] { 5, 0, 3 });

            // bottom
            indices.WriteRange(new short[] { 6, 5, 4 });
            indices.WriteRange(new short[] { 4, 7, 6 });

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
            DeviceManager.Instance.context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
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
