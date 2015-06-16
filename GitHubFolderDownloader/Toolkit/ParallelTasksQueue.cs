using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubFolderDownloader.Toolkit
{
    public class ParallelTasksQueue : TaskScheduler, IDisposable
    {
        private readonly ApartmentState _apartmentState;
        private readonly ThreadPriority _threadPriority;
        private readonly List<Thread> _threads;
        private bool _disposed;
        private BlockingCollection<Task> _tasks;

        public ParallelTasksQueue(int numberOfThreads, ApartmentState apartmentState, ThreadPriority threadPriority)
        {
            _apartmentState = apartmentState;
            _threadPriority = threadPriority;

            if (numberOfThreads < 1) numberOfThreads = Environment.ProcessorCount;

            _tasks = new BlockingCollection<Task>();

            _threads = Enumerable.Range(0, numberOfThreads).Select(i =>
            {
                var thread = new Thread(() =>
                {
                    foreach (var task in _tasks.GetConsumingEnumerable())
                    {
                        TryExecuteTask(task);
                    }
                }) { IsBackground = true, Priority = _threadPriority };
                thread.SetApartmentState(_apartmentState);
                return thread;
            }).ToList();

            _threads.ForEach(t => t.Start());
        }

        public ParallelTasksQueue(int numberOfThreads)
            : this(numberOfThreads, ApartmentState.MTA, ThreadPriority.Normal) { }


        public override int MaximumConcurrencyLevel
        {
            get { return _threads.Count; }
        }


        public void Dispose()
        {
            Dispose(true);
            // tell the GC that the Finalize process no longer needs to be run for this object.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManagedResources)
        {
            if (_disposed) return;
            if (!disposeManagedResources) return;

            if (_tasks != null)
            {
                _tasks.CompleteAdding();

                foreach (var thread in _threads) thread.Join();

                _tasks.Dispose();
                _tasks = null;
            }

            _disposed = true;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (Thread.CurrentThread.GetApartmentState() != _apartmentState) return false;
            return Thread.CurrentThread.Priority == _threadPriority && TryExecuteTask(task);
        }
    }
}