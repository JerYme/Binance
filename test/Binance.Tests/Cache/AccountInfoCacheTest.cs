﻿using Binance.Api;
using Binance.Api.WebSocket;
using Binance.Cache;
using Moq;
using System;
using Xunit;

namespace Binance.Tests.Cache
{
    public class AccountInfoCacheTest
    {
        [Fact]
        public void SubscribeThrows()
        {
            var api = new Mock<IBinanceApi>().Object;
            var client = new Mock<IUserDataWebSocketClient>().Object;

            var cache = new AccountInfoCache(api, client);

            Assert.ThrowsAsync<ArgumentNullException>("user", () => cache.SubscribeAsync(null));
        }
    }
}
