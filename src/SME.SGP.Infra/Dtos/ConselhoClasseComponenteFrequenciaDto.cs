using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoClasseComponenteFrequenciaDto
    {
        public ConselhoClasseComponenteFrequenciaDto()
        {
            NotasFechamentos = new List<NotaBimestreDto>();
        }

        public string Nome { get; set; }
        public int QuantidadeAulas { get; set; }
        public int Faltas { get; set; }
        public int AusenciasCompensadas { get; set; }
        public double Frequencia { get; set; }
        public double NotaPosConselho { get; set; }
        public List<NotaBimestreDto> NotasFechamentos { get; set; }
    }
}
