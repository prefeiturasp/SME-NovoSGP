using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioSemestralPAPMap: DommelEntityMap<SecaoRelatorioSemestralPAP>
    {
        public SecaoRelatorioSemestralPAPMap()
        {
            ToTable("secao_relatorio_semestral_pap");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Obrigatorio).ToColumn("obrigatorio");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.Ordem).ToColumn("ordem");
        }
    }
}
