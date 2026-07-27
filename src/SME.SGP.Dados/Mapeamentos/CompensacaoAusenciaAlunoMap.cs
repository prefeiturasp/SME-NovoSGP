using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class CompensacaoAusenciaAlunoMap : BaseEntityMap<CompensacaoAusenciaAluno>
    {
        public CompensacaoAusenciaAlunoMap()
        {
            ToTable("compensacao_ausencia_aluno");
            Map(nameof(CompensacaoAusenciaAluno.Excluido), "excluido");
            Map(nameof(CompensacaoAusenciaAluno.CompensacaoAusenciaId), "compensacao_ausencia_id");
            Map(nameof(CompensacaoAusenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(CompensacaoAusenciaAluno.QuantidadeFaltasCompensadas), "qtd_faltas_compensadas");
            Map(nameof(CompensacaoAusenciaAluno.Notificado), "notificado");
        }
    }
}