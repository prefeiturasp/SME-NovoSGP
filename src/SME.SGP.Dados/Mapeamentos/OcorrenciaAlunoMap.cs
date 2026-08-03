using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class OcorrenciaAlunoMap : SimpleMap<OcorrenciaAluno>
    {
        public OcorrenciaAlunoMap()
        {
            ToTable("ocorrencia_aluno");
            Map(nameof(OcorrenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(OcorrenciaAluno.OcorrenciaId), "ocorrencia_id");
        }
    }
}