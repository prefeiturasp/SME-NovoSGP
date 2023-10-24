using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPeriodoRelatorioPAPQuery : IRequest<long>
    {
        public ObterIdPeriodoRelatorioPAPQuery(int anoLetivo, int periodo, string tipoPeriodo)
        {
            AnoLetivo = anoLetivo;
            Periodo = periodo;
            TipoPeriodo = tipoPeriodo;
        }

        public int AnoLetivo { get; set; }
        public int Periodo { get; set; }
        public string TipoPeriodo { get; set; }
    }
}
