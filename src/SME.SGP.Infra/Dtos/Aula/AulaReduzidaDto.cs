using System;

namespace SME.SGP.Infra.Dtos
{
    public class AulaReduzidaDto
    {
        public AulaReduzidaDto()
        {

        }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public string Professor { get; set; }
        public string ProfessorRf { get; set; }
    }
}
