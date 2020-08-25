using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class CartaIntencoesRetornoDto
    {
        public long Id { get; set; }
        public string Planejamento { get; set; }
        public long PeriodoEscolarId { get; set; }
        public int Bimestre { get; set; }
        public bool PeriodoAberto { get; set; }
        public bool UsuarioTemAtribuicao { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
