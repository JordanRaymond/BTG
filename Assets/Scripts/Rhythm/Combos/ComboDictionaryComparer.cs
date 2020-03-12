using System.Collections.Generic;
using System.Linq;


public class ComboDictionaryComparer : IEqualityComparer<List<ComboInput>>
{
    public bool Equals(List<ComboInput> x, List<ComboInput> y) {
        var firstNotSecond = x.Except(y).ToList();
        var secondNotFirst = y.Except(x).ToList();

        return !firstNotSecond.Any() && !secondNotFirst.Any();
    }

    public int GetHashCode(List<ComboInput> obj) {
        unchecked {
            int hash = 19;
            foreach (ComboInput comboInput in obj) {
                hash = hash * 31 + comboInput.GetHashCode();
            }
            return hash;
        }
    }
}
