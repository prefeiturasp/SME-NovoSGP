using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAnotacaoFechamentoAlunoCommandHandler : AsyncRequestHandler<SalvarAnotacaoFechamentoAlunoCommand>
    {
        private readonly IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno;

        public SalvarAnotacaoFechamentoAlunoCommandHandler(IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno)
        {
            this.repositorioAnotacaoFechamentoAluno = repositorioAnotacaoFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAluno));
        }

        protected override Task Handle(SalvarAnotacaoFechamentoAlunoCommand request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAluno.SalvarAsync(request.AnotacaoFechamentoAluno);
    }
}
