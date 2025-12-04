using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaAtendimentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaAtendimentoNAAPAPorIdQuery, RespostaEncaminhamentoNAAPA>
    {
        public IRepositorioRespostaAtendimentoNAAPA repositorioRespostaEncaminhamentoNAAPA { get; }

        public ObterRespostaAtendimentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioRespostaEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<RespostaEncaminhamentoNAAPA> Handle(ObterRespostaAtendimentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(repositorioRespostaEncaminhamentoNAAPA.ObterPorId(request.Id));
        }
    }
}
