using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardTotalAtividadesCompensacaoQuery : IRequest<IEnumerable<TotalCompensacaoAusenciaDto>>
    {
        public ObterDadosDashboardTotalAtividadesCompensacaoQuery(int anoletivo, long dreId, long ueId, int modalidadeCodigo, int bimestre, int semestre)
        {
            Anoletivo = anoletivo;
            DreId = dreId;
            UeId = ueId;
            ModalidadeCodigo = modalidadeCodigo;
            Bimestre = bimestre;
            Semestre = semestre;
        }

        public int Anoletivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public int ModalidadeCodigo { get; set; }
        public int Bimestre { get; set; }
        public int Semestre { get; set; }
    }
}
