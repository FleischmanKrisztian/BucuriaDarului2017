using System;
using System.Collections.Generic;

namespace Elm.Core.Parsers
{
    public class Parser : IDisposable
    {
        public List<string> HeaderData
        {
            get;
            set;
        }

        public List<string[]> RowsData
        {
            get;
            set;
        }

        public List<string[]> RawData
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public int GetNumberOfRows()
        {
            return this.RowsData.Count;
        }

        public int GetNumberOfColumns()
        {
            return this.HeaderData.Count;
        }

        public string GetName()
        {
            return this.GetType().Name;
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}