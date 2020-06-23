using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class AulaConsultaDto
    {
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public bool AulaCJ { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public DateTime DataAula { get; set; }
        public string DisciplinaCompartilhadaId { get; set; }
        public string DisciplinaId { get; set; }
        public long Id { get; set; }
        public string ProfessorRf { get; set; }
        public int Quantidade { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public bool SomenteLeitura { get; set; }
        public TipoAula TipoAula { get; set; }
        public long TipoCalendarioId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
        public bool DentroPeriodo { get; set; }
        public bool Migrado { get; set; }
        public bool EmManutencao { get; set; }

        public void VerificarSomenteLeitura(string disciplinaId)
        {
            SomenteLeitura = string.IsNullOrWhiteSpace(disciplinaId) || !DisciplinaId.Equals(disciplinaId);
        }
    }
}