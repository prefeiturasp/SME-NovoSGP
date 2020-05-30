using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioSemestralPAPMap: DommelEntityMap<SecaoRelatorioSemestralPAP>
    {
        public SecaoRelatorioSemestralPAPMap()
        {
            ToTable("secao_relatorio_semestral_pap");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}
