using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Surrogate.Model.Handler
{
    public interface IProcessHandler
    {
        event EventHandler<BackgroundWorker> ProcessAdded;
        event EventHandler<BackgroundWorker> ProcessStarted;
        event EventHandler<BackgroundWorker> ProcessRemoved;

        /// <summary>
        /// Adds a Backgroundworker instance as process to the handler
        /// </summary>
        /// <param name="name"> the name of the process. Can be used to identify the process</param>
        /// <param name="worker">the background worker</param>
        void AddProcess(string name, BackgroundWorker worker);

        /// <summary>
        /// Adds a Backgroundworker instance as process to the handler
        /// </summary>
        /// <param name="worker">the Backgroundworker instance</param>
        void AddProcess(BackgroundWorker worker);

        /// <summary>
        /// Removes a Backgorundworker instance from the handler
        /// </summary>
        /// <param name="name">the name of the worker</param>
        void RemoveProcess(string name);
        void RemoveProcess(BackgroundWorker worker);

        /// <summary>
        /// Checks if the process is currently running.
        /// </summary>
        /// <param name="name">the name identifying the process</param>
        /// <returns>true: if the process is running, false: otherwise</returns>
        bool IsProcessRunning(string name);
        bool IsProcessRunning(BackgroundWorker worker);


        /// <summary>
        /// Starts the process.
        /// </summary>
        /// <param name="name">The name identifying the process</param>
        void StartProcess(string name);
        void StartProcess(BackgroundWorker worker);

        /// <summary>
        /// Stops the process.
        /// </summary>
        /// <param name="name">The name identifying the process</param>
        void EndProcess(string name);
        void EndProcess(BackgroundWorker worker);

        /// <summary>
        /// Starts all processes of this handler
        /// </summary>
        void StartAllProcesses();

        /// <summary>
        /// Stops all processes of this handler
        /// </summary>
        void EndAllProcesses();
    }
}
