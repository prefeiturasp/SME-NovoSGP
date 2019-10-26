using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public void Salvar(Evento evento)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);
            if (tipoEvento == null)
            {
                throw new NegocioException("O tipo do evento deve ser informado.");
            }
            evento.AdicionarTipoEvento(tipoEvento);
            if (!evento.PermiteConcomitancia())
            {
                var existeOutroEventoNaMesmaData = repositorioEvento.ObterEventosPorData(evento.DataInicio);
            }
            repositorioEvento.Salvar(evento);
        }
    }
}