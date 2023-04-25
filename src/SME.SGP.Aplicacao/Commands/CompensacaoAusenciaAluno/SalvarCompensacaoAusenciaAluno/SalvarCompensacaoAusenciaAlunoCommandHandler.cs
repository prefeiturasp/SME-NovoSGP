using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoCommandHandler : IRequestHandler<SalvarCompensacaoAusenciaAlunoCommand, long>
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public SalvarCompensacaoAusenciaAlunoCommandHandler(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public Task<long> Handle(SalvarCompensacaoAusenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAluno.SalvarAsync(request.CompensacaoAusenciaAluno);
        }
    }
}
