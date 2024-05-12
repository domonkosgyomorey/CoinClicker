using System;
using System.Numerics;

namespace CoinClicker
{
    public class Particle<T>
    {
        public enum MovementType
        {
            STATIC,
            RADIAL,
            LINEAR
        }

        public T Item { get; set; }
        public int Lives { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public Particle(T obj, int lives, Vector2 position, MovementType movementType)
        {
            Item = obj;
            Lives = lives;
            Position = position;
            switch (movementType)
            {
                case MovementType.STATIC:
                    Velocity = Vector2.Zero; break;
                case MovementType.RADIAL:
                    Velocity = new Vector2((float)Utility.random.NextDouble() * 2 - 1, (float)Utility.random.NextDouble() * 2 - 1);
                    break;
                case MovementType.LINEAR:
                    Velocity = new Vector2(0, (float)Utility.random.NextDouble() * 20);
                    Position = new Vector2((float)Utility.random.NextDouble() * 100 - 50, position.Y);
                    break;
            }
        }

        public void Update()
        {
            Lives -= 1;
            Position += Velocity;
        }
    }
}
