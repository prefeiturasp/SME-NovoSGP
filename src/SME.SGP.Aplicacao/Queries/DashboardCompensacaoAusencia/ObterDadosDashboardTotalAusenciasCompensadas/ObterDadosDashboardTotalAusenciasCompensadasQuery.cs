using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAusenciasCompensadasQuery : IRequest<IEnumerable<TotalCompensacaoAusenciaDto>>
    {
        public ObterDadosDashboardTotalAusenciasCompensadasQuery(int anoLetivo, long dreId, long ueId, int modalidadeCodigo, int bimestre, int semestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            ModalidadeCodigo = modalidadeCodigo;
            Bimestre = bimestre;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int ModalidadeCodigo { get; set; }
        public int Bimestre { get; set; }
        public int Semestre { get; set; }
    }
}
