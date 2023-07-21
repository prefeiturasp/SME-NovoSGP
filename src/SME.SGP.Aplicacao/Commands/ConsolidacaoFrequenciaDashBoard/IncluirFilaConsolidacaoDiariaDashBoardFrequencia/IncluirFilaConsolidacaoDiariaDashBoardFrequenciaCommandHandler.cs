using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandHandler : IRequestHandler<IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var filtro = new ConsolidacaoPorTurmaDashBoardFrequencia()
            {
                TurmaId = request.TurmaId,
                Mes = request.DataAula.Month,
                AnoLetivo = request.DataAula.Year,
                DataAula = request.DataAula,
            };            

            var publicar = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma, filtro, Guid.NewGuid(), null));
            if (!publicar)
            {
                var mensagem = string.Format(MensagemNegocioComuns.NAO_FOI_POSSIVEL_INSERIR_TURMA_X_NA_FILA_DE_CONSOLIDACAO_DIARIA_DE_FREQUENCIA, request.TurmaId);
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, Dominio.Enumerados.LogNivel.Critico, Dominio.Enumerados.LogContexto.Frequencia));
            }

            return true;
        }
    }
}
