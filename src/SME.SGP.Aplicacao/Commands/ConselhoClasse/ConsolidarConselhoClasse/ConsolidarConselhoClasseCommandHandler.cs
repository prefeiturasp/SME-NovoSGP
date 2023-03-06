using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{

    public class ConsolidarConselhoClasseCommandHandler : IRequestHandler<ConsolidarConselhoClasseCommand, bool>
    {
        private readonly IMediator mediator;

        public ConsolidarConselhoClasseCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ConsolidarConselhoClasseCommand request, CancellationToken cancellationToken)
        {
            var listaConselhoAlunoReprocessar =
                await mediator.Send(new ObterAlunosReprocessamentoConsolidacaoConselhoQuery(request.DreId));

            var listaAgrupada = listaConselhoAlunoReprocessar
                .GroupBy(l => new { l.turmaId, l.bimestre })
                .Select(l => l.Key)
                .Distinct();

            foreach (var conselhoAluno in listaAgrupada)
                await PublicarMensagem(conselhoAluno.turmaId, conselhoAluno.bimestre);

            return true;
        }

        private async Task PublicarMensagem(long turmaId, int bimestre)
        {
            var consolidacaoTurma =
                new ConsolidacaoTurmaDto(turmaId, bimestre);

            var mensagemParaPublicar = JsonConvert
                .SerializeObject(consolidacaoTurma);

            await mediator
                .Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaConselhoClasseSync,
                    mensagemParaPublicar, Guid.NewGuid(), null));
        }
    }
}