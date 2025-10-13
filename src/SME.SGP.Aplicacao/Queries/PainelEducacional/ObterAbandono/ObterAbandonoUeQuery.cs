using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono
{
    public class ObterAbandonoUeQuery : IRequest<PainelEducacionalAbandonoUeDto>
    {
        public ObterAbandonoUeQuery(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros)
        {
            AnoLetivo = anoLetivo;
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            NumeroPagina = numeroPagina;
            NumeroRegistros = numeroRegistros;
        }

        public int AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string Modalidade { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
    }
}