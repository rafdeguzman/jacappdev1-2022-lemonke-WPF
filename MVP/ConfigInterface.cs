using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public interface ConfigInterface
    {
        string getDirectory { get; }
        string getFileName { get; }
        string lastUsedFilePath { get; }
        string currentFile { get; }

        string firstTimeUser { get; }

        void saveConfig();
    }
}
