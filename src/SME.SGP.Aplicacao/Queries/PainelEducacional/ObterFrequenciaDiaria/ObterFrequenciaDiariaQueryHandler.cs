using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class ObterFrequenciaDiariaQueryHandler : IRequestHandler<ObterFrequenciaDiariaQuery, IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto>>
    {
        private readonly IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia;

        public ObterFrequenciaDiariaQueryHandler(IRepositorioDashBoardFrequencia repositorioDashBoardFrequencia)
        {
            this.repositorioDashBoardFrequencia = repositorioDashBoardFrequencia;
        }


        public async Task<IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto>> Handle(ObterFrequenciaDiariaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDashBoardFrequencia.ObterDadosParaConsolidacaoPainelEducacional(request.AnoLetivo);
        }
    }
}
