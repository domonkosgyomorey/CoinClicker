using System.Numerics;

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

    public class ParticleSystem<T>
    {
        public List<Particle<T>> Particles { get; set; }
        public Vector2 StartPosition { get; set; }
        public int Lives { get; set; }
        private Stack<int> removableIndices;

        public ParticleSystem(Vector2 startPosition, int lives)
        {
            Particles = new();
            removableIndices = new();

            StartPosition = startPosition;
            Lives = lives;
        }

        public void AddInstance(T obj, bool radialRandom)
        {
            Particles.Add(new Particle<T>(obj, Lives, StartPosition, radialRandom));
        }

        public void Update()
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particle<T> particle = Particles[i];
                if (particle.Lives == 0)
                {
                    removableIndices.Push(i);
                }
                else
                {
                    particle.Update();
                }
            }

            while (removableIndices.Count > 0)
            {
                Particles.RemoveAt(removableIndices.Pop());
            }
        }
    }
}
