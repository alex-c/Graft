﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Graft.Labs.Utilities
{
    public class PrettyPrinter
    {
        private ILogger Logger { get; }

        public PrettyPrinter(ILogger logger)
        {
            Logger = logger;
        }

        public TableBuilder BuildTable(IEnumerable<string> columns, string title = null)
        {
            return new TableBuilder(columns, Logger, title);
        }
    }
}
