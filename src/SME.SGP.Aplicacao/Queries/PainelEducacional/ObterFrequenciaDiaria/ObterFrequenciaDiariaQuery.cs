using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class ObterFrequenciaDiariaQuery : IRequest<IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto>>
    {
        public ObterFrequenciaDiariaQuery(int anoLetivo, long dreId)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
        }

        public long DreId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
