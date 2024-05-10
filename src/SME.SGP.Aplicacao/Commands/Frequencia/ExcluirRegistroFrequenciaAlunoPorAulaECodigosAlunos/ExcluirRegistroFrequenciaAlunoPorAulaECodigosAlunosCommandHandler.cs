using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommandHandler : IRequestHandler<ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand, bool>
    {
        public readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IMediator mediator;

        public ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioRegistroFrequenciaAluno.ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunos(request.AulaId, request.CodigosAlunos);
                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao excluir registro de frequência da aula = {request.AulaId} / Motivo: {ex.Message}", LogNivel.Critico, LogContexto.Frequencia));
                return false;
            }
        }
    }
}
