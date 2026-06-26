using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacaoFechamentoAlunoCommandHandler : IRequestHandler<ExcluirAnotacaoFechamentoAlunoCommand>
    {
        private readonly IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno;

        public ExcluirAnotacaoFechamentoAlunoCommandHandler(IRepositorioAnotacaoFechamentoAluno repositorioAnotacaoFechamentoAluno)
        {
            this.repositorioAnotacaoFechamentoAluno = repositorioAnotacaoFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAluno));
        }

        public Task Handle(ExcluirAnotacaoFechamentoAlunoCommand request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAluno.RemoverAsync(request.AnotacaoFechamentoAluno);
    }
}
