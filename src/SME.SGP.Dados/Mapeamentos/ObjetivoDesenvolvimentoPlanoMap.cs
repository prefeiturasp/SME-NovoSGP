using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoDesenvolvimentoPlanoMap : DommelEntityMap<ObjetivoDesenvolvimentoPlano>
    {
        public ObjetivoDesenvolvimentoPlanoMap()
        {
            ToTable("objetivo_desenvolvimento_plano");
        }
    }
}