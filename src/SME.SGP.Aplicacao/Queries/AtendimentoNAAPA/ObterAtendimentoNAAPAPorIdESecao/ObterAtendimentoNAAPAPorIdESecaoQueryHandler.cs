using System;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAPorIdESecaoQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentoNAAPAPorIdESecaoQuery, EncaminhamentoNAAPA>
    {
        private readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa;

        public ObterAtendimentoNAAPAPorIdESecaoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNaapa) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapa = repositorioEncaminhamentoNaapa ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapa));
        }

        public async Task<EncaminhamentoNAAPA> Handle(ObterAtendimentoNAAPAPorIdESecaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNaapa.ObterEncaminhamentoPorIdESecao(request.Id, request.SecaoId);
        }
    }
}
