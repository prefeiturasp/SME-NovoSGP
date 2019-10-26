using SME.SGP.Dominio.Entidades;
using System;

namespace SME.SGP.Dominio
{
    public class Evento : EntidadeBase
    {
        public DateTime? DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public string Descricao { get; set; }
        public string DreId { get; set; }
        public FeriadoCalendario FeriadoCalendario { get; set; }
        public long? FeriadoId { get; set; }
        public EventoLetivo Letivo { get; set; }
        public string Nome { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public EventoTipo TipoEvento { get; set; }
        public long TipoEventoId { get; set; }
        public string UeId { get; set; }

        public void AdicionarTipoEvento(EventoTipo tipoEvento)
        {
            TipoEvento = tipoEvento;
        }

        public bool DeveSerEmDiaLetivo()
        {
            TipoEventoObrigatorio();
            return TipoEvento.Letivo == EventoLetivo.Sim;
        }

        public bool PermiteConcomitancia()
        {
            TipoEventoObrigatorio();
            return TipoEvento.Concomitancia;
        }

        private void TipoEventoObrigatorio()
        {
            if (TipoEvento == null)
            {
                throw new NegocioException("O tipo de evento não foi encontrado.");
            }
        }
    }
}