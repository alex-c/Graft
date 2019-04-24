using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graft.Labs.Utilities
{
    public class TableBuilder
    {
        private string Title { get; }

        private HashSet<string> Header { get; }

        private HashSet<HashSet<string>> Lines { get; }

        private List<int> ColumnWidths { get; set; }

        private ILogger Logger { get; }

        public TableBuilder(IEnumerable<string> columns, ILogger logger, string title = null)
        {
            Header = new HashSet<string>(columns);
            Lines = new HashSet<HashSet<string>>();
            ColumnWidths = new List<int>(Header.Select(c => c.Length));
            Logger = logger;
            Title = title;
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
            // Prepare line separators
            string titleLine = "+";
            string lineSeparator = "+";
            string headerLine = "+";
            foreach (int columnWidth in ColumnWidths)
            {
                titleLine += new string('-', columnWidth + 2);
                titleLine += "-";
                headerLine += new string('=', columnWidth + 2);
                headerLine += "+";
                lineSeparator += new string('-', columnWidth + 2);
                lineSeparator += "+";
            }
            titleLine = titleLine.Remove(titleLine.Length - 1, 1) + "+";

            // Prepare line template
            string lineTemplate = "| ";
            for (int i = 0; i < Header.Count; i++)
            {
                lineTemplate += "{" + i + "} | ";
            }
            
            // Print table title if needed
            if (Title != null)
            {
                Logger.LogInformation(titleLine);
                Logger.LogInformation(string.Format("| {0} |", Title.PadRight(ColumnWidths.Sum() + ColumnWidths.Count * 2)));
            }

            // Print table header
            Logger.LogInformation(lineSeparator);
            Logger.LogInformation(string.Format(lineTemplate, PadCellValues(Header, ColumnWidths)));
            Logger.LogInformation(headerLine);

            // Print table lines
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
