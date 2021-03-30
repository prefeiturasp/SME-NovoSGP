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
        public bool EhNaoLetivo { get; set; }
        public List<string> UesIds { get; set; }
        public List<string> DreIds { get; set; }
        public bool PossuiEvento { get; set; }
        public bool PossuiEventoUe(string codigoUe) => PossuiUe(codigoUe) && PossuiEvento;
        public bool PossuiEventoSME() => NaoPossuiUe() && PossuiEvento;
        public bool CriarAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && EhLetivo;
        public bool CriarAulaSME() => PossuiEventoSME() && EhLetivo;
        public bool ExcluirAulaUe(string codigoUe) => PossuiEventoUe(codigoUe) && !EhLetivo;
        public bool ExcluirAulaSME => (UesIds == null || !UesIds.Any()) && !EhLetivo;
        public bool NaoPossuiUe() => (UesIds == null || !UesIds.Any());
        public bool PossuiUe(string codigoUe) => UesIds != null && UesIds.Any(c => c == codigoUe);
    }
}
