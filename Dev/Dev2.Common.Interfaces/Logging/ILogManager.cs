using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev2.Common.Interfaces.Logging
{
    public interface ILogManager
    {
        bool SaveLoggingPath(string auditsFilePath);
    }
}
