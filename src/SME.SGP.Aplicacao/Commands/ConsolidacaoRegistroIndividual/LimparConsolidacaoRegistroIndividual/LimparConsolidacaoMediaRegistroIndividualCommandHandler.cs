using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoMediaRegistroIndividualCommandHandler : IRequestHandler<LimparConsolidacaoMediaRegistroIndividualCommand, bool>
    {
        private readonly IRepositorioConsolidacaoRegistroIndividualMedia repositorio;
        private readonly IMediator mediator;

        public LimparConsolidacaoMediaRegistroIndividualCommandHandler(IRepositorioConsolidacaoRegistroIndividualMedia repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(LimparConsolidacaoMediaRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.LimparConsolidacaoMediaRegistrosIndividuaisPorAno(request.AnoLetivo);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao limpar consolidação de Registro Individual.", LogNivel.Negocio, LogContexto.RegistroIndividual, ex.Message));
            }

            return true;
        }
    }
}
