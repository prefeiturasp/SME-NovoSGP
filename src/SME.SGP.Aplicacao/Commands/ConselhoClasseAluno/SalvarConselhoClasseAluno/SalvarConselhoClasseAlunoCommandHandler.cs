using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoCommandHandler : AsyncRequestHandler<SalvarConselhoClasseAlunoCommand>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public SalvarConselhoClasseAlunoCommandHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        protected override async Task Handle(SalvarConselhoClasseAlunoCommand request, CancellationToken cancellationToken)
            => await repositorioConselhoClasseAluno.SalvarAsync(request.ConselhoClasseAluno);
    }
}
