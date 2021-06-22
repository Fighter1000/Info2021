using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Info2021
{
    public static class FirstLevel
    {
        public static Level GetLevel(Game1 game) {
            List<Tile> tiles = new List<Tile>();
            List<StaticCollider> staticColliders = new List<StaticCollider>();
            for(int i = 0; i < 30; i++) {
                tiles.Add(new Tile(new TileInfo(game, "Character"), i, 15));
                
            }
             for(int i = 0; i < 30; i++) {
                tiles.Add(new Tile(new TileInfo(game, "Character"), 15, i));
                
            }
            staticColliders.Add(new StaticCollider(new Vector2(0, 15*16), new Vector2(450, 16*16)));
            staticColliders.Add(new StaticCollider(new Vector2(15*16, 0), new Vector2(17*16, 450)));
            return new Level(Vector2.Zero, Vector2.Zero, tiles, staticColliders, new List<DynamicObject>(),
                new Background(game.resourceAccessor.LoadContent<Texture2D>("background1")));
        }
    }
}