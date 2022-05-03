using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudgetWPF
{
    public interface ViewInterface
    {
        public bool ShowFirstTimeMessage();
        public void ShowFilesCreated(string path);
        void ShowBudgetItems(List<BudgetItem> budgetItemsList);
        void ShowBudgetItemsByDate(List<BudgetItemsByMonth> budgetItemsListByMonth);
        void ShowBudgetItemsByCategory(List<BudgetItemsByCategory> budgetItemsListByCategory);
        void ShowBudgetItemsDateAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory);

        DateTime? GetStartDate();
        DateTime? GetEndDate();
        bool GetFilterFlag();
        int GetCategoryId();

    }
}