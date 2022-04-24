using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace HomeBudgetWPF
{
    public interface ConfigInterface
    {
        static string getDirectory { get; set; }
        static string getFileName { get; set; }
        static string lastUsedFilePath { get; set; }
        static string currentFile { get; set; }
        static string firstTimeUser { get; set; }
    }
}
