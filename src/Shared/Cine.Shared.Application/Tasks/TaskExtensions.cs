using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Cine.Shared.Application.Tasks;

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

    public static void Forget(this Task task, ILogger logger)
    {
        if (!task.IsCompleted || task.IsFaulted)
        {
            _ = ForgetAwaited(task, logger);
        }

        return;

        static async Task ForgetAwaited(Task task, ILogger logger)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while executing fire-and-forget task.");
            }
        }
    }
}