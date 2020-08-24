using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAnotacaoFrequenciaAlunoCommandHandler : IRequestHandler<AlterarAnotacaoFrequenciaAlunoCommand, bool>
    {
        private readonly IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno;

        public AlterarAnotacaoFrequenciaAlunoCommandHandler(IRepositorioAnotacaoFrequenciaAluno repositorioAnotacaoFrequenciaAluno)
        {
            this.repositorioAnotacaoFrequenciaAluno = repositorioAnotacaoFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFrequenciaAluno));
        }

        public async Task<bool> Handle(AlterarAnotacaoFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            await repositorioAnotacaoFrequenciaAluno.SalvarAsync(request.Anotacao);
            return true;
        }
    }
}
