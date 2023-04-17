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
        private readonly IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta;

        public ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQueryHandler(IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta)
        {
            this.repositorioCompensacaoAusenciaAlunoAulaConsulta = repositorioCompensacaoAusenciaAlunoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAulaConsulta));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> Handle(ObterCompensacaoAusenciaAlunoAulasPorCompensacaoIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoAulaConsulta.ObterPorCompensacaoIdAsync(request.CompensacaoId);
        }
    }
}
