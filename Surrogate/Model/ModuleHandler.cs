

using Surrogate.Model;
using Surrogate.Modules;

namespace Surrogat.Handler
{
	interface ModuleHandler
    {
        void addModule(IModule module);
        IModule removeModule(IModule module);
    }

	interface ConnectionHandler
    {
        void registerConnection(IConnection connection);
    }

	interface ProcessHandler
    {
        void addProcess();
        void removeProcess();
    }

}