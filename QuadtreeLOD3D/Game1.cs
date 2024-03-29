﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace QuadtreeLOD3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RasterizerState rs;

        private LODOrigin lodOrigin;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            

            base.Initialize();
        }

        public static Texture2D rTexture2D;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>  
        protected override void LoadContent()
        {
            rTexture2D = Content.Load<Texture2D>("texture");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            lodOrigin = new LODOrigin(GraphicsDevice, 1500, 1, 1500);
            
    
            new Camera(GraphicsDevice, 0.007f, 5);

            rs = new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.WireFrame,
                ScissorTestEnable = true,
                MultiSampleAntiAlias = true,
            };



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!IsActive) return;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                Camera.Move(new Vector3(0, 0, -1));

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                Camera.Move(new Vector3(0, 0, 1));

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                Camera.Move(new Vector3(-1, 0, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                Camera.Move(new Vector3(1, 0, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                Camera.Move(new Vector3(0, 1, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                Camera.Move(new Vector3(0, -1, 0));

            lodOrigin.Move(Camera.CameraPosition.X, Camera.CameraPosition.Y, Camera.CameraPosition.Z);
            lodOrigin.Update();

            Camera.Update();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RasterizerState = rs;

            lodOrigin.Draw();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
