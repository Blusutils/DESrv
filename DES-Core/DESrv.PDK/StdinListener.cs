using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESrv.PDK {
    public class StdinListener {
        public static StdinListener Instance { get; private set; } = new StdinListener();
        public delegate void StdinListenerDelegate(string data);
        public event StdinListenerDelegate? OnStdinDataEvent;
        StdinListener() { }
    }
}
