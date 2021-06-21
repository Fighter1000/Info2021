﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Info2021
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Player player;
        private List<Tile> tiles = new List<Tile>();
        private List<IUpdateable> updateables = new List<IUpdateable>();
        TileRenderer tileRenderer;
        BackgroundRenderer backgroundRenderer;
        DynamicRenderer playerRenderer;
        ResourceAccessor resourceAccessor;
        private Vector2 camPos = Vector2.Zero;
        private Background background;
        private List<StaticCollider> staticColliders = new List<StaticCollider>();
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            resourceAccessor = new ResourceAccessor(this);
        }

       
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            updateables.Add(player);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();
            // TODO: Remove test.
            
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tileRenderer = new TileRenderer(_spriteBatch);
            backgroundRenderer = new BackgroundRenderer(_spriteBatch);
            background = new Background(Content.Load<Texture2D>("640x360"));
            playerRenderer = new DynamicRenderer(_spriteBatch);
            staticColliders.Add(new StaticCollider(new Vector2(0, 15*16), new Vector2(450, 16*16)));
            staticColliders.Add(new StaticCollider(new Vector2(15*16, 0), new Vector2(17*16, 450)));
            for(int i = 0; i < 30; i++) {
                tiles.Add(new Tile(new TileInfo(this, "Character"), i, 15));
                
            }
             for(int i = 0; i < 30; i++) {
                tiles.Add(new Tile(new TileInfo(this, "Character"), 15, i));
                
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.IsActive(InputEvent.Escape))
                Exit();

           
            // Then resolve collisions as suggested in https://spicyyoghurt.com/tutorials/html5-javascript-game-development/collision-detection-physics
            
             
            if(player.Position.X - camPos.X > 640) {
                
                camPos.X += 640;
            }
            if(player.Position.X - camPos.X < -16) {
                
                camPos.X -= 640;
            }
            if(player.Position.Y - camPos.Y > 376) {
                
                camPos.Y += 360;
            }
            if(player.Position.Y - camPos.Y < -16) {
                
                camPos.Y -= 360;
            }
            player.VelPos = player.VelPos.ApplyVelocity((float)gameTime.ElapsedGameTime.TotalSeconds);
               for (int i = 0; i < updateables.Count; i++) {
                // Do normal physics...
                updateables[i].Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            // TODO: Generalize to multiple dynamic colliders and possibly even dynamic collider - dynamic collider collision.
            for (int i = 0; i < staticColliders.Count; i++) {
                player.Collider.CollideWith(staticColliders[i]);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
            foreach(Tile tile in tiles) {
                tile.Draw(tileRenderer, resourceAccessor, camPos);
            }
            
            background.Draw(backgroundRenderer, resourceAccessor, camPos);
            player.Draw(playerRenderer, resourceAccessor, camPos);
            
            _spriteBatch.End();
            base.Draw(gameTime);
            
        }


    }
}
