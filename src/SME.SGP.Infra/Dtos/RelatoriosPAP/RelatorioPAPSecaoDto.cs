using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioPAPSecaoDto
    {
        public RelatorioPAPSecaoDto()
        {
            Respostas = new List<RelatorioPAPRespostaDto>();
        }

        public long? Id { get; set; }
        public long SecaoId { get; set; }
        public List<RelatorioPAPRespostaDto> Respostas { get; set; }
    }
}
