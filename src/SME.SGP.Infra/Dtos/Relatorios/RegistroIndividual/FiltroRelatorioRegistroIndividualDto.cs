using System;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioRegistroIndividualDto
    {
        public long TurmaId { get; set; }
        public long? AlunoCodigo { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
