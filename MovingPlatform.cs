// Gehört zu Johannes bzw. (nur AddHelper) Arvid

using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Info2021 {
    [DataContract]
    class MovingPlatform : CinematicObject, ILevelElement {
        [DataMember]
        public VelPos VelPos { get; set; }
        public StaticCollider Collider;
        public override Vector2 Position { get { return VelPos.P; } set { VelPos = VelPos.WithPosition(value); } }

        [DataMember]
        Vector2 translationOnNextFrame = Vector2.Zero;

        [DataMember]
        public float MaxTime;
        [DataMember]
        private float movingTime;


        public MovingPlatform(Vector2 position, Vector2 velocity, float maxTime) {
            VelPos = new VelPos(velocity, position);
            Collider = new StaticCollider(VelPos.P, VelPos.P + Vector2.One * 16);
            CCollider = new CinematicCollider(this, VelPos.P - Vector2.UnitY * 2, new Vector2(16, 8));

            MaxTime = maxTime;
            movingTime = 0;
        }

        public override Texture2D GetTexture(ResourceAccessor resourceAccessor) {
            return resourceAccessor.GetSprite(0, 0);
        }

        public override void Update(float dt, Player player) {
            // move player
            player.VelPos = player.VelPos.Translate(translationOnNextFrame);
            // reset value
            translationOnNextFrame = Vector2.Zero;
            // if stoppingtime is reached reverse Velosity of Platform
            if (movingTime > MaxTime) {
                movingTime = 0;
                VelPos = VelPos.WithVelocity(-VelPos.V);
            }
            // move platform by translating by v * dt
            VelPos = VelPos.ApplyVelocity(dt);
            // Update hitbox according to the priviously updated VelPos
            Collider.TopLeft = Position;
            Collider.BottomRight = Position + Vector2.One * 16;
            CCollider.TopLeft = VelPos.P - Vector2.UnitY * 2;
            // update movingTime
            movingTime = movingTime + dt;
        }

        public override void OnCollision(Player player) {
            // move player with the platform
            // v * 1/60 = dx
            translationOnNextFrame = VelPos.V / 60; 

        }

        public override void AddHelper(Level level) {
            Collider = new StaticCollider(VelPos.P, VelPos.P + Vector2.One * 16);
            level.cinematicObjects.Add(this);
            level.staticColliders.Add(Collider);
        }
    }
}