namespace Monytor.Core.Repositories {
    public interface ISession {
        T Load<T>(string id);
        void Store<T>(T item);
    }
}