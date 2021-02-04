using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<AtribuirResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public AtribuirResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(AtribuirResponsavelEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            if( encaminhamentoAEE.Situacao != Dominio.Enumerados.SituacaoAEE.Analise
             || encaminhamentoAEE.Situacao != Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel)
                throw new NegocioException("A situação do encaminhamento não permite atribuição do responsável");

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.RfResponsavel));

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            return idEntidadeEncaminhamento != 0;
        }
    }
}
