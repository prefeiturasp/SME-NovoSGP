using System;

namespace SME.SGP.Infra
{
    public class AlunoInfantilComRegistroIndividualDTO
    {

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
    }

    public class RegistroIndividualAlunoDTO
    {
        public long AlunoCodigo { get; set; }
        public DateTime DataRegistro { get; set; }
    }

    public class MediaRegistoIndividualCriancaDTO
    {
        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Media { get; set; }

    }

}
