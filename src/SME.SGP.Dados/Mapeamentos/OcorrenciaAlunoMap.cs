using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaAlunoMap : DommelEntityMap<OcorrenciaAluno>
    {
        public OcorrenciaAlunoMap()
        {
            ToTable("ocorrencia_aluno");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.OcorrenciaId).ToColumn("ocorrencia_id");
        }
    }
}