using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SME.SGP.Dominio.DateTimeExtension;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorioProcessoExecutando;
        private readonly IMediator mediator;

        public IncluirFilaCalcularFrequenciaPorTurmaCommandHandler(IRepositorioProcessoExecutando repositorioProcessoExecutando, IMediator mediator)
        {
            this.repositorioProcessoExecutando = repositorioProcessoExecutando ?? throw new ArgumentNullException(nameof(repositorioProcessoExecutando));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(IncluirFilaCalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var comando = new CalcularFrequenciaPorTurmaCommand(request.Alunos, request.DataAula, request.TurmaId, request.DisciplinaId, request.Bimestre);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null));

            return true;
        }
    }
}