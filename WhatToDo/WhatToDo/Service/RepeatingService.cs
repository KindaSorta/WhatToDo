
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToDo.Service
{
    public class RepeatingService 
    {
        private Task? _timerTask;
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts = new();

        public RepeatingService(TimeSpan interval)
        {
            _timer = new PeriodicTimer(interval);
        }


        public void Start()
        {
            _timerTask = DoWorkAsync();
        }

        private async Task DoWorkAsync()
        {
            try
            {
                
            }
            catch (OperationCanceledException) 
            {

            }

        }

        public async Task StopAsync()
        {
            if (_timerTask is null)
            {
                return;
            }

            _cts.Cancel();
            await _timerTask;
            _cts.Dispose();
        }

    }
}
