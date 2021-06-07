using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Infra
{
    public class DiaLetivoDto
    {
        public DiaLetivoDto()
        {
            UesIds = new List<string>();
            DreIds = new List<string>();
        }

        public DateTime Data { get; set; }
        public string Motivo { get; set; }
        public bool EhLetivo { get; set; }
        public bool EhNaoLetivo { get { return !EhLetivo; }  }
        public List<string> UesIds { get; set; }
        public List<string> DreIds { get; set; }
        public bool PossuiEvento { get; set; }
        public bool PossuiEventoDre(string codigoDre) => PossuiDre(codigoDre) && NaoPossuiUe && PossuiEvento;
        public bool PossuiEventoUe(string codigoUe) => PossuiUe(codigoUe) && PossuiEvento;
        public bool PossuiEventoSME => NaoPossuiDre && NaoPossuiUe && PossuiEvento;
        public bool CriarAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && EhLetivo;
        public bool CriarAulaDre(string codigoDre) => PossuiEventoDre(codigoDre) && EhLetivo;
        public bool CriarAulaSME => PossuiEventoSME && EhLetivo;
        public bool ExcluirAulaDre(string codigoDre) => PossuiEventoDre(codigoDre) && !EhLetivo;
        public bool ExcluirAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && !EhLetivo;
        public bool ExcluirAulaSME => NaoPossuiDre && NaoPossuiUe && !EhLetivo;
        public bool NaoPossuiDre => (DreIds == null || !DreIds.Any());
        public bool NaoPossuiUe => (UesIds == null || !UesIds.Any());
        public bool PossuiUe(string codigoUe) => UesIds != null && UesIds.Any(c => c == codigoUe);
        public bool PossuiDre(string codigoDre) => DreIds != null && DreIds.Any(c => c == codigoDre);
    }
}
