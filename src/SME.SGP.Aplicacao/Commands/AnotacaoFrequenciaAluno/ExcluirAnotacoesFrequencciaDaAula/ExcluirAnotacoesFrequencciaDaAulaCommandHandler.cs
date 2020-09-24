using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacoesFrequencciaDaAulaCommandHandler : IRequestHandler<ExcluirAnotacoesFrequencciaDaAulaCommand, bool>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ExcluirAnotacoesFrequencciaDaAulaCommandHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<bool> Handle(ExcluirAnotacoesFrequencciaDaAulaCommand request, CancellationToken cancellationToken)
            => await repositorioAnotacaoFrequenciaAluno.ExcluirAnotacoesDaAula(request.AulaId);
    }
}
