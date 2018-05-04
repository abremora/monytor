using Monytor.Core.Configurations;
using Monytor.Core.Models;
using System;
using System.Linq;

namespace Monytor.Implementation.Verifiers {
    public class SerieChangeVerifierBehavior : VerfiyBehavior<SerieChangeVerifier> {
        public override VerifyResult Verify(Verifier verifier, Serie serie) {
            var typedVerifier = verifier as SerieChangeVerifier;
            if(typedVerifier == null)
                return new VerifyResult { Successful = false };
            if (verifier.Notifications == null || !verifier.Notifications.Any())
                return new VerifyResult { Successful = false };

            var query = new SerieQuery {
                Group = typedVerifier.Group,
                Tag = typedVerifier.Tag,
                End = serie.Time.Subtract(typedVerifier.TimeInterval),
                Start = DateTime.MinValue,
                OrderBy = Ordering.Descanding,
                MaxValues = 1
            };
            var serieResult = SerieRepository.GetSeries(query)
                .FirstOrDefault();

            return new VerifyResult {
                Successful = serie.Value == serie.Value,
                NotificationShortDescription = $"Series '{typedVerifier.Group}:{typedVerifier.Tag}' unchange",
                NotificationLongDescription = $"Series '{serie.Id}' with '{typedVerifier.Group}:{typedVerifier.Tag}:{serie.Value}' is unchange at least {typedVerifier.TimeInterval}"
            };
        }
    }
}
