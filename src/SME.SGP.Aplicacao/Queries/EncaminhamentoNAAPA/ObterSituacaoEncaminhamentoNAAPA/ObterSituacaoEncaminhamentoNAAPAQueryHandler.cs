using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterSituacaoEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterSituacaoEncaminhamentoNAAPAQuery, SituacaoDto>
    {
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public ObterSituacaoEncaminhamentoNAAPAQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) 
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }
        public async Task<SituacaoDto> Handle(ObterSituacaoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPA.ObterSituacao(request.Id);
        }
    }
}
