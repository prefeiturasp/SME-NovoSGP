using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaDto
    {
        public RegistroAcaoBuscaAtivaDto()
        {
            Secoes = new List<RegistroAcaoBuscaAtivaSecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
      public List<RegistroAcaoBuscaAtivaSecaoDto> Secoes { get; set; }
    }
}
