using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarFechamentoAlunoCommandHandler : AsyncRequestHandler<SalvarFechamentoAlunoCommand>
    {
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;

        public SalvarFechamentoAlunoCommandHandler(IRepositorioFechamentoAluno repositorioFechamentoAluno)
        {
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
        }

        protected override Task Handle(SalvarFechamentoAlunoCommand request, CancellationToken cancellationToken)
            => repositorioFechamentoAluno.SalvarAsync(request.FechamentoAluno);
    }
}
