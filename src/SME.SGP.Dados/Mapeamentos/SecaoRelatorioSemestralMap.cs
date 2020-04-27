using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioSemestralMap: DommelEntityMap<SecaoRelatorioSemestral>
    {
        public SecaoRelatorioSemestralMap()
        {
            ToTable("secao_relatorio_semestral");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
