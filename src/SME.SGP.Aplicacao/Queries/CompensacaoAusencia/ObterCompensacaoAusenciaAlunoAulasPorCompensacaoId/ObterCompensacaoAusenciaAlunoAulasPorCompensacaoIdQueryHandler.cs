using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery, IEnumerable<CompensacaoAusenciaAlunoAula>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula;

        public ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQueryHandler(IRepositorioCompensacaoAusenciaAlunoAula repositorioCompensacaoAusenciaAlunoAula)
        {
            this.repositorioCompensacaoAusenciaAlunoAula = repositorioCompensacaoAusenciaAlunoAula ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAula));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> Handle(ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoAula.ObterPorCompensacaoIdAsync(request.CompensacaoId);
        }
    }
}
