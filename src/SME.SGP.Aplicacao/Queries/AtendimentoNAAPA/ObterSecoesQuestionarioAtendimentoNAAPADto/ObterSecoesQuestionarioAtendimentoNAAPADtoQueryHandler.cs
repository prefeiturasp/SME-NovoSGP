using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSecoesQuestionarioAtendimentoNAAPADtoQueryHandler : IRequestHandler<ObterSecoesQuestionarioAtendimentoNAAPADtoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoAtendimentoNAAPA repositorioSecaoNAAPA;

        public ObterSecoesQuestionarioAtendimentoNAAPADtoQueryHandler(IRepositorioSecaoAtendimentoNAAPA repositorioSecaoNAAPA)
        {
            this.repositorioSecaoNAAPA = repositorioSecaoNAAPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoNAAPA));
        }
        
        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesQuestionarioAtendimentoNAAPADtoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoNAAPA.ObterSecoesQuestionarioDto(request.Modalidade);
        }
    }
}
