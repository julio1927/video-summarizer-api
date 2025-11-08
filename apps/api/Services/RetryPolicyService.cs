using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace VideoSummarizer.Api.Services;

internal interface IRetryPolicyService
{
    AsyncRetryPolicy GetDatabaseRetryPolicy();
    AsyncRetryPolicy GetFileOperationRetryPolicy();
}

internal class RetryPolicyService : IRetryPolicyService
{
    private readonly ILogger<RetryPolicyService> _logger;

    public RetryPolicyService(ILogger<RetryPolicyService> logger)
    {
        _logger = logger;
    }

    public AsyncRetryPolicy GetDatabaseRetryPolicy()
    {
        return Policy
            .Handle<Microsoft.Data.Sqlite.SqliteException>()
            .Or<Microsoft.EntityFrameworkCore.DbUpdateException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Database operation failed. Retrying in {Delay}s. Attempt {RetryCount}/3",
                        timeSpan.TotalSeconds,
                        retryCount);
                });
    }

    public AsyncRetryPolicy GetFileOperationRetryPolicy()
    {
        return Policy
            .Handle<IOException>()
            .Or<UnauthorizedAccessException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        exception,
                        "File operation failed. Retrying in {Delay}s. Attempt {RetryCount}/3",
                        timeSpan.TotalSeconds,
                        retryCount);
                });
    }
}


