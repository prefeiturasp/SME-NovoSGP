using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaAulaDto
    {
        public long AulaId { get; set; }
        public bool PossuiPendenciaDiarioBordo { get; set; }
        public bool PossuiPendenciaAtividadeAvaliativa { get; set; }
        public bool PossuiPendenciaFrequencia { get; set; }
        public bool PossuiPendenciaPlanoAula { get; set; }

        public long[] RetornarTipoPendecias()
        {
            var tipoPendencias = new List<TipoPendenciaAula>();

            if (PossuiPendenciaAtividadeAvaliativa)
                tipoPendencias.Add(TipoPendenciaAula.Avaliacao);

            if (PossuiPendenciaDiarioBordo)
                tipoPendencias.Add(TipoPendenciaAula.DiarioBordo);

            if (PossuiPendenciaFrequencia)
                tipoPendencias.Add(TipoPendenciaAula.Frequencia);

            if (PossuiPendenciaPlanoAula)
                tipoPendencias.Add(TipoPendenciaAula.PlanoAula);

            return Array.ConvertAll(tipoPendencias.ToArray(), value => (long)value);
        }
        public DateTime DataAula { get; set; }
        public string Motivo { get; set; }
    }
}
