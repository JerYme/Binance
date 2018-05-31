using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Utility
{
    public class TaskController : IDisposable
    {
        #region Public Properties

        public Task Task => Task.WhenAll(_tasks);

        #endregion Public Properties

        #region Private Fields

        private readonly IList<Task> _tasks;
        private readonly CancellationTokenSource _cts;
        private readonly TaskFactory _tf;

        #endregion Private Fields

        #region Constructors

        public TaskController()
        {
            _cts = new CancellationTokenSource();
            _tf = new TaskFactory(_cts.Token);
            _tasks = new List<Task>();
        }

        #endregion Constructors

        #region Public Methods

        public virtual void Begin(Func<CancellationToken, Task> action, Action<Exception> onError = null)
        {
            var t = Task.Run(async () =>
            {
                try { await action(_cts.Token); }
                catch (OperationCanceledException) { }
                catch (Exception e)
                {
                    if (!_cts.IsCancellationRequested)
                    {
                        onError?.Invoke(e);
                        OnError(e);
                    }
                }
            });

            _tasks.Add(t);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void OnError(Exception e) { }

        #endregion Protected Methods

        #region IDisposable

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _cts?.Cancel();
                Task?.GetAwaiter().GetResult();
                _cts?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable
    }
}
