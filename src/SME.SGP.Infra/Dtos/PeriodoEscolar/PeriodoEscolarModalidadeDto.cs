using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PeriodoEscolarModalidadeDto
    {
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public int Bimestre { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
