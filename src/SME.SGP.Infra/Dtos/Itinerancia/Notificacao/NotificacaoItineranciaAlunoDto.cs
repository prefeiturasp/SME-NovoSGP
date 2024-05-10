using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotificacaoItineranciaAlunoDto
    {
        public string AlunoCodigo { get; set; }
        public string AlunoNumeroChamada { get; set; }
        public string AlunoNome { get; set; }
        public long TurmaId { get; set; }
    }
}