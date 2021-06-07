using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasIdFrequenciasExistentesPorAnosLetivosQueryHandler : IRequestHandler<ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery, IEnumerable<FiltroMigracaoFrequenciaAulasDto>>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;

        public ObterTurmasIdFrequenciasExistentesPorAnosLetivosQueryHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }

        public async Task<IEnumerable<FiltroMigracaoFrequenciaAulasDto>> Handle(ObterTurmasIdFrequenciasExistentesPorAnosLetivosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroFrequenciaAluno.ObterTurmasIdFrequenciasExistentesPorAnoAsync(request.AnosLetivos);
        }
    }
}
