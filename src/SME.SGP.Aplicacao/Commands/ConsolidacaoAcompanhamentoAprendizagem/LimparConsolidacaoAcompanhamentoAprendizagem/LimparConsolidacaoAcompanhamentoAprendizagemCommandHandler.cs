using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoAcompanhamentoAprendizagemCommandHandler : IRequestHandler<LimparConsolidacaoAcompanhamentoAprendizagemCommand, bool>
    {
        private readonly IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio;
        private readonly IMediator mediator;

        public LimparConsolidacaoAcompanhamentoAprendizagemCommandHandler(IRepositorioConsolidacaoAcompanhamentoAprendizagemAluno repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(LimparConsolidacaoAcompanhamentoAprendizagemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorio.Limpar(request.AnoLetivo);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Limpar consolidação de Acompanhamento de aprendizagem.", LogNivel.Negocio, LogContexto.SincronizacaoInstitucional, ex.Message));
            }

            return true;
        }
    }
}
