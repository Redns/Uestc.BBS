namespace Uestc.BBS.Core.Helpers
{
    public static class TaskHelper
    {
        /// <summary>
        /// 等待指定 Task 完成，若在 <paramref name="timeout"/> 内未完成则自动取消并抛出 TimeoutException。
        /// </summary>
        /// <typeparam name="TResult">Task 返回值类型。</typeparam>
        /// <param name="task">要等待的任务。</param>
        /// <param name="timeout">超时时间。</param>
        /// <param name="cancellationToken">外部取消令牌（可选）。</param>
        /// <returns>Task 的结果。</returns>
        /// <exception cref="TimeoutException">超时后抛出。</exception>
        public static async Task<TResult> TimeoutCancelAsync<TResult>(
            this Task<TResult> task,
            TimeSpan timeout,
            CancellationToken cancellationToken = default
        )
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return await task;
            }

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            try
            {
                var completedTask = await Task.WhenAny(
                        task,
                        Task.Delay(Timeout.Infinite, cts.Token)
                    )
                    .ConfigureAwait(false);

                if (completedTask == task)
                {
                    return await task.ConfigureAwait(false);
                }

                // 超时分支
                throw new TimeoutException(
                    $"The operation timed out after {timeout.TotalMilliseconds} ms."
                );
            }
            catch (OperationCanceledException)
                when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                // 仅因为内部超时而被取消
                throw new TimeoutException(
                    $"The operation timed out after {timeout.TotalMilliseconds} ms."
                );
            }
        }

        /// <summary>
        /// 针对无返回值的 Task 版本。
        /// </summary>
        public static Task TimeoutCancelAsync(
            this Task task,
            TimeSpan timeout,
            CancellationToken cancellationToken = default
        ) =>
            task is Task<object> tObj
                ? tObj.TimeoutCancelAsync(timeout, cancellationToken)
                : task.ContinueWith(t => t, cancellationToken)
                    .TimeoutCancelAsync(timeout, cancellationToken);
    }
}
