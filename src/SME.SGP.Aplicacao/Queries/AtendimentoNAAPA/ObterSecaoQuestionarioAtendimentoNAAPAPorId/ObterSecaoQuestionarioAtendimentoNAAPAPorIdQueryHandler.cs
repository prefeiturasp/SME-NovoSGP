using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoQuestionarioAtendimentoNAAPAPorIdQueryHandler : IRequestHandler<ObterSecaoQuestionarioAtendimentoNAAPAPorIdQuery, SecaoQuestionarioDto>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA;

        public ObterSecaoQuestionarioAtendimentoNAAPAPorIdQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA)
        {
            this.repositorioSecaoNAAPA = repositorioSecaoNAAPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoNAAPA));
        }

        public async Task<SecaoQuestionarioDto> Handle(ObterSecaoQuestionarioAtendimentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoNAAPA.ObterSecaoQuestionarioDtoPorId(request.SecaoId);
        }
    }
}
