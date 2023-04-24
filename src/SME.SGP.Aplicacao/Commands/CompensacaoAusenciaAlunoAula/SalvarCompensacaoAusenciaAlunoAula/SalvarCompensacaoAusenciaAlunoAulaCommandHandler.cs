using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaAlunoAulaCommandHandler : IRequestHandler<SalvarCompensacaoAusenciaAlunoAulaCommand, long>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;

        public SalvarCompensacaoAusenciaAlunoAulaCommandHandler(IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula)
        {
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
        }

        public Task<long> Handle(SalvarCompensacaoAusenciaAlunoAulaCommand request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoAula.SalvarAsync(request.CompensacaoAusenciaAlunoAula);
        }
    }
}
