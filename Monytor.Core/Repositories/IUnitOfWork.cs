using System;

namespace Monytor.Core.Repositories {
    public interface IUnitOfWork : IDisposable {
        ISession Session { get; }
        ISession OpenSession();
        void SaveChanges();
    }
}       