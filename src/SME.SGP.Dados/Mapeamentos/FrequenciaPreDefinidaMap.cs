using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;


namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaPreDefinidaMap : DommelEntityMap<FrequenciaPreDefinida>
    {
        public FrequenciaPreDefinidaMap()
        {
            ToTable("frequencia_pre_definida");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.TurmaId).ToColumn("turma_id");            
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");           
            Map(c => c.Situacao).ToColumn("situacao");           
        }
    }
}
