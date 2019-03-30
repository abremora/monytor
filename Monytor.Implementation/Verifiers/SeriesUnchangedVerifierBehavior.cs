using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Linq;

namespace Monytor.Implementation.Verifiers {
    public class SeriesUnchangedVerifierBehavior : VerifierBehavior<SeriesUnchangedVerifier> {
        public override VerifyResult Verify(Verifier verifier, Series series) {
            var typedVerifier = verifier as SeriesUnchangedVerifier;
            if (typedVerifier == null)
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
                Successful = seriesResult.Value == series.Value,
                NotificationShortDescription = $"Series '{typedVerifier.Group}:{typedVerifier.Tag}' unchanged",
                NotificationLongDescription = $"Series '{series.Id}' with '{typedVerifier.Group}:{typedVerifier.Tag}:{series.Value}' is unchanged within the time interval {typedVerifier.TimeInterval}"
            };
        }
    }
}
