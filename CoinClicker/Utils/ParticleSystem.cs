using System.Numerics;

namespace CoinClicker
{
    public class ParticleSystem<T>
    {
        public List<Particle<T>> Particles { get; set; }
        public int Lives { get; set; }
        private Stack<int> removableIndices;

        public ParticleSystem(int lives)
        {
            Particles = new();
            removableIndices = new();

            Lives = lives;
        }

        public void AddInstance(T obj, Vector2 position, Particle<T>.MovementType type)
        {
            Particles.Add(new Particle<T>(obj, Lives, position, type));
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
