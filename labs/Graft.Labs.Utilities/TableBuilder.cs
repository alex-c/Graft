using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Labs.Utilities
{
    public class TableBuilder
    {
        private Dictionary<string, HashSet<string>> Cells { get; }

        private ILogger Logger { get; }

        public TableBuilder(IEnumerable<string> columns, ILogger logger)
        {
            foreach (string column in columns)
            {
                Cells.Add(column, new HashSet<string>());
            }
            Logger = logger;
        }

        public TableBuilder AddLine(IEnumerable<string> cellValues)
        {
            int cellCount = cellValues.Count();
            if (cellCount == Cells.Count)
            {
                for (int i = 0; i < cellCount; i++)
                {
                    Cells.ElementAt(i).Value.Add(cellValues.ElementAt(i));
                }
                return this;
            }
            else
            {
                throw new InvalidOperationException($"Expected {Cells.Count} cell values, got {cellCount} instead.");
            }
        }

        public void Print()
        {
            // Compute max cell length per column
            Dictionary<string, int> maxCellWidths = new Dictionary<string, int>();
            foreach (KeyValuePair<string, HashSet<string>> column in Cells)
            {
                maxCellWidths.Add(column.Key, column.Value.Max(content => content.Length));
            }

            // Prepare line separator
            string lineSeparator = "+";
            foreach (int maxCellWidth in maxCellWidths.Values)
            {
                lineSeparator += new string('-', maxCellWidth + 2);
            }

            // Prepare line template
            string lineTemplate = "| ";
            for (int i = 0; i < Cells.Count; i++)
            {
                lineTemplate += "{cell" + i + "} | ";
            }

            // Print header
            Logger.LogInformation(lineSeparator);
            Logger.LogInformation(lineTemplate, Cells.Keys);
            Logger.LogInformation(lineSeparator);
        }
    }
}
