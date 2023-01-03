using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQueryHandler : IRequestHandler<ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery, SecaoQuestionarioDto>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA;

        public ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoNAAPA)
        {
            this.repositorioSecaoNAAPA = repositorioSecaoNAAPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoNAAPA));
        }

        public async Task<SecaoQuestionarioDto> Handle(ObterSecaoQuestionarioEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoNAAPA.ObterSecaoQuestionarioDtoPorId(request.SecaoId);
        }
    }
}
