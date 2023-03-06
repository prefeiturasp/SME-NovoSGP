using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandler: IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery,AbrangenciaFiltroRetorno>
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;

        public ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandler(IConsultasAbrangencia consultasAbrangencia)
        {
            this.consultasAbrangencia =
                consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<AbrangenciaFiltroRetorno> Handle(ObterAbrangenciaPorTurmaEConsideraHistoricoQuery request, CancellationToken cancellationToken)
        {
            return await consultasAbrangencia.ObterAbrangenciaTurma(request.TurmaId, request.ConsideraHistorico);
        }
    }
}