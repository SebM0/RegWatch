using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegUpdater
{
    abstract class RecurrentTool : IDisposable
    {
        private CancellationTokenSource cancel;
        protected virtual TimeSpan timeout => TimeSpan.FromSeconds(30);

        protected RecurrentTool()
        {
        }

        public void Start()
        {
            cancel = new CancellationTokenSource();
            LaunchRecurrentTask(Run, timeout, cancel.Token);
        }

        protected abstract void Run();

        public void Stop()
        {
            if (cancel == null)
                return;
            cancel.Cancel();
            cancel.Dispose();
            cancel = null;
        }

        public bool IsActive()
        {
            return cancel != null;
        }

        public void Dispose()
        {
            Stop();
        }
        private static void LaunchRecurrentTask(Action action, TimeSpan timeout, CancellationToken token)
        {
            if (action == null)
                return;
            Task.Run(async () => {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(timeout, token);
                }
            }, token);
        }
    }
}
