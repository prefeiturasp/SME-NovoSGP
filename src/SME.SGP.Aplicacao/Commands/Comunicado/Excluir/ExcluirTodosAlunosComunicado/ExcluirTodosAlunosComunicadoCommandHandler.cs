using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodosAlunosComunicadoCommandHandler : IRequestHandler<ExcluirTodosAlunosComunicadoCommand, bool>
    {
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;

        public ExcluirTodosAlunosComunicadoCommandHandler(IRepositorioComunicadoAluno repositorioComunicadoAluno)
        {
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
        }

        public async Task<bool> Handle(ExcluirTodosAlunosComunicadoCommand request, CancellationToken cancellationToken)
        {
            await repositorioComunicadoAluno.RemoverTodosAlunosComunicado(request.Id);

            return true;
        }
    }
}
