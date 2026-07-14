using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTextoRecomendacoesAlunoFamiliaQuery : IRequest<(string recomendacoesAluno, string recomendacoesFamilia)>
    {
    }
}
