using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class PlanoAEEMap : BaseEntityMap<PlanoAEE>
    {
        public PlanoAEEMap()
        {
            ToTable("plano_aee");
            Map(nameof(PlanoAEE.TurmaId), "turma_id");
            Map(nameof(PlanoAEE.AlunoNumero), "aluno_numero");
            Map(nameof(PlanoAEE.AlunoCodigo), "aluno_codigo");
            Map(nameof(PlanoAEE.AlunoNome), "aluno_nome");
            Map(nameof(PlanoAEE.Situacao), "situacao");
            Map(nameof(PlanoAEE.ParecerCoordenacao), "parecer_coordenacao");
            Map(nameof(PlanoAEE.ParecerPAAI), "parecer_paai");
            Map(nameof(PlanoAEE.ResponsavelPaaiId), "responsavel_paai_id");
            Map(nameof(PlanoAEE.ResponsavelId), "responsavel_id");
        }
    }
}