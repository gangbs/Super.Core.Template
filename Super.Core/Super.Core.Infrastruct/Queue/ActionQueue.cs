using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Super.Core.Infrastruct.Queue
{
    public class ActionQueue : IDisposable
    {
        readonly ConcurrentQueue<Action> _queue;
        readonly Thread _thread;
        private bool isRunning;

        public ActionQueue()
        {
            this._queue = new ConcurrentQueue<Action>();
            this._thread = new Thread(this.Run);
        }

        public void Start()
        {
            this.isRunning = true;
            this._thread.Start();
        }

        public void Add(Action action)
        {
            this._queue.Enqueue(action);
        }

        private void Run()
        {
            while (this.isRunning)
            {
                Action action;
                if (this._queue.TryDequeue(out action))
                {
                    action();
                    Thread.Sleep(1000);
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }


        public void Dispose()
        {
            this.isRunning = false;
            this._thread.Join();
        }

    }
}
