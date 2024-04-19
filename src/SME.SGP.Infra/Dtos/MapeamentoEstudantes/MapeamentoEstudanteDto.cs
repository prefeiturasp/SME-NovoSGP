using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.MapeamentoEstudantes
{
    public class MapeamentoEstudanteDto
    {
        public MapeamentoEstudanteDto()
        {
            Secoes = new List<MapeamentoEstudanteSecaoDto>();
        }
        public long? Id { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public int Bimestre {  get; set; }
        public List<MapeamentoEstudanteSecaoDto> Secoes { get; set; }
    }
}
