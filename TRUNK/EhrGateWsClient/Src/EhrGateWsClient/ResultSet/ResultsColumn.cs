using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEhr.EhrGate.WsClient.ResultSet
{
    public class ResultsColumn : System.Data.DataColumn
    {
        private ResultsColumn(string name, string path, Type type)
            : base(name)
        {
            this.path = path;

            if (type != null)
                this.DataType = type;
            else
                this.DataType = typeof(object);
        }

        internal ResultsColumn(string name, string path)
            : base(name)
        {
            this.path = path;
            this.DataType = typeof(object);
        }

        private string path;

        public string Path
        {
            get { return this.path; }
        }
    }
}
