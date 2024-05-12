using System;
using System.Numerics;

namespace CoinClicker
{
    public class Enemy
    {
        private float maxSpeed;
        private Vector2 position;
        private Vector2 velocity;
        private float initialSpeed;

        public Enemy(Vector2 startPosition, float maxSpeed = 2f, float initialSpeed = 2f)
        {
            position = startPosition;
            this.maxSpeed = maxSpeed;
            this.initialSpeed = initialSpeed;
        }

        public Vector2 Position { get => position; set => position = value; }

        public void Update(Vector2 target, float deltaTime)
        {
            Vector2 direction = target - position;
            velocity += Vector2.Normalize(direction)*initialSpeed*deltaTime;

            if (velocity.Length() > maxSpeed)
            {
                velocity = Vector2.Normalize(velocity);
                velocity *= maxSpeed;
            }

            position += velocity;
        }
    }
}
