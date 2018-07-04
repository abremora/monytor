using System;

namespace Monytor.Core.Repositories {
    public interface IUnitOfWork : IDisposable {
        ISession OpenSession();
        void SaveChanges();
    }
}       