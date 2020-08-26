using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacaoFrequenciaAlunoCommandHandler : IRequestHandler<ExcluirAnotacaoFrequenciaAlunoCommand, bool>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public ExcluirAnotacaoFrequenciaAlunoCommandHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<bool> Handle(ExcluirAnotacaoFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            request.AnotacaoFrequenciaAluno.Excluido = true;
            await repositorioAnotacaoFrequenciaAluno.SalvarAsync(request.AnotacaoFrequenciaAluno);
            return await Task.FromResult(true);
        }
    }
}
