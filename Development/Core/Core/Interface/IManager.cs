using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Development.Core.Core.Interface;

namespace Development.Core.Interface
{
    interface IManager
    {
        void Initialize(IDevelopmentManager DevelopmentManager);

        void CommitCaches();
        void RollbackCaches();
    }
}
