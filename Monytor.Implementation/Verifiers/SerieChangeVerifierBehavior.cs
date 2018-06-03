using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Linq;

namespace Monytor.Implementation.Verifiers {
    public class SerieChangeVerifierBehavior : VerfiyBehavior<SeriesChangeVerifier> {
        public override VerifyResult Verify(Verifier verifier, Series series) {
            var typedVerifier = verifier as SeriesChangeVerifier;
            if(typedVerifier == null)
                return new VerifyResult { Successful = false };
            if (verifier.Notifications == null || !verifier.Notifications.Any())
                return new VerifyResult { Successful = false };

            var query = new SeriesQuery {
                Group = typedVerifier.Group,
                Tag = typedVerifier.Tag,
                End = series.Time.Subtract(typedVerifier.TimeInterval),
                Start = DateTime.MinValue,
                OrderBy = Ordering.Descanding,
                MaxValues = 1
            };
            var serieResult = SeriesRepository.GetSeries(query)
                .FirstOrDefault();

            return new VerifyResult {
                Successful = series.Value == series.Value,
                NotificationShortDescription = $"Series '{typedVerifier.Group}:{typedVerifier.Tag}' unchange",
                NotificationLongDescription = $"Series '{series.Id}' with '{typedVerifier.Group}:{typedVerifier.Tag}:{series.Value}' is unchange at least {typedVerifier.TimeInterval}"
            };
        }
    }
}
