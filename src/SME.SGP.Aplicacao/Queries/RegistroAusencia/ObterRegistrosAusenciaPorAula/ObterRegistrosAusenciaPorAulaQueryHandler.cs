using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAusenciaPorAulaQueryHandler : IRequestHandler<ObterRegistrosAusenciaPorAulaQuery, IEnumerable<RegistroFrequenciaAluno>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAlunoConsulta;

        public ObterRegistrosAusenciaPorAulaQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorio)
        {
            this.repositorioRegistroFrequenciaAlunoConsulta = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> Handle(ObterRegistrosAusenciaPorAulaQuery request, CancellationToken cancellationToken)
            => repositorioRegistroFrequenciaAlunoConsulta.ObterRegistrosAusenciaPorAula(request.AulaId);
    }
}

