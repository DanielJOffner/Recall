namespace LT.Recall.Application.Abstractions
{
    public interface IInstaller
    {
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionOrLocation">The location to or name of the collection to install</param>
        /// <returns></returns>
        Task<(int inserted, int updated)> Install(string collectionOrLocation);
        

        /// <summary>
        /// List all collections available to install
        /// </summary>
        /// <returns></returns>
        Task<List<string>> ListCollections();
    }
}
