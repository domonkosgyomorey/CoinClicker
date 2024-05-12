using System;
using System.Linq;
using System.Numerics;

namespace CoinClicker
{
    public class Enemy
    {
        private float maxSpeed;
        private Vector2 position;
        private Vector2 velocity;
        private int lives;
        private float initialSpeed;

        public Enemy(Vector2 startPosition, int maxLives, float maxSpeed = 5f, float initialSpeed = 2f)
        {
            position = startPosition;
            this.maxSpeed = maxSpeed;
            lives = maxLives;
            this.initialSpeed = initialSpeed;
        }

        public int Lives { get => lives; set => lives = value; }
        public Vector2 Position { get => position; set => position = value; }

        public void Update(Vector2 target)
        {
            Vector2 direction = target - position;
            velocity += Vector2.Normalize(direction)*initialSpeed;

            if (velocity.Length() > maxSpeed)
            {
                velocity = Vector2.Normalize(velocity);
                velocity *= maxSpeed;
            }

            position += velocity;

            lives--;
        }
    }
}
