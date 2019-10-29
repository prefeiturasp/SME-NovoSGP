using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo,
                             IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                             IServicoUsuario servicoUsuario)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task Salvar(Evento evento)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);
            if (tipoEvento == null)
            {
                throw new NegocioException("O tipo do evento deve ser informado.");
            }
            evento.AdicionarTipoEvento(tipoEvento);

            evento.ValidaPeriodoEvento();

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            usuario.PodeCriarEvento(evento);

            if (!evento.PermiteConcomitancia())
            {
                var existeOutroEventoNaMesmaData = repositorioEvento.ExisteEventoNaMesmaDataECalendario(evento.DataInicio, evento.TipoCalendarioId);
                if (existeOutroEventoNaMesmaData)
                {
                    throw new NegocioException("Não é permitido cadastrar um evento nesta data pois esse tipo de evento não permite concomitância.");
                }
            }

            if (evento.DeveSerEmDiaLetivo())
            {
                var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

                evento.EstaNoPeriodoLetivo(periodos);
            }

            repositorioEvento.Salvar(evento);
        }
    }
}