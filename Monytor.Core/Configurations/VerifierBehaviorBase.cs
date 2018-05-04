using Monytor.Core.Models;
using Monytor.Core.Repositories;

namespace Monytor.Core.Configurations {
    public abstract class VerifierBehaviorBase : Behavior {
        public ISerieRepository SerieRepository { get; set; }
        public abstract VerifyResult Verify(Verifier verifier, Serie serie);
    }
}
