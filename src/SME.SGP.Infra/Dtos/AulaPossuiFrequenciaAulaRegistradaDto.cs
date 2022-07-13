using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class AulaPossuiFrequenciaAulaRegistradaDto
    {
        public long Id { get; set; }
        public DateTime DataAula { get; set; }
        public bool AulaCJ { get; set; }
        public string ProfessorRf { get; set; }
        public string CriadoPor { get; set; }
        public TipoAula TipoAula { get; set; }
        public bool PossuiFrequenciaRegistrada { get; set; }
    }
}