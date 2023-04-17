using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery, IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta;

        public ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQueryHandler(IRepositorioCompensacaoAusenciaAlunoAulaConsulta repositorioCompensacaoAusenciaAlunoAulaConsulta)
        {
            this.repositorioCompensacaoAusenciaAlunoAulaConsulta = repositorioCompensacaoAusenciaAlunoAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoAulaConsulta));
        }

        public Task<IEnumerable<CompensacaoAusenciaAlunoAulaSimplificadoDto>> Handle(ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoAulaConsulta.ObterSimplificadoPorAulaIdsAsync(request.AulaIds);
        }
    }
}
