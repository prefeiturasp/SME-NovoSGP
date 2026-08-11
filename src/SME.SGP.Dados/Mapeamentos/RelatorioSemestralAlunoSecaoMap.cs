using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioSemestralAlunoSecaoMap : SimpleMap<RelatorioSemestralPAPAlunoSecao>
    {
        public RelatorioSemestralAlunoSecaoMap()
        {
            ToTable("relatorio_semestral_pap_aluno_secao");

            Map(nameof(RelatorioSemestralPAPAlunoSecao.RelatorioSemestralPAPAlunoId), "relatorio_semestral_pap_aluno_id");
            Map(nameof(RelatorioSemestralPAPAlunoSecao.SecaoRelatorioSemestralPAPId), "secao_relatorio_semestral_pap_id");
            Map(nameof(RelatorioSemestralPAPAlunoSecao.Valor), "valor");
        }
    }
}