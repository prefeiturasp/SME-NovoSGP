using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoTurmaCommandHandler : IRequestHandler<InserirComunicadoTurmaCommand, bool>
    {
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;

        public InserirComunicadoTurmaCommandHandler(IRepositorioComunicadoTurma repositorioComunicadoTurma)
        {
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
        }

        public async Task<bool> Handle(InserirComunicadoTurmaCommand request, CancellationToken cancellationToken)
        {
            foreach (var turma in request.Turmas)
                await repositorioComunicadoTurma.SalvarAsync(turma);

            return true;
        }
    }
}
