using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Api.WebSocket.Events;

namespace Binance.Api.WebSocket
{
    /// <summary>
    /// A web socket client interface for BinanceWebSocketClient.
    /// </summary>
    public interface IWebSocketClient
    {
        /// <summary>
        /// Connect web socket to URI and begin receiving messages.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="token">The cancellation token (required to cancel operation).</param>
        /// <returns></returns>
        Task RunAsync(Uri uri, EventHandler<WebSocketClientMessageEventArgs> message, EventHandler<EventArgs> open = null, EventHandler<EventArgs> close = null, CancellationToken token = default(CancellationToken));
    }
}
