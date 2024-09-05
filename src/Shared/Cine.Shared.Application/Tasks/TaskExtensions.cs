using System.Runtime.CompilerServices;

namespace Cine.Shared.Application.Tasks
{
    public static class TaskExtensions
    {
        public static TaskAwaiter<(TType, TType)> GetAwaiter<TType>(this (Task<TType>, Task<TType>) tasksTuple)
        {
            async Task<(TType, TType)> CombineTasks()
            {
                var (task1, task2) = tasksTuple;
                await Task.WhenAll(task1, task2);

                return (task1.Result, task2.Result);
            }

            return CombineTasks().GetAwaiter();
        }
    }
}
