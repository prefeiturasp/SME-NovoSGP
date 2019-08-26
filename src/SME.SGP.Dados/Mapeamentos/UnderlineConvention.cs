using Dapper.FluentMap.Conventions;
using System.Text.RegularExpressions;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UnderlineConvention : Convention
    {
        public UnderlineConvention()
        {
            Properties()
                .Configure(c => c.Transform(s => Regex.Replace(input: s, pattern: "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", replacement: "$1$3_$2$4")));
        }
    }
}