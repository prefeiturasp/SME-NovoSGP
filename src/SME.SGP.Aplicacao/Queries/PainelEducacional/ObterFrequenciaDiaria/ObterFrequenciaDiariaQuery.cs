using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class ObterFrequenciaDiariaQuery : IRequest<IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto>>
    {
        public ObterFrequenciaDiariaQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
