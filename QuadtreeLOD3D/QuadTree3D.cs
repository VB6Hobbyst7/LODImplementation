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
    public class QuadTree3D
    {

        public GraphicsDevice GraphicsDevice { get; private set; }

        public BoundingBox ChunkDefinition { get; private set; }

        public QuadTree3D Parent { get; private set; }
        public QuadTree3D[] Childs { get; private set; }

        private BoundingSphere correspondingSphere;

         
        private float X, Y, Z, W, H, D;


        private VertexChunk lVertexChunk;


        public QuadTree3D(GraphicsDevice g, float x, float y, float z, float w, float h, float d)
        {

            GraphicsDevice = g;

            ChunkDefinition = new BoundingBox(new Vector3(x, y, z), new Vector3(x + w, y + h, z + d));

            X = x;
            Y = y;
            Z = z;

            W = w;
            H = h;
            D = d;

            lVertexChunk = new VertexChunk(GraphicsDevice, X, Z, W, D);

            //ChunkDefinition = new BoundingBox(new Vector3(x, y + lVertexChunk.MaximumHeight, z), new Vector3(x + w, y + h + lVertexChunk.MaximumHeight, z + d));

        }



        public void Update(BoundingSphere sphere)
        {
            if (Childs != null)
                for (int i = 0; i < Childs.Length; i++)
                {
                    Childs[i].Check(sphere);
                }

            if (Check(sphere))
                return;

            if (Parent != null && LODOrigin.eq(Parent.correspondingSphere, sphere))
                return;

            if (Childs == null)
            {
                correspondingSphere = sphere;

                float nW = W * 0.5f;
                float nH = H * 0.5f;
                float nD = D * 0.5f;

                Childs = new QuadTree3D[]
                {
                    new QuadTree3D(GraphicsDevice, X, Y, Z, nW, nH, nD) {Parent = this },
                    new QuadTree3D(GraphicsDevice, X + nW, Y, Z, nW, nH, nD) {Parent = this },
                    new QuadTree3D(GraphicsDevice, X, Y, Z +nD, nW, nH, nD) {Parent = this },
                    new QuadTree3D(GraphicsDevice, X + nW, Y, Z + nD, nW, nH, nD) {Parent = this }


                };
            }
            else for (int i = 0; i < Childs.Length; i++)
                {
                    Childs[i].Update(sphere);
                }

        }

        public bool Check(BoundingSphere c)
        {

            if (Childs != null)
                for (int i = 0; i < Childs.Length; i++)
                {
                    Childs[i].Check(c);
                }

            if (ChunkDefinition.Contains(c) == ContainmentType.Disjoint)
            {
                if (correspondingSphere != null && LODOrigin.eq(correspondingSphere, c))
                {
                    if (Childs != null)
                    {

                        for (int i = 0; i < Childs.Length; i++)
                        {
                            Childs[i].lVertexChunk.Clear();
                        }

                        Childs = new QuadTree3D[0];
                        Childs = null;
                    }
                }

                return true;
            }
            return false;
        }

        public void Draw()
        {
            if (Childs != null)
                for (int i = 0; i < Childs.Length; i++)
                {
                    Childs[i].Draw();
                }

            else
            {
                if (Camera.BoundingFrustum.Contains(this.ChunkDefinition) == ContainmentType.Disjoint) ;


                lVertexChunk.Draw();
                //BoundingBoxRenderer.Render(ChunkDefinition, GraphicsDevice, Camera.View, Camera.Projection, Color.Red);
            }
        }

    }
}
