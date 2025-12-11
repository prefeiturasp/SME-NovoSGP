using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoAtendimentoNaapaPorIdQueryHandler : ConsultasBase, IRequestHandler<AlterarSituacaoAtendimentoNaapaPorIdQuery, EncaminhamentoNAAPA>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa;
        public AlterarSituacaoAtendimentoNaapaPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapa = repositorioEncaminhamentoNaapa ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapa));
        }

        public async Task<EncaminhamentoNAAPA> Handle(AlterarSituacaoAtendimentoNaapaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNaapa.ObterEncaminhamentoPorId(request.Id);
        }
    }
}
