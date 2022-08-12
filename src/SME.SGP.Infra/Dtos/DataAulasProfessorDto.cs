using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class DataAulasProfessorDto
    {
        public DateTime Data { get; set; }
        public long IdAula { get; set; }
        public bool AulaCJ { get; set; }
        public int Bimestre { get; set; }
        public string ProfessorRf { get; set; }
        public string CriadoPor { get; set; }
        public TipoAula TipoAula { get; set; }
    }
}