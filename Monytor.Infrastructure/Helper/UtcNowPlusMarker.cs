using System;

namespace Monytor.Infrastructure.Helper {
    public class UtcNowPlusMarker : MarkerBase {
        public override string Tag => "DATETIME.UTCNOW+";

        public override Func<string, string> PlaceholderResult
            => (string value) => (DateTime.UtcNow + TimeSpan.Parse(value)).ToString("o");
    }
}
