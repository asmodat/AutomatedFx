using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;
using System.Reflection;

using System.Linq.Expressions;

using System.Windows.Forms;

using System.Collections.Concurrent;

using System.Security.Permissions;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedMethod
    {
        public void Terminate(Expression<Action> EAMethod)
        {
            Terminate(Expressions.nameofFull(EAMethod));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void Terminate(string ID)
        {

            if (!TDSThreads.ContainsKey(ID)) return;

            lock (this)
            {
                do
                {

                    while (TDSThreads[ID] != null && TDSThreads[ID].IsAlive)
                    {
                        TDSThreads[ID].Interrupt();
                        TDSThreads[ID].Abort();
                    }

                    TDSThreads[ID] = null;
                    bool removed = TDSThreads.Remove(ID);

                    if (!removed)
                    {

                    }

                } while (TDSThreads.ContainsKey(ID));
            }
        }

        public void TerminateAll()
        {
            if (TDSThreads == null) return;
            string[] saTKeys = TDSThreads.Keys.ToArray();

            foreach (string s in saTKeys)
                this.Terminate(s);
        }
        public bool AllTerminated()
        {
            if (TDSThreads == null) return true;
            string[] saTKeys = TDSThreads.Keys.ToArray();

            foreach (string s in saTKeys)
                if (this.IsAlive(s)) return false;

            return true;
        }


        public bool TerminateAllCompleated()
        {
            string[] keys = TDSThreads.Keys.ToArray();
            foreach (string key in keys)
            {
                if (!IsAlive(key))
                    this.Terminate(key);
            }


            return true;
        }
    }
}
