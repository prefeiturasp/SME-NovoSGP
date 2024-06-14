using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioBuscasAtivasDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public Modalidade Modalidade { get; set; }
        public int? Semestre { get; set; }
        public string[] TurmasCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public string CpfABAE { get; set; }
        public DateTime? DataInicioRegistroAcao { get; set; }
        public DateTime? DataFimRegistroAcao { get; set; }
        public long[] OpcoesRespostaIdMotivoAusencia { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
