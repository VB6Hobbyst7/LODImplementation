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
    public class Camera
    {
        public static Vector3 REFERENCEVECTOR = new Vector3(0, 0, -1);

        public static GraphicsDevice GraphicsDevice { get; set; }

        public static float Yaw { get; private set; }
        public static float Pitch { get; private set; }

        public static float MouseSensity { get; set; }
        public static float Velocity { get; set; }

        public static Matrix View, Projection;

        public static Vector3 CameraPosition;
        public static Vector3 Direction { get; private set; }

        public static BoundingFrustum BoundingFrustum { get; private set; }

        private static int oldX, oldY;

        public Camera(GraphicsDevice device, float dpi, float velocity)
        {
            GraphicsDevice = device;

            MouseSensity = dpi;
            Velocity = velocity;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, .01f, 1500.0f); ;

            CameraPosition = new Vector3(5, 200, 5);
        }

        public static void Update()
        {
            float dX = Mouse.GetState().X - oldX;
            float dY = Mouse.GetState().Y - oldY;

            Pitch += -MouseSensity * dY;
            Yaw += -MouseSensity * dX;

            Pitch = MathHelper.Clamp(Pitch, -1.5f, 1.5f);


            Matrix rotation = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw);
            Vector3 transformedVector = Vector3.Transform(REFERENCEVECTOR, rotation);
            Direction = CameraPosition + transformedVector;
            View = Matrix.CreateLookAt(CameraPosition, Direction, Vector3.Up);
            try
            {
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            }
            catch { }
            oldX = GraphicsDevice.Viewport.Width / 2;
            oldY = GraphicsDevice.Viewport.Height / 2;

            BoundingFrustum = new BoundingFrustum(View * Projection);

            //CameraPosition.Y = LODOrigin.Simplex.GetNoise2D(CameraPosition.X, CameraPosition.Z) + 0.25f;
        }

        public static void Move(Vector3 v)
        {
            
            Matrix rotation =  Matrix.CreateRotationY(Yaw);
            Vector3 transformed = Vector3.Transform(v, rotation);

            Vector3 t = transformed * dd(CameraPosition.Y) * Velocity;

            if (v.Y == -1)
            {
                if ((CameraPosition + t).Y >= LODOrigin.Simplex.GetNoise2D(CameraPosition.X, CameraPosition.Z) + 0.25f)
                    CameraPosition.Y += t.Y;

            }
            else CameraPosition.Y += t.Y;

            CameraPosition.X += t.X;
            CameraPosition.Z += t.Z;

        }

        static float dd(float y)
        {
            double r =  Math.Exp((1.0 / 100.0) * (y - 100)) - 0.368;
            return MathHelper.Clamp((float)r, 0.0075f, 2);
        }
    }   
}
