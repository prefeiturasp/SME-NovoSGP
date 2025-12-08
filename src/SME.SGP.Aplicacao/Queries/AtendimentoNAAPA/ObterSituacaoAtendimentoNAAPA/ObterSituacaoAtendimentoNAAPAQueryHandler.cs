using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSituacaoAtendimentoNAAPAQueryHandler : IRequestHandler<ObterSituacaoAtendimentoNAAPAQuery, SituacaoDto>
    {
        public IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public ObterSituacaoAtendimentoNAAPAQueryHandler(IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) 
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public async Task<SituacaoDto> Handle(ObterSituacaoAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPA.ObterSituacao(request.Id);
        }
    }
}
