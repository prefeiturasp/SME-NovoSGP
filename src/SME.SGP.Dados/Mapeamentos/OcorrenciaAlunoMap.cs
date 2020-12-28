using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaAlunoMap : BaseMap<OcorrenciaAluno>
    {
        public OcorrenciaAlunoMap()
        {
            ToTable("ocorrencia_aluno");
            Map(x => x.CodigoAluno).ToColumn("codigo_aluno");
            Map(x => x.OcorrenciaId).ToColumn("ocorrencia_id");
        }
    }
}