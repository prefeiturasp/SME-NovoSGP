using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroEventosEscolaAquiDto
    {
        public FiltroEventosEscolaAquiDto() {}

        public FiltroEventosEscolaAquiDto(string codigoDre, string codigoUe, string codigoTurma, int modalidadeCalendario, DateTime mesAno)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            CodigoTurma = codigoTurma;
            ModalidadeCalendario = modalidadeCalendario;
            MesAno = mesAno;
        }

        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public string CodigoTurma { get; set; }
        public int ModalidadeCalendario { get; set; }
        public DateTime MesAno { get; set; }

    }
}
