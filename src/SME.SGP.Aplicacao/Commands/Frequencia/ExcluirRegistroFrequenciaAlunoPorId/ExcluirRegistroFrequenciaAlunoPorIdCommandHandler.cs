using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroFrequenciaAlunoPorIdCommandHandler : IRequestHandler<ExcluirRegistroFrequenciaAlunoPorIdCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IMediator mediator;

        public ExcluirRegistroFrequenciaAlunoPorIdCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirRegistroFrequenciaAlunoPorIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioRegistroFrequenciaAluno.RemoverLogico(request.Ids);
                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao excluir registro de frequência dos ids = {request.Ids} / Motivo: {ex.Message}", LogNivel.Critico, LogContexto.Frequencia));
                return false;
            }
        }
    }
}
