using System;
using System.Numerics;

namespace CoinClicker
{
    public class EnemyManager
    {
        private List<Enemy> enemies;
        float maxSpeed;

        public List<Enemy> Enemies { get => enemies; set => enemies = value; }

        public EnemyManager(float maxSpeed = 2f) { 
            enemies = new List<Enemy>();
            this.maxSpeed = maxSpeed;
        }

        public void Update(Vector2 target, float deltaTime) {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(target, deltaTime);
            }
        }

        public void AddEnemy(Vector2 position)
        {
            enemies.Add(new Enemy(position, maxSpeed));
        }
    }
}
