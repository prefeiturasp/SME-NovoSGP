using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommandHandler : IRequestHandler<AtribuirResponsavelPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public AtribuirResponsavelPlanoAEECommandHandler(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(AtribuirResponsavelPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEId));

            if (planoAEE == null)
                throw new NegocioException("O Plano AEE informado não foi encontrado");

            if (planoAEE.Situacao == Dominio.Enumerados.SituacaoPlanoAEE.Encerrado)
                throw new NegocioException("A situação do Plano AEE não permite a remoção do responsável");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.DevolutivaPAAI;
            planoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.ResponsavelRF));

            var idEntidadePlanoAEE = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            return idEntidadePlanoAEE != 0;
        }
    }
}
