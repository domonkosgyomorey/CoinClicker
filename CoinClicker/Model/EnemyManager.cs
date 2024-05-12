using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoinClicker
{
    public class EnemyManager
    {
        private List<Enemy> enemies;
        Queue<Enemy> removableIdx;
        float maxSpeed;
        float acc;
        int lives;

        public List<Enemy> Enemies { get => enemies; set => enemies = value; }

        public EnemyManager(int lives = 100, float maxSpeed = 5f) { 
            enemies = new List<Enemy>();
            removableIdx = new Queue<Enemy>();
            this.maxSpeed = maxSpeed;
            this.lives = lives;
        }

        public void Update(Vector2 target) {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.Lives == 0)
                {
                    removableIdx.Enqueue(enemy);
                }
                enemy.Update(target);
            }
            while (removableIdx.Count > 0)
            {
                enemies.Remove(removableIdx.Dequeue());
            }
        }

        public void AddEnemy(Vector2 position)
        {
            enemies.Add(new Enemy(position, lives, maxSpeed));
        }
    }
}
