using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class EncerrarPlanoAeeCommandHandler : IRequestHandler<EncerrarPlanoAeeCommand, RetornoEncerramentoPlanoAEEDto>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;

        public EncerrarPlanoAeeCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IMediator mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoEncerramentoPlanoAEEDto> Handle(EncerrarPlanoAeeCommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoId));
            
            planoAEE.EncerrarPlanoAEE();
            
            var planoId = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            await ExcluirPendenciasPlano(planoId);
            if(await ParametroGeracaoPendenciaAtivo())
                await mediator.Send(new GerarPendenciaCPEncerramentoPlanoAEECommand(planoAEE.Id));

            return new RetornoEncerramentoPlanoAEEDto(planoId, planoAEE.Situacao);
        }

        private async Task ExcluirPendenciasPlano(long planoId)
            => await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoId));

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
