using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.IO
{
    public partial class FileList<TValue>
    {


        public DateTime _UpdateTime = DateTime.MinValue;
        public DateTime _SaveTime = DateTime.MinValue;
        public DateTime UpdateTime { get { return _UpdateTime; } private set { _UpdateTime = value; } }
        public DateTime SaveTime { get { return _SaveTime; } private set { _SaveTime = value; } }

        public bool IsSaved
        {
            get
            {
                return this.UpdateTime > this.SaveTime;
            }
        }

        ThreadedLocker Locker = new ThreadedLocker(10);
        public List<TValue> Data { get; private set; }
        public string FullPath { get; private set; }
        public string FullDirectory { get; private set; }
        public int SaveInterval { get; private set; }

        ThreadedTimers Timers = new ThreadedTimers(10);
    }
}
