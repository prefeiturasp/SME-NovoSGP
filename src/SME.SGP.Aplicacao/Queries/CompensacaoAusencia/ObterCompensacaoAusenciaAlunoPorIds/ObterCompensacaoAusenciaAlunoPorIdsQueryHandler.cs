using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoPorIdsQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoPorIdsQuery, IEnumerable<CompensacaoAusenciaAluno>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;

        public ObterCompensacaoAusenciaAlunoPorIdsQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorioCompensacaoAusenciaAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAlunoConsulta));
        }

        public Task<IEnumerable<CompensacaoAusenciaAluno>> Handle(ObterCompensacaoAusenciaAlunoPorIdsQuery request, CancellationToken cancellationToken)
        {
            return repositorioCompensacaoAusenciaAlunoConsulta.ObterPorIdsAsync(request.Ids);
        }
    }
}
