
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Linq.Expressions;

namespace Warewolf.Scheduler.Service
{
    public interface IRecurringJobFacade
    {
        void AddOrUpdate(Expression<Action> methodCall, Func<string> cronExpression);

        //  Mimic other methods from RecurringJob that you are going to use.
        // ...
    }
    public class RecurringJobFacade : IRecurringJobFacade
    {
        public void AddOrUpdate(Expression<Action> methodCall, Func<string> cronExpression)
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression);
        }
    }
}