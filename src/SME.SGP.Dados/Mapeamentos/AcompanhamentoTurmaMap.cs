using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AcompanhamentoTurmaMap : BaseMap<AcompanhamentoTurma>
    {
        public AcompanhamentoTurmaMap()
        {
            ToTable("acompanhamento_turma");
            Map(nameof(AcompanhamentoTurma.TurmaId), "turma_id");
            Map(nameof(AcompanhamentoTurma.Semestre), "semestre");
            Map(nameof(AcompanhamentoTurma.ApanhadoGeral), "apanhado_geral");
        }
    }
}