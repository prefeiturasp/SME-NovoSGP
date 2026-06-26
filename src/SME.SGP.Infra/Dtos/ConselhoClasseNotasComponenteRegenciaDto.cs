using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ConselhoClasseNotasComponenteRegenciaDto
    {
        public ConselhoClasseNotasComponenteRegenciaDto()
        {
            NotasFechamentos = new List<NotaBimestreDto>();
        }

        public string Nome { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public List<NotaBimestreDto> NotasFechamentos { get; set; }
        public NotaPosConselhoDto NotaPosConselho { get; set; }
    }
}
