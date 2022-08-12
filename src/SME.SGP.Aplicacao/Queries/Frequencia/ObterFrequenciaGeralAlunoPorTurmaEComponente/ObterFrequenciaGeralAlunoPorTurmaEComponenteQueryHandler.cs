using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmaEComponenteQueryHandler : IRequestHandler<ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery, FrequenciaAluno>
    {
        private readonly IConsultasFrequencia consultasFrequencia;

        public ObterFrequenciaGeralAlunoPorTurmaEComponenteQueryHandler(IConsultasFrequencia consultasFrequencia)
        {
            this.consultasFrequencia = consultasFrequencia ?? throw new ArgumentNullException(nameof(consultasFrequencia));
        }
        public async Task<FrequenciaAluno> Handle(ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery request, CancellationToken cancellationToken)
         => await consultasFrequencia.ObterFrequenciaGeralAlunoPorTurmaEComponente(request.AlunoCodigo, request.TurmaCodigo, request.ComponenteCurricularCodigo);
    }
}
