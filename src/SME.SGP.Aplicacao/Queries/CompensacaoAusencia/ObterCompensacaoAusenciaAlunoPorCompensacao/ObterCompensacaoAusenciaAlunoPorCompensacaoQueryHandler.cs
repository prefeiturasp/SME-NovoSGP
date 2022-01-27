using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaAlunoPorCompensacaoQueryHandler : IRequestHandler<ObterCompensacaoAusenciaAlunoPorCompensacaoQuery, IEnumerable<CompensacaoAusenciaAluno>>
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAlunoConsulta;

        public ObterCompensacaoAusenciaAlunoPorCompensacaoQueryHandler(IRepositorioCompensacaoAusenciaAlunoConsulta repositorio)
        {
            this.repositorioCompensacaoAusenciaAlunoConsulta = repositorio;
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> Handle(ObterCompensacaoAusenciaAlunoPorCompensacaoQuery request, CancellationToken cancellationToken)
            => await repositorioCompensacaoAusenciaAlunoConsulta.ObterPorCompensacao(request.CompensacaoId);
    }
}