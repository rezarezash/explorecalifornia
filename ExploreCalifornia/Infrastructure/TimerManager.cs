using System;
using System.Threading;

namespace ExploreCalifornia.Infrastructure
{
    public class TimerManager
    {
        private Action? _action;
        private Timer? _timer;
        private AutoResetEvent? _autoResetEvent;
        public bool IsTimerStarted { get; set; }
        public DateTime TimerStartetAt { get; set; }

        public void PrerpareTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            TimerStartetAt = DateTime.Now;
            _timer = new Timer(Execute, _autoResetEvent, 1000, 2000);
        }

        private void Execute(object? stateInfo)
        {
            _action();
            if ((DateTime.Now - TimerStartetAt).TotalSeconds > 60)
            {
                IsTimerStarted = false;
                _timer.Dispose();
            }
        }
    }
}
