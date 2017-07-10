using Development.Core.Core.Interface.Managers;
using Development.Core.Interface;
using Development.Core.Interface.Managers;
using Development.Core.User.Interface;

namespace Development.Core.Core.Interface
{
    public interface IDevelopmentManager
    {
        IUser User { get; set; }

        ICommonManager CommonManager { get; }
       
        IEventManager EventManager { get; }
        IPluginManager PluginManager { get; }

        ITransaction GetTransaction();
        ITransaction GetTransaction(bool create);

        bool PowerMode { get; set; }
    }
}
