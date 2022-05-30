using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESCore
{
    class CallableList<TRtype> : IDisposable
    {
        public virtual List<Func<TRtype>> Funcs { get; set; } = new List<Func<TRtype>>();
        public event Func<TRtype> Callbacks
        {
            add
            {
                Funcs.Add(value);
            }
            remove
            {
                Funcs.Remove(value);
            }
        }
        /// <summary>
        /// Calls all functions in <see cref="Funcs"/>
        /// </summary>
        /// <returns>Number of sucess calls</returns>
        public extern uint CallAll();
        /*{
            uint successCalls = 0;
            foreach (var func in Funcs)
                try { _=func.Invoke((T1)arg1); successCalls += 1; } catch {  }
            return successCalls;
        }*/
        public void Dispose()
        {
            Funcs.Clear();
        }
    }
}
