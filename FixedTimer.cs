using Microsoft.Xna.Framework;

namespace Asteroids
{
    public class FixedTimer
    {
        public bool Tick { get; set; }
        public int TickTime { get; set; }
        private double _elapsedTime = 0;
        private bool _enabled;
        
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public FixedTimer(int milliseconds, bool enabled = true)
        {
            TickTime = milliseconds;
            Enabled = enabled;
        }

        public FixedTimer()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            Tick = false;

            if (!_enabled)
            {
                _elapsedTime = 0;
                return;
            }

            _elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTime >= TickTime)
            {
                Tick = true;
                _elapsedTime = 0;
            }
        }

        public void ResetElapsedTime()
        {
            _elapsedTime = 0;
        }
    }
}