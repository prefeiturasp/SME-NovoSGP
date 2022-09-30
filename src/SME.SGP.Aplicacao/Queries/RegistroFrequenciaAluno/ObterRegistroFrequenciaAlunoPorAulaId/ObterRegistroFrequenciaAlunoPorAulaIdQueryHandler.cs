using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroFrequenciaAlunoPorAulaIdQueryHandler : IRequestHandler<ObterRegistroFrequenciaAlunoPorAulaIdQuery, IEnumerable<RegistroFrequenciaAluno>>
    {
        public readonly IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno;

        public ObterRegistroFrequenciaAlunoPorAulaIdQueryHandler(IRepositorioRegistroFrequenciaAlunoConsulta repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }
        public async Task<IEnumerable<RegistroFrequenciaAluno>> Handle(ObterRegistroFrequenciaAlunoPorAulaIdQuery request, CancellationToken cancellationToken)
            => await repositorioRegistroFrequenciaAluno.ObterRegistrosFrequenciaPorAulaId(request.AulaId);
    }
}
