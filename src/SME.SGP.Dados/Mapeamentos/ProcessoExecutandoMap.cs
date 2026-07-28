using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ProcessoExecutandoMap : SimpleEntityMap<ProcessoExecutando>
    {
        public ProcessoExecutandoMap()
        {
            ToTable("processo_executando");
            Map(nameof(ProcessoExecutando.TipoProcesso), "tipo_processo");
            Map(nameof(ProcessoExecutando.TurmaId), "turma_id");
            Map(nameof(ProcessoExecutando.DisciplinaId), "disciplina_id");
            Map(nameof(ProcessoExecutando.Bimestre), "bimestre");
            Map(nameof(ProcessoExecutando.AulaId), "aula_id");
            Map(nameof(ProcessoExecutando.CriadoEm), "criado_em");
        }
    }
}