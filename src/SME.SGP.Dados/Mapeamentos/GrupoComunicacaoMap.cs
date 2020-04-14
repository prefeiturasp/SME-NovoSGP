using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class GrupoComunicacaoMap : DommelEntityMap<GrupoComunicacao>
    {
        public GrupoComunicacaoMap()
        {
            ToTable("grupo_comunicado");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.Nome).ToColumn("Nome");
            Map(c => c.TipoCicloId).ToColumn("Tipo_Ciclo_Id");
            Map(c => c.TipoEscolaId).ToColumn("Tipo_Escola_Id");
        }
    }
}