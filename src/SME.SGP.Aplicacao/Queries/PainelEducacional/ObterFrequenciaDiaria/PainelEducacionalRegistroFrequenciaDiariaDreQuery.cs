using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class PainelEducacionalRegistroFrequenciaDiariaDreQuery : IRequest<FrequenciaDiariaDreDto>
    {
        public PainelEducacionalRegistroFrequenciaDiariaDreQuery(FiltroFrequenciaDiariaDreDto filtro)
        {
            Filtro = filtro;
        }
        public FiltroFrequenciaDiariaDreDto Filtro { get; set; }
    }
}
