using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComunicadoAlunoMap : BaseMap<ComunicadoAluno>
    {
        public ComunicadoAlunoMap()
        {
            ToTable("comunicado_aluno");
            Map(c => c.AlunoCodigo).ToColumn("aluno_codigo");
            Map(c => c.AlunoNome).Ignore();
            Map(c => c.ComunicadoId).ToColumn("comunicado_id");
        }
    }
}
