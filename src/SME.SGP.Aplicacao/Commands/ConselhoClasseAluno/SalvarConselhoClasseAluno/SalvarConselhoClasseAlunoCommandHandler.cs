using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoCommandHandler : IRequestHandler<SalvarConselhoClasseAlunoCommand, long>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public SalvarConselhoClasseAlunoCommandHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<long> Handle(SalvarConselhoClasseAlunoCommand request,CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.SalvarAsync(request.ConselhoClasseAluno);
        }
    }
}
