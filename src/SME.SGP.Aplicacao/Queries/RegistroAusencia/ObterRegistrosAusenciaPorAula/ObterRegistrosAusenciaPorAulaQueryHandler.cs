using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAusenciaPorAulaQueryHandler : IRequestHandler<ObterRegistrosAusenciaPorAulaQuery, IEnumerable<RegistroAusenciaAluno>>
    {
        private readonly IRepositorioRegistroAusenciaAlunoConsulta repositorioRegistroAusenciaAlunoConsulta;

        public ObterRegistrosAusenciaPorAulaQueryHandler(IRepositorioRegistroAusenciaAlunoConsulta repositorio)
        {
            this.repositorioRegistroAusenciaAlunoConsulta = repositorio;
        }

        public async Task<IEnumerable<RegistroAusenciaAluno>> Handle(ObterRegistrosAusenciaPorAulaQuery request, CancellationToken cancellationToken)
            => await repositorioRegistroAusenciaAlunoConsulta.ObterRegistrosAusenciaPorAulaAsync(request.AulaId);
    }
}

