using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    public interface ConfigInterface
    {
        static string getDirectory { get; set; }
        static string getFileName { get; set; }
        static string lastUsedFilePath { get; set; }
        static string currentFile { get; set; }
        static string firstTimeUser { get; set; }
        static string themeColor { get; set; }
        static string darkMode { get; set; }

    }
}
