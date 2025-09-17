using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesPorTipoPap
{
    public class ObterDificuldadesPorTipoPapQuery : IRequest<ContagemDificuldadePorTipoDto>
    {
        public ObterDificuldadesPorTipoPapQuery(TipoPap tipoPap)
        {
            TipoPap = tipoPap;
        }

        public TipoPap TipoPap { get; }
    }
}