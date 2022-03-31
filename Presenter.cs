using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace HomeBudgetWPF
{
    class Presenter
    {
        //const string DEFAULT_FILENAME = "%homepath%\\BudgetFiles\\budget.db";
        const string DEFAULT_FILENAME = "./budget.db";
        HomeBudget model;
        public Presenter()
        {
            model = new HomeBudget(DEFAULT_FILENAME);
        }

    }
}
