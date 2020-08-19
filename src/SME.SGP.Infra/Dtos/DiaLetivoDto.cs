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
        }

        public DateTime Data { get; set; }
        public bool EhLetivo { get; set; }
        public List<string> UesIds { get; set; }
        public bool PossuiEvento { get; set; }
        public bool PossuiEventoUe(string codigoUe) => PossuiUe(codigoUe) && PossuiEvento;
        public bool PossuiEventoSME(string codigoUe) => NaoPossuiUe(codigoUe) && PossuiEvento;
        public bool CriarAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && EhLetivo;
        public bool CriarAulaSME(string codigoUe) => PossuiEventoSME(codigoUe) && EhLetivo;
        public bool ExcluirAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && !EhLetivo;
        public bool ExcluirAulaSME => (UesIds == null || !UesIds.Any()) && !EhLetivo;
        public bool NaoPossuiUe(string codigoUe) => (UesIds == null || !UesIds.Any(c => c == codigoUe));
        public bool PossuiUe(string codigoUe) => UesIds != null && UesIds.Any(c => c == codigoUe);
    }
}
