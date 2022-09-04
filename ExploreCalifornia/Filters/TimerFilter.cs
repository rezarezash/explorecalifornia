using ExploreCalifornia.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ExploreCalifornia.Filters
{
    public class TimerFilter : ActionFilterAttribute
    {
        private readonly TimerManager _timerManager;
        public TimerFilter (TimerManager timerManager)
        {
            _timerManager = timerManager;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_timerManager.IsTimerStarted)
            {
                if (!_timerManager.IsTimerStarted)
                {
                    _timerManager.PrerpareTimer(() =>
                    {
                        Console.WriteLine("Running ... ");
                    });
                }
            }
        }
    }
}
