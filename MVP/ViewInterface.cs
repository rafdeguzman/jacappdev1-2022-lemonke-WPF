using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    public interface ViewInterface
    {
        public void ShowBudgetItems(List<BudgetItem> budgetItems);
        public bool ShowFirstTimeMessage();
        public void ShowFilesCreated(string path);
    }
}