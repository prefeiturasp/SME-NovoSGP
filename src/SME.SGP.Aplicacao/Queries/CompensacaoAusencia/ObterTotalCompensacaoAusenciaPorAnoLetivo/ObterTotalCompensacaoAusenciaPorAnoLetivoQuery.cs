using MediatR;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacaoAusenciaPorAnoLetivoQuery : IRequest<TotalCompensacaoAusenciaDto>
    {
        public ObterTotalCompensacaoAusenciaPorAnoLetivoQuery(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int bimestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semestre = semestre;
            Bimestre = bimestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public int Bimestre { get; set; }
    }
}
