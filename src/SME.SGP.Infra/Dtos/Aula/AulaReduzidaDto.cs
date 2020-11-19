using System;

namespace SME.SGP.Infra.Dtos
{
    public class AulaReduzidaDto
    {
        public AulaReduzidaDto()
        {

        }
        public long aulaId { get; set; }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public string Professor { get; set; }
        public string ProfessorRf { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public string CodigoUe { get; set; }
    }
}
