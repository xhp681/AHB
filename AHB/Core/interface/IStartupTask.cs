namespace AHB.Core
{
    public interface IStartupTask
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ExecuteAsync();

        /// <summary>
        /// Gets order of this startup task implementation
        /// </summary>
        int Order { get; }
    }
}