using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEvento : IConsultasEvento
    {
        private readonly IRepositorioEvento repositorioEvento;

        public ConsultasEvento(IRepositorioEvento repositorioEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
        }

        public EventoObterParaEdicaoDto ObterPorId(long id)
        {
            return MapearParaDto(repositorioEvento.ObterPorId(id));
        }

        public Task<IEnumerable<CalendarioEventosMesesDto>> ObterQuantidadeDeEventosPorMeses(CalendarioEventosMesesFiltroDto calendarioEventosMesesFiltro)
        {
            return repositorioEvento.ObterQuantidadeDeEventosPorMeses(calendarioEventosMesesFiltro);
        }

        private EventoObterParaEdicaoDto MapearParaDto(Evento evento)
        {
            return evento == null ? null : new EventoObterParaEdicaoDto
            {
                DataFim = evento.DataFim,
                DataInicio = evento.DataInicio,
                Descricao = evento.Descricao,
                DreId = evento.DreId,
                FeriadoId = evento.FeriadoId,
                Id = evento.Id,
                Letivo = evento.Letivo,
                Nome = evento.Nome,
                TipoCalendarioId = evento.TipoCalendarioId,
                TipoEventoId = evento.TipoEventoId,
                UeId = evento.UeId
            };
        }
    }
}