namespace ProductionExamples;

public static class Sync
{
    private static readonly SemaphoreSlim _sem = new(1);

    /// <summary>
    /// Global syncronization object among all formula types.
    /// Uses in Refresh ALL usecase. Syncronized different refresh algorighms
    /// </summary>
    public static IDisposable LockUpdate() => UpdateSemapthore.Wait();

    /// <summary>
    /// Global syncronization object among all formula types. (Async version)
    /// Uses in Refresh ALL usecase. Syncronized different refresh algorighms
    /// </summary>
    public static Task<IDisposable> LockUpdateAsync() => UpdateSemapthore.WaitAsync();

    private sealed class UpdateSemapthore : IDisposable
    {
        public static async Task<IDisposable> WaitAsync()
        {
            await _sem.WaitAsync().ConfigureAwait(false);
            return new UpdateSemapthore();
        }

        public static IDisposable Wait()
        {
            _sem.Wait();
            return new UpdateSemapthore();
        }

        public void Dispose() => _sem.Release();
    }
}
