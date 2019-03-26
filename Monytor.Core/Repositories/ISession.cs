namespace Monytor.Core.Repositories {

    public interface ISession {
        T Load<T>(string id);
        T Load<T>(int id);
        void Store<T>(T item);
    }
}