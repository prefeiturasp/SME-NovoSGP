using System;

namespace SME.SGP.Infra
{
    public class PlanoAEEResumoDto
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }
        public bool PossuiEncaminhamentoAEE { get; set; }
        public bool EhAtendidoAEE { get; set; }
        
        public string Situacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Versao { get; set; }
    }
}
