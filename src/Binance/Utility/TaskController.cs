using System;
using System.Threading;
using System.Threading.Tasks;

namespace Binance.Utility
{
    public class TaskController : IDisposable
    {
        #region Public Properties

        public Task Task { get; private set; }

        #endregion Public Properties

        #region Private Fields

        private readonly CancellationTokenSource _cts;
        private readonly TaskFactory _tf;

        #endregion Private Fields

        #region Constructors

        public TaskController()
        {
            _cts = new CancellationTokenSource();
            _tf = new TaskFactory(_cts.Token);
            Task = Task.Delay(0);
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

            Task = _tf.ContinueWhenAll(new[] { Task, t }, (tasks) => { });
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
