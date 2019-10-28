using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosEvento : IComandosEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoEvento servicoEvento;

        public ComandosEvento(IRepositorioEvento repositorioEvento,
                              IServicoEvento servicoEvento)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.servicoEvento = servicoEvento ?? throw new System.ArgumentNullException(nameof(servicoEvento));
        }

        public async Task Alterar(long id, EventoDto eventoDto)
        {
            var evento = repositorioEvento.ObterPorId(id);

            evento = MapearParaEntidade(evento, eventoDto);
            await servicoEvento.Salvar(evento);
        }

        public async Task Criar(EventoDto eventoDto)
        {
            var evento = MapearParaEntidade(new Evento(), eventoDto);
            await servicoEvento.Salvar(evento);
        }

        private Evento MapearParaEntidade(Evento evento, EventoDto eventoDto)
        {
            evento.DataFim = eventoDto.DataFim;
            evento.DataInicio = eventoDto.DataInicio.Value;
            evento.Descricao = eventoDto.Descricao;
            evento.DreId = eventoDto.DreId;
            evento.FeriadoId = eventoDto.FeriadoId;
            evento.Letivo = eventoDto.Letivo;
            evento.Nome = eventoDto.Nome;
            evento.TipoCalendarioId = eventoDto.TipoCalendarioId;
            evento.TipoEventoId = eventoDto.TipoEventoId;
            evento.UeId = eventoDto.UeId;
            return evento;
        }
    }
}