using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQuery : IRequest<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>>
    {
        public PainelEducacionalAprovacaoUeQuery(int anoLetivo, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            Modalidade = modalidade;
            NumeroPagina = numeroPagina;
            NumeroRegistros = numeroRegistros;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public string Modalidade { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
