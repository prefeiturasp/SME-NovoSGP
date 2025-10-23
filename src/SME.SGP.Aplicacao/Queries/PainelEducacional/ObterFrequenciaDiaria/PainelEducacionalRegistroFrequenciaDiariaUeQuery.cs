using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaDiaria
{
    public class PainelEducacionalRegistroFrequenciaDiariaUeQuery : IRequest<FrequenciaDiariaUeDto>
    {
        public PainelEducacionalRegistroFrequenciaDiariaUeQuery(FiltroFrequenciaDiariaUeDto filtro)
        {
            Filtro = filtro;
        }
        public FiltroFrequenciaDiariaUeDto Filtro { get; set; }
    }
}
