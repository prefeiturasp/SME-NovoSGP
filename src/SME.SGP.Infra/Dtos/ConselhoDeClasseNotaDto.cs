using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConselhoDeClasseNotaDto
    {
        public long Id { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public ConceitoDto Conceito { get; set; }
        public string Justificativa { get; set; }

        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        
        public AuditoriaDto Auditoria { get; set; }

        public static explicit operator ConselhoDeClasseNotaDto(ConselhoClasseNota conselhoClasseNota)
             => new ConselhoDeClasseNotaDto
             {
                 Auditoria = (AuditoriaDto)conselhoClasseNota,
                 ComponenteCurricularCodigo = conselhoClasseNota.ComponenteCurricularCodigo,
                 Conceito = (ConceitoDto)conselhoClasseNota.Conceito,
                 Id = conselhoClasseNota.Id,
                 ConceitoId = conselhoClasseNota.ConceitoId,
                 ConselhoClasseAlunoId = conselhoClasseNota.ConselhoClasseAlunoId,
                 Excluido = conselhoClasseNota.Excluido,
                 Justificativa = conselhoClasseNota.Justificativa,
                 Migrado = conselhoClasseNota.Migrado,
                 Nota = conselhoClasseNota.Nota
             };
    }
}
