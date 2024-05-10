using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulaPorAulaIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery, IEnumerable<CompensacaoAusenciaAlunoAula>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta;

        public ObterCompensacaoAusenciaAlunoAulaPorAulaIdQueryHandler(IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta)
        {
            this.repositorioCompensacaoAusenciaAlunoAulaConsulta = repositorioCompensacaoAusenciaAlunoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAulaConsulta));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAula>> Handle(ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoAulaConsulta.ObterPorAulaIdAsync(request.AulaId, request.NumeroAula);
        }

    }
}