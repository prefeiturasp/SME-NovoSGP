using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RecalcularFrequenciaPorTurmaCommandHandler : IRequestHandler<RecalcularFrequenciaPorTurmaCommand, bool>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IMediator mediator;

        public RecalcularFrequenciaPorTurmaCommandHandler(IRepositorioAula repositorioAula, IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RecalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var periodo = await repositorioAula.ObterPeriodoEscolarDaAula(request.AulaId);

            if (periodo == null)
                throw new NegocioException($"Não encontrado período escolar da aula [{request.AulaId}]");

            return await mediator.Send(new IncluirFilaConciliacaoFrequenciaTurmaCommand(request.TurmaCodigo, periodo.Bimestre, request.ComponenteCurricularId, periodo.DataInicio, periodo.DataFim));
        }
    }
}
