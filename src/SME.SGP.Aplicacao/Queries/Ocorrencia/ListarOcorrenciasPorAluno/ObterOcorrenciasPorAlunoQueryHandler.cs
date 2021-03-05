using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasPorAlunoQueryHandler : ConsultasBase, IRequestHandler<ObterOcorrenciasPorAlunoQuery, IEnumerable<OcorrenciasPorAlunoDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IMediator mediator;

        public ObterOcorrenciasPorAlunoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioOcorrencia repositorioOcorrencia, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OcorrenciasPorAlunoDto>> Handle(ObterOcorrenciasPorAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioOcorrencia.ObterOcorrenciasPorTurmaAlunoEPeriodo(request.TurmaId, request.AlunoCodigo, request.PeriodoInicio, request.PeriodoFim);
        }
    }
}
