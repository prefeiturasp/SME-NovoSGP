using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConselhoClasseComponenteFrequenciaDto
    {
        public ConselhoClasseComponenteFrequenciaDto()
        {
            NotasFechamentos = new List<NotaBimestreDto>();
        }

        public string Nome { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public int QuantidadeAulas { get; set; }
        public int Faltas { get; set; }
        public int AusenciasCompensadas { get; set; }
        public string Frequencia { get; set; }
        public NotaPosConselhoDto NotaPosConselho { get; set; }
        public List<NotaBimestreDto> NotasFechamentos { get; set; }
    }
}
