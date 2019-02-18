namespace Movies.Client.Services
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IIntegrationService" />
    /// </summary>
    public interface IIntegrationService
    {
        /// <summary>
        /// The Run
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        Task Run();
    }
}
