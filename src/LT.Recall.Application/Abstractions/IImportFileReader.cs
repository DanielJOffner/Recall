using LT.Recall.Domain.Entities;

namespace LT.Recall.Application.Abstractions
{
    public interface IImportFileReader
    {
        public List<Command> Read(string filePath);
    }
}
