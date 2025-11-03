namespace UsersApi.Abstractions
{
    public interface IMemoryCacheService
    {
        bool TryGet<T>(object key, out T? value);
        void Set<T>(object key, T value);
    }
}
