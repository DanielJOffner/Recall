using LT.Recall.Domain.Entities;

namespace LT.Recall.Application.Abstractions
{
    public interface IExportFileWriter 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the number of records exported</returns>
        public int Write(string filePath, List<Command> commands);
    }
}
