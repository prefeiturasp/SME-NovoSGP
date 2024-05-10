using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimoPeriodoEscolarPorDataQuery : IRequest<PeriodoEscolar>
    {
        public ObterUltimoPeriodoEscolarPorDataQuery(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataAtual)
        {
            AnoLetivo = anoLetivo;
            ModalidadeTipoCalendario = modalidadeTipoCalendario;
            DataAtual = dataAtual;
        }
        public int AnoLetivo { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario { get; set; }
        public DateTime DataAtual { get; set; }
    }
}
