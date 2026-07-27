using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaPreDefinidaMap : SimpleEntityMap<FrequenciaPreDefinida>
    {
        public FrequenciaPreDefinidaMap()
        {
            ToTable("frequencia_pre_definida");
            Map(nameof(FrequenciaPreDefinida.TurmaId), "turma_id");
            Map(nameof(FrequenciaPreDefinida.CodigoAluno), "codigo_aluno");
            Map(nameof(FrequenciaPreDefinida.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(FrequenciaPreDefinida.TipoFrequencia), "tipo_frequencia");
        }
    }
}