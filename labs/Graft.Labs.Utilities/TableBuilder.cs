using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Labs.Utilities
{
    public class TableBuilder
    {
        private HashSet<string> Header { get; }

        private HashSet<HashSet<string>> Lines { get; }

        private List<int> ColumnWidths { get; set; }

        private ILogger Logger { get; }

        public TableBuilder(IEnumerable<string> columns, ILogger logger)
        {
            Header = new HashSet<string>(columns);
            Lines = new HashSet<HashSet<string>>();
            ColumnWidths = new List<int>(Header.Select(c => c.Length));
            Logger = logger;
        }

        public TableBuilder AddLine(IEnumerable<string> cellValues)
        {
            if (cellValues.Count() == Header.Count)
            {
                Lines.Add(new HashSet<string>(cellValues));
                ColumnWidths = UpdateColumnWidths(cellValues);
                return this;
            }
            else
            {
                throw new InvalidOperationException("A table line needs to have as many cell values as there are columns.");
            }
        }

        public void Print()
        {
            // Prepare line separator
            string lineSeparator = "+";
            string headerLine = "+";
            foreach (int columnWidth in ColumnWidths)
            {
                lineSeparator += new string('-', columnWidth + 2);
                headerLine += new string('=', columnWidth + 2);
                lineSeparator += "+";
                headerLine += "+";
            }

            // Prepare line template
            string lineTemplate = "| ";
            for (int i = 0; i < Header.Count; i++)
            {
                lineTemplate += "{" + i + "} | ";
            }

            // Print header
            Logger.LogInformation(lineSeparator);
            Logger.LogInformation(string.Format(lineTemplate, PadCellValues(Header, ColumnWidths)));
            Logger.LogInformation(headerLine);

            // Print lines
            foreach (HashSet<string> line in Lines)
            {
                Logger.LogInformation(lineTemplate, PadCellValues(line, ColumnWidths));
                Logger.LogInformation(lineSeparator);
            }
        }

        private List<int> UpdateColumnWidths(IEnumerable<string> newCellValues)
        {
            List<int> newColumnWidths = new List<int>();
            for (int i = 0; i < Header.Count; i++)
            {
                if (newCellValues.ElementAt(i).Length > ColumnWidths.ElementAt(i))
                {
                    newColumnWidths.Add(newCellValues.ElementAt(i).Length);
                }
                else
                {
                    newColumnWidths.Add(ColumnWidths.ElementAt(i));
                }
            }
            return newColumnWidths;
        }

        private string[] PadCellValues(IEnumerable<string> cellValues, IEnumerable<int> cellWidths)
        {
            string[] result = new string[cellValues.Count()];
            for (int i = 0; i < cellValues.Count(); i++)
            {
                result[i] = cellValues.ElementAt(i).PadRight(cellWidths.ElementAt(i));
            }
            return result;
        }
    }
}
