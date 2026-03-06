namespace Solvedoku.Services.Repository
{
    public interface IRepositoryService<T>
    {
        void Save(T file, string path);

        T Load(string path);
    }
}
