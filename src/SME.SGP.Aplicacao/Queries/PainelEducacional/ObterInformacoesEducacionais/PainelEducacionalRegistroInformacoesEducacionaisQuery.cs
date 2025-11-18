using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterInformacoesEducacionais
{
    public class PainelEducacionalRegistroInformacoesEducacionaisQuery : IRequest<InformacoesEducacionaisRetornoDto>
    {
        public PainelEducacionalRegistroInformacoesEducacionaisQuery(FiltroInformacoesEducacionais filtro)
        {
            Filtro = filtro;
        }
        public FiltroInformacoesEducacionais Filtro { get; set; }
    }
}
