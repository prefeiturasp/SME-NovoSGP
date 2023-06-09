using System;

namespace SME.SGP.Infra
{
    public class ConselhoClasseParecerConclusivoConsultaDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string CodigoTurma { get; set; }
        public bool ConsideraHistorico { get; set; }

        public ConselhoClasseParecerConclusivoConsultaDto(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, bool consideraHistorico)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
            CodigoTurma = codigoTurma;
            ConsideraHistorico = consideraHistorico;
        }
    }
}
