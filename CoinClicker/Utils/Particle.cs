using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoinClicker
{
    public class Particle<T>
    {
        public T Item { get; set; }
        public int Lives { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public Particle(T obj, int lives, Vector2 position, bool radialRandom)
        {
            Item = obj;
            Lives = lives;
            Position = position;
            if (radialRandom)
            {
                Velocity = new Vector2((float)Utility.random.NextDouble() * 2 - 1, (float)Utility.random.NextDouble() * 2 - 1);
            }
            else
            {
                Velocity = new Vector2(0, (float)Utility.random.NextDouble() * 20);
                Position = new Vector2((float)Utility.random.NextDouble() * 100 - 50, position.Y);
            }

        }

        public void Update()
        {
            Lives -= 1;
            Position += Velocity;
        }
    }
}
