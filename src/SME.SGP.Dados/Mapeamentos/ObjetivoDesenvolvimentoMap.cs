using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoDesenvolvimentoMap : DommelEntityMap<ObjetivoDesenvolvimento>
    {
        public ObjetivoDesenvolvimentoMap()
        {
            ToTable("objetivo_desenvolvimento");
        }
    }
}