using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoAlunoCommandHandler : IRequestHandler<InserirComunicadoAlunoCommand, bool>
    {
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;

        public InserirComunicadoAlunoCommandHandler(IRepositorioComunicadoAluno repositorioComunicadoAluno)
        {
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
        }

        public async Task<bool> Handle(InserirComunicadoAlunoCommand request, CancellationToken cancellationToken)
        {
            foreach (var aluno in request.Alunos)
                await repositorioComunicadoAluno.SalvarAsync(aluno);

            return true;
        }
    }
}
