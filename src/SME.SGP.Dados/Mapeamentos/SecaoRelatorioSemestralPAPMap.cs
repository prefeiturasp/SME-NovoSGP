using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SecaoRelatorioSemestralPAPMap : SimpleMap<SecaoRelatorioSemestralPAP>
    {
        public SecaoRelatorioSemestralPAPMap()
        {
            ToTable("secao_relatorio_semestral_pap");

            Map(nameof(SecaoRelatorioSemestralPAP.Nome), "nome");
            Map(nameof(SecaoRelatorioSemestralPAP.Descricao), "descricao");
            Map(nameof(SecaoRelatorioSemestralPAP.Obrigatorio), "obrigatorio");
            Map(nameof(SecaoRelatorioSemestralPAP.InicioVigencia), "inicio_vigencia");
            Map(nameof(SecaoRelatorioSemestralPAP.FimVigencia), "fim_vigencia");
            Map(nameof(SecaoRelatorioSemestralPAP.Ordem), "ordem");
        }
    }
}