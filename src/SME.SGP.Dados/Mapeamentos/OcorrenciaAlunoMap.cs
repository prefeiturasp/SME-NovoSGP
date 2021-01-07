using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaAlunoMap : DommelEntityMap<OcorrenciaAluno>
    {
        public OcorrenciaAlunoMap()
        {
            ToTable("ocorrencia_aluno");
            Map(c => c.Id).ToColumn("id");
            Map(x => x.CodigoAluno).ToColumn("codigo_aluno");
            Map(x => x.OcorrenciaId).ToColumn("ocorrencia_id");
        }
    }
}