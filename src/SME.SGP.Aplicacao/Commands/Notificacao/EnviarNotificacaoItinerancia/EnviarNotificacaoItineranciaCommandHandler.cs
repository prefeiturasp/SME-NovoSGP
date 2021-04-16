using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoItineranciaCommandHandler : IRequestHandler<EnviarNotificacaoItineranciaCommand, long>
    {
        private readonly IMediator mediator;

        public EnviarNotificacaoItineranciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> Handle(EnviarNotificacaoItineranciaCommand request, CancellationToken cancellationToken)
        {
            var wf = new WorkflowAprovacaoDto()
            {
                EntidadeParaAprovarId = request.ItineranciaId,
                Ano = DateTime.Today.Year,
                NotificacaoCategoria = request.CategoriaNotificacao,
                Tipo = WorkflowAprovacaoTipo.RegistroItinerancia,
                DreId = request.DreCodigo,
                UeId = request.UeCodigo,
                TurmaId = request.TurmaCodigo,
                NotificacaoTitulo = request.Titulo,
                NotificacaoTipo = request.TipoNotificacao,
                NotificacaoMensagem = request.Mensagem
            };

            foreach(var cargo in request.Cargos)
            {
                if (request.CategoriaNotificacao == NotificacaoCategoria.Workflow_Aprovacao)
                    wf.AdicionarNivel(cargo);
                else
                    wf.AdicionarCargo(cargo);
            }

            return await mediator.Send(new SalvarWorkflowAprovacaoCommand(wf));
        }
    }
}
