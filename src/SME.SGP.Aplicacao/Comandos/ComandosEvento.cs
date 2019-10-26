using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;

        public ComandosEvento(IRepositorioEvento repositorioEvento,
                              IServicoEvento servicoEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.servicoEvento = servicoEvento ?? throw new System.ArgumentNullException(nameof(servicoEvento));
        }

        public void Salvar(EventoDto eventoDto)
        {
            var evento = repositorioEvento.ObterPorId(eventoDto.Id);

            MapearParaEntidade(evento, eventoDto);
            servicoEvento.Salvar(evento);
        }

        private void MapearParaEntidade(Evento evento, EventoDto eventoDto)
        {
            if (evento == null)
            {
                evento = new Evento();
            }
            evento.DataFim = eventoDto.DataFim;
            evento.DataInicio = eventoDto.DataInicio;
            evento.Descricao = eventoDto.Descricao;
            evento.DreId = eventoDto.DreId;
            evento.FeriadoId = eventoDto.FeriadoId;
            evento.Letivo = eventoDto.Letivo;
            evento.Nome = eventoDto.Nome;
            evento.TipoCalendarioId = eventoDto.TipoCalendarioId;
            evento.TipoEventoId = eventoDto.TipoEventoId;
            evento.UeId = eventoDto.UeId;
        }
    }
}