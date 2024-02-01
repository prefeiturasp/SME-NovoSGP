using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaSecaoDto
    {
        public RegistroAcaoBuscaAtivaSecaoDto()
        {
            Questoes = new List<RegistroAcaoBuscaAtivaSecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public List<RegistroAcaoBuscaAtivaSecaoQuestaoDto> Questoes { get; set; }
    }
}
