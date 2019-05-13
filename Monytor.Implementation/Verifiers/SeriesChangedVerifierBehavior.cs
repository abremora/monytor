using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Linq;

namespace Monytor.Implementation.Verifiers {
    public class SeriesChangedVerifierBehavior : VerifierBehavior<SeriesChangedVerifier> {
        public override VerifyResult Verify(Verifier verifier, Series series) {
            var typedVerifier = verifier as SeriesChangedVerifier;
            if(typedVerifier == null)
                return new VerifyResult { Successful = false };
            if (verifier.Notifications == null || !verifier.Notifications.Any())
                return new VerifyResult { Successful = false };

            var query = new SeriesQuery {
                Group = typedVerifier.Group,
                Tag = typedVerifier.Tag,
                End = series.Time.Subtract(TimeSpan.FromMilliseconds(1)).Subtract(typedVerifier.TimeInterval),
                Start = DateTime.MinValue,
                OrderBy = Ordering.Descanding,
                MaxValues = 1
            };
            var seriesResult = SeriesRepository.GetSeries(query)
                .FirstOrDefault();

            return new VerifyResult {
                Successful = seriesResult != null && seriesResult.Value != series.Value,
                NotificationShortDescription = $"Series '{typedVerifier.Group}:{typedVerifier.Tag}' changed",
                NotificationLongDescription = $"Series '{series.Id}' with '{typedVerifier.Group}:{typedVerifier.Tag}:{series.Value}' has changed to {seriesResult?.Value} within the time interval {typedVerifier.TimeInterval}"
            };
        }
    }
}
