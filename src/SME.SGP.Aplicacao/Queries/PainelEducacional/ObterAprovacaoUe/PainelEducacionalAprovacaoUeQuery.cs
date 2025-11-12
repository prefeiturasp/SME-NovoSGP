using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQuery : IRequest<PainelEducacionalAprovacaoUeRetorno>
    {
        public PainelEducacionalAprovacaoUeQuery(FiltroAprovacaoUeDto filtro)
        {
            Filtro = filtro;
        }

        public FiltroAprovacaoUeDto Filtro { get; set; }
    }
}
