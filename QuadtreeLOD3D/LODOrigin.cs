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
    public class LODOrigin
    {
        public BoundingSphere[] LODs { get; private set; }

        private QuadTree3D qTree;

        public static SimplexNoiseGenerator Simplex;

        public LODOrigin(GraphicsDevice g, int nWidth, int nHeight, int nDepth)
        {

            Simplex = new SimplexNoiseGenerator(0, 1.0f / 8.0f, 1.0f / 8.0f, 1.0f / 16.0f, 1.0f / 16.0f)
            {
                Factor = 1.5f,
                Sealevel = 0,
                Octaves = 6
            }; 


            LODs = new BoundingSphere[]
            {
                new BoundingSphere(Vector3.Zero, 500),
                new BoundingSphere(Vector3.Zero, 450),
                new BoundingSphere(Vector3.Zero, 350),
                new BoundingSphere(Vector3.Zero, 250),
                new BoundingSphere(Vector3.Zero, 150),
                new BoundingSphere(Vector3.Zero, 130),
                new BoundingSphere(Vector3.Zero, 065),
                new BoundingSphere(Vector3.Zero, 030),
                new BoundingSphere(Vector3.Zero, 020),
                new BoundingSphere(Vector3.Zero, 010),

            };


            qTree = new QuadTree3D(g, 0, 0, 0, nWidth, nHeight, nDepth);
        }

        public static bool eq(BoundingSphere a, BoundingSphere b)
        {
            return a.Radius == b.Radius;
        }


        public void Move(float x, float y, float z)
        {
            for (int i = 0; i < LODs.Length; i++)
            {
                LODs[i].Center = new Vector3(x, y, z);
            }
        }
            
        public void Update()
        {
            for (int i = 0; i < LODs.Length; i++)
            {
                qTree.Update(LODs[i]);
            }
        }
        public void Draw()
        {
            qTree.Draw();
        }

    }
}
