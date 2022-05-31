using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery : IRequest<PeriodoFechamentoBimestre>
    {
        public ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery(long tipoCandarioId, bool ehTurmaInfantil, int bimestre)
        {
            TipoCandarioId = tipoCandarioId;
            EhTurmaInfantil = ehTurmaInfantil;
            Bimestre = bimestre;
        }

        public long TipoCandarioId { get; set; }
        public bool EhTurmaInfantil { get; set; }
        public int Bimestre { get; set; }
    }
}
