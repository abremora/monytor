using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monytor.Core.Repositories {

    public interface ISession {
        T Load<T>(string id);
        T Load<T>(int id);
        IEnumerable<T> LoadAll<T>(int start = 0, int length = 1024);

        void Store<T>(T item);

        void Delete<T>(T item);
        void Delete(string id);
        void Delete(int id);
    }
}