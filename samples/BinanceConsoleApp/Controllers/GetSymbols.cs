﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Binance;

namespace BinanceConsoleApp.Controllers
{
    internal class GetSymbols : IHandleCommand
    {
        public async Task<bool> HandleAsync(string command, CancellationToken token = default)
        {
            if (!command.StartsWith("symbols ", StringComparison.OrdinalIgnoreCase) &&
                !command.Equals("symbols", StringComparison.OrdinalIgnoreCase) &&
                !command.StartsWith("pairs ", StringComparison.OrdinalIgnoreCase) &&
                !command.Equals("pairs", StringComparison.OrdinalIgnoreCase))
                return false;

            var args = command.Split(' ');

            if (args.Length > 1 && args[1].Equals("refresh", StringComparison.OrdinalIgnoreCase))
            {
                var _symbols = await Program.Api.GetSymbolsAsync(token);

                Symbol.UpdateCache(_symbols);
                Asset.UpdateCache(_symbols);
            }

            var symbols = Symbol.Cache.Values;
            //var symbols = await Program.Api.SymbolsAsync(token); // as string.

            lock (Program.ConsoleSync)
            {
                Console.WriteLine();
                Console.WriteLine(string.Join(", ", symbols));
                Console.WriteLine();
            }

            return true;
        }
    }
}
