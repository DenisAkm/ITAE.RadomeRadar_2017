using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using Apparat.ShaderManagement;
using System.Drawing;

namespace Apparat
{
    public class Grid : Renderable
    {
        SlimDX.Direct3D11.Buffer vertexBuffer;
        DataStream vertices;
        Color color = Color.FromArgb(255, 51, 51, 51);
        int numVertices = 0;

        public Grid(float sizeArrange)
        {
            int cellsPerSide = 50;     //500
            float cellSize = sizeArrange;       //100
            int numLines = cellsPerSide + 1;
            float lineLength = cellsPerSide * cellSize;

            float xStart = -lineLength / 2.0f;
            float yStart = -lineLength / 2.0f;

            float xCurrent = xStart;
            float yCurrent = yStart;

            numVertices = 2 * 2 * numLines;
            int SizeInBytes = 12 * numVertices;

            vertices = new DataStream(SizeInBytes, true, true);
            float pushdown = 0.01f;
            for (int y = 0; y < numLines; y++)
            {
                vertices.Write(new Vector3(xCurrent, 0 - pushdown, yStart));
                vertices.Write(new Vector3(xCurrent, 0 - pushdown, yStart + lineLength));
                xCurrent += cellSize;
            }

            for (int x = 0; x < numLines; x++)
            {
                vertices.Write(new Vector3(xStart, 0 - pushdown, yCurrent));
                vertices.Write(new Vector3(xStart + lineLength, 0 - pushdown, yCurrent));
                yCurrent += cellSize;
            }

            vertices.Position = 0;

            // create the vertex buffer            
            vertexBuffer = new SlimDX.Direct3D11.Buffer(DeviceManager.Instance.device, vertices, SizeInBytes, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        EffectWrapperTransformEffect ew = ShaderManager.Instance.transformEffect;

        public override void Render()
        {
            Matrix ViewPerspective = CameraManager.Instance.ViewPerspective;
            Matrix WorldViewPerspective = this.transform * ViewPerspective;
            ew.tmat.SetMatrix(WorldViewPerspective);

            ew.mCol.Set(new Color4(1, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f));
            
            // configure the Input Assembler portion of the pipeline with the vertex data
            DeviceManager.Instance.context.InputAssembler.InputLayout = ew.layout;
            DeviceManager.Instance.context.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
            DeviceManager.Instance.context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, 12, 0));

            ew.technique = ew.effect.GetTechniqueByName("Render");

            EffectTechniqueDescription techDesc;
            techDesc = ew.technique.Description;

            for (int p = 0; p < techDesc.PassCount; ++p)
            {
                ew.technique.GetPassByIndex(p).Apply(DeviceManager.Instance.context);
                DeviceManager.Instance.context.Draw(numVertices, 0);
            }
        }

        public override void Dispose()
        {
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
