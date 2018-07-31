using Surrogat.Handler;
using Surrogate.Model.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations.Handler
{
    /// <summary>
    /// Implementation of the <see cref="IProcessHandler"/>.
    /// </summary>
    public class ProcessHandler : IProcessHandler
    {
        
        public event EventHandler<BackgroundWorker> ProcessAdded;
        public event EventHandler<BackgroundWorker> ProcessRemoved;
        public event EventHandler<BackgroundWorker> ProcessStarted;
        public event EventHandler<BackgroundWorker> ProcessEnded;

        private Dictionary<string, BackgroundWorker> _workers = new Dictionary<string, BackgroundWorker>();

        public void AddProcess(string name, BackgroundWorker worker)
        {
            if (!worker.WorkerSupportsCancellation)
            {
                throw new ArgumentException("BackgroundWorker added to this handler must support Cancellation");
            }
            _workers.Add(name, worker);
            ProcessAdded?.Invoke(this, worker);
            worker.RunWorkerCompleted += WorkerCompleted;
        }

        /// <summary>
        /// Adds a process with default name to this handler. The default name used by this
        /// <see cref="IProcessHandler"/> implementation is the hash code value of the class
        /// as string. See also: <seealso cref="AddProcess(string, BackgroundWorker)"/>
        /// </summary>
        /// <param name="worker">the process as BackgorundWorker</param>
        public void AddProcess(BackgroundWorker worker)
        {
            
            AddProcess(worker.GetHashCode().ToString(), worker);
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProcessEnded?.Invoke(this, sender as BackgroundWorker);
        }

        public bool IsProcessRunning(string name)
        {
            if (_workers.ContainsKey(name))
            {
                return _workers[name].IsBusy;
            }
            return false;
        }

        public bool IsProcessRunning(BackgroundWorker worker)
        {
            return IsProcessRunning(worker.GetHashCode().ToString());
        }

        public void RemoveProcess(string name)
        {
            if (_workers.ContainsKey(name))
            {
                ProcessRemoved?.Invoke(this, _workers[name]);
                _workers.Remove(name);
            }
        }

        public void RemoveProcess(BackgroundWorker worker)
        {
            RemoveProcess(worker.GetHashCode().ToString());
        }

        public void StartProcess(string name)
        {
            if (_workers.ContainsKey(name) && !_workers[name].IsBusy)
            {
                ProcessStarted?.Invoke(this, _workers[name]);
                _workers[name].RunWorkerAsync();
            }
        }

        public void StartProcess(BackgroundWorker worker)
        {
            StartProcess(worker.GetHashCode().ToString());
        }

        public void StartAllProcesses()
        {
            foreach(var key in _workers.Keys)
            {
                StartProcess(key);
            }
        }

        public void EndProcess(string name)
        {
            if (_workers.ContainsKey(name))
            {
                _workers[name].CancelAsync();
                ProcessEnded?.Invoke(this, _workers[name]);
            }
        }

        public void EndProcess(BackgroundWorker worker)
        {
            EndProcess(worker.GetHashCode().ToString());
        }

        public void EndAllProcesses()
        {
            foreach (var key in _workers.Keys)
            {
                EndProcess(key);
            }
        }
    }
}
