using System.Windows.Media;

namespace CoinClicker
{
    public class MySoundPlayer
    {
        MediaPlayer player;
        DateTime lastAutoClickTime;
        int minMsBetweenPlay;

        public MySoundPlayer(string soundPath, int minMsBetweenPlay)
        {
            player = new MediaPlayer();
            player.Open(new Uri(soundPath));
            lastAutoClickTime = DateTime.Now;
            this.minMsBetweenPlay = minMsBetweenPlay;
        }

        public void Play(float volume)
        {
            player.Volume = volume;
            if ((DateTime.Now - lastAutoClickTime).TotalMilliseconds >= minMsBetweenPlay)
            {
                lastAutoClickTime = DateTime.Now;
                player.Position = TimeSpan.Zero;
                player.Play();
            }
        }
    }
}
