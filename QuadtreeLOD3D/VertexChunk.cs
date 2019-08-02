using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuadtreeLOD3D
{
    public class VertexChunk
    {
        public GraphicsDevice GraphicsDevice { get; private set; }

        public float Width { get; private set; }
        public float Depth { get; private set; }

        public float MaximumHeight;


        private BasicEffect bEffect;

        private VertexPositionTexture[] aVertices;
        private int[] aIndices;

        private VertexBuffer cVertexBuffer;
        private IndexBuffer cIndexBuffer;

        
       
        public VertexChunk (GraphicsDevice pGraphics, float pX, float pY, float pWidth, float pDepth)
        {
            GraphicsDevice = pGraphics;

            Width = pWidth;
            Depth = pDepth;

            bEffect = new BasicEffect(GraphicsDevice);

            float stepX = (Width * 0.05f);
            float stepZ = (Depth * 0.05f);

            int lW = (int)(this.Width / stepX) + 1;
            int lD = (int)(this.Depth / stepZ) + 1;

            aVertices = new VertexPositionTexture[lW * lD];
            MaximumHeight = int.MinValue;
            
            
            for (int z = 0; z < lD; z++)
            {
                for (int x = 0; x < lW; x++)
                {
                    float y = LODOrigin.Simplex.GetNoise2D(x * stepX + pX, z * stepZ + pY);

                    if (y > MaximumHeight)
                        MaximumHeight = y;

                    VertexPositionTexture lVertex = new VertexPositionTexture(new Vector3(x * stepX + pX, y, z * stepZ + pY), new Vector2(x, z));
                    aVertices[x + z * lW] = lVertex;
                }
            }


            aIndices = new int[lW * lD * 6];

            int index = 0;


            for (int z = 0; z < lD - 1; z++)
            {
                for (int x = 0; x < lW - 1; x++)
                {

                    aIndices[index++] = x + z * lW;
                    aIndices[index++] = (x + 1) + z * lW;
                    aIndices[index++] = (x + 1) + (z + 1) * lW;

                    aIndices[index++] = (x + 1) + (z + 1) * lW;
                    aIndices[index++] = x + (z + 1) * lW;
                    aIndices[index++] = x + z * lW;
                }
            }


            cVertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), aVertices.Length, BufferUsage.WriteOnly);
            cIndexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, aIndices.Length, BufferUsage.WriteOnly);

            cVertexBuffer.SetData<VertexPositionTexture>(aVertices);
            cIndexBuffer.SetData<int>(aIndices);

        }

        public void Draw()
        {
            GraphicsDevice.SetVertexBuffer(cVertexBuffer);
            GraphicsDevice.Indices = cIndexBuffer;

 

            bEffect.View = Camera.View;
            bEffect.Projection = Camera.Projection;
            bEffect.Texture = Game1.rTexture2D;
            bEffect.TextureEnabled = true;

            bEffect.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, aVertices.Length * 2);

        }

        public void Clear()
        {
            cVertexBuffer.Dispose();
            cIndexBuffer.Dispose();

            aVertices = new VertexPositionTexture[0];
            aIndices = new int[0];

            bEffect.Dispose();
        }


    }
}
