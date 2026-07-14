using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroDeFrequenciaAlunoPorIdRegistroQueryHandler : IRequestHandler<ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery, IEnumerable<RegistroFrequenciaAluno>>
    {
        private readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAlunoConsulta;

        public ObterRegistroDeFrequenciaAlunoPorIdRegistroQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorio)
        {
            this.repositorioRegistroFrequenciaAlunoConsulta = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<RegistroFrequenciaAluno>> Handle(ObterRegistroDeFrequenciaAlunoPorIdRegistroQuery request, CancellationToken cancellationToken)
        {
            return repositorioRegistroFrequenciaAlunoConsulta.ObterRegistrosAusenciaPorIdRegistro(request.RegistroFrequenciaId);
        }
    }
}
