using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoAlunoMap : BaseEntityMap<ComunicadoAluno>
    {
        public ComunicadoAlunoMap()
        {
            ToTable("comunicado_aluno");
            Map(nameof(ComunicadoAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(ComunicadoAluno.ComunicadoId), "comunicado_id");
        }
    }
}