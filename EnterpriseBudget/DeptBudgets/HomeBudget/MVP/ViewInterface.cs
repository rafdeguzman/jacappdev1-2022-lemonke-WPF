using Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBudget.DeptBudgets.HomeBudget
{
    public interface ViewInterface
    {
        public bool ShowFirstTimeMessage();
        public void ShowFilesCreated(string path);
        public void ShowBudgetItems(List<BudgetItem> budgetItemsList);
        void ShowBudgetItemsByMonth(List<BudgetItemsByMonth> budgetItemsListByMonth);
        void ShowBudgetItemsByCategory(List<BudgetItemsByCategory> budgetItemsListByCategory);
        void ShowBudgetItemsMonthAndCategory(List<Dictionary<string, object>> budgetItemsListByMonthAndCategory);

        DateTime? GetStartDate();
        DateTime? GetEndDate();
        bool GetFilterFlag();
        int GetCategoryId();

    }
}