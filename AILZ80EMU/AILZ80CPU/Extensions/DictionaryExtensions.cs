using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AILZ80CPU.Extensions
{
    public static class DictionaryExtensions
    {
        // TryGetValueRegex の拡張メソッドを定義
        public static bool TryGetValueRegex(this Dictionary<string, Action<CPUZ80>> dictionary, string operand, out Action<CPUZ80> action)
        {
            foreach (var key in dictionary.Keys)
            {
                // 正規表現を使ってキーがオペランドに一致するかを確認
                if (Regex.IsMatch(operand, key))
                {
                    action = dictionary[key];
                    return true;
                }
            }

            action = default!;

            return false;
        }
    }
}
