using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Monytor.Domain {

    public static class ValueCompareExtensions {
        public static bool IsEqualByJsonCompare(this object self, object compareValue,
            StringComparison stringComparison = StringComparison.InvariantCulture) {
            if (ReferenceEquals(self, compareValue)) return true;
            if ((self == null) && (compareValue == null)) return true;
            if ((self == null) || (compareValue == null)) return false;
            if (self.GetType() != compareValue.GetType()) return false;

            var selfAsJson = JsonConvert.SerializeObject(self);
            var compareValueAsJson = JsonConvert.SerializeObject(compareValue);

            return string.Equals(selfAsJson, compareValueAsJson, stringComparison);
        }
    }
}
