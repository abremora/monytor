using Monytor.Core.Models;
using Raven.Client.Indexes;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Monytor.RavenDb {
    public class SeriesByHourIndex : AbstractIndexCreationTask<Series, SeriesByDayIndex.Result> {
        public class Result {
            public string Group { get; set; }
            public string Tag { get; set; }
            public double MeanValue { get; set; }
            public int ValueCount { get; set; }
            public double ValueSum { get; set; }
            public DateTime Date { get; set; }
        }

        public SeriesByHourIndex() {

            Map = series => from s in series
                            let isNumeric = !string.IsNullOrWhiteSpace(s.Value) && Regex.IsMatch(s.Value, @"^-?[0-9]\d*(\.\d+)?$")
                            where isNumeric
                            select new Result {
                                Group = s.Group,
                                Tag = s.Tag,
                                Date = s.Time.Date.AddHours(s.Time.Hour),
                                MeanValue = 0,
                                ValueCount = 1,
                                ValueSum = double.Parse(s.Value)
                            };

            Reduce = results => from result in results
                                group result by new { result.Group, result.Tag, result.Date } into g
                                let valueCount = g.Sum(x => x.ValueCount)
                                let valueSum = g.Sum(x => x.ValueSum)
                                select new Result {
                                    Group = g.Key.Group,
                                    Tag = g.Key.Tag,
                                    ValueSum = valueSum,
                                    MeanValue = valueSum / valueCount,
                                    ValueCount = valueCount,
                                    Date = g.Key.Date
                                };
        }
    }
}