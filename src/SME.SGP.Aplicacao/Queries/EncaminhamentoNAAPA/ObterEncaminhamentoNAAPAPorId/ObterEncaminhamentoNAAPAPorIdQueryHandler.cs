using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentoNAAPAPorIdQuery, EncaminhamentoNAAPA>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNaapa;
        public ObterEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNaapa) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapa = repositorioEncaminhamentoNaapa ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapa));
        }

        public async Task<EncaminhamentoNAAPA> Handle(ObterEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNaapa.ObterEncaminhamentoPorId(request.Id);
        }
    }
}
