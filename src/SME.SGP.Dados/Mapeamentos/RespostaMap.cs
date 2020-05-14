using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RespostaMap : BaseMap<Resposta>
    {
        public RespostaMap()
        {
            ToTable("resposta");
            Map(c => c.DtInicio).ToColumn("dt_inicio");
            Map(c => c.DtFim).ToColumn("dt_fim");
        }
    }
}