namespace LT.Recall.Application.Abstractions
{
    public interface IInstallerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionOrLocation">The location to or name of the collection to install</param>
        /// <returns></returns>
        public IInstaller GetInstaller(string collectionOrLocation);

        public List<IInstaller> ListInstallers();
    }
}
