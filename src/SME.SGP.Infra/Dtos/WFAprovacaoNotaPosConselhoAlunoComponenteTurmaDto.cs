using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra.Dtos
{
    public class WFAprovacaoNotaPosConselhoAlunoComponenteTurmaDto
    {
        public DateTime CriadoEm { get; set; }
        public long UsuarioSolicitanteId { get; set; }
        public string NomeUsuarioSolicitante { get; set; }
        public string CodigoRfUsuarioSolicitante { get; set; }
        public int CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public int NumeroAlunoChamada { get; set; }
        public string CodigoTurma { get; set; }
        public long CodigoComponenteCurricular { get; set; }
        public string DescricaoComponenteCurricular { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }

        public double? NotaConselhoClasse { get; set; }
        public long? ConceitoIdConselhoClasse { get; set; }
    }
}
