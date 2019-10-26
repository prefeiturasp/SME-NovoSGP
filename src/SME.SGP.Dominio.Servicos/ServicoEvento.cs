using SME.SGP.Dominio.Interfaces;
using System.Linq;

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

        public void Salvar(Evento evento)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);
            if (tipoEvento == null)
            {
                throw new NegocioException("O tipo do evento deve ser informado.");
            }
            evento.AdicionarTipoEvento(tipoEvento);

            var usuario = servicoUsuario.ObterUsuarioLogado();

            if (!evento.PermiteConcomitancia())
            {
                var existeOutroEventoNaMesmaData = repositorioEvento.ExisteEventoNaDataEspecificada(evento.DataInicio);
                if (existeOutroEventoNaMesmaData)
                {
                    throw new NegocioException("Não é permitido cadastrar um evento nesta data pois esse tipo de evento não permite concomitância.");
                }
            }

            if (evento.DeveSerEmDiaLetivo())
            {
                var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

                if (!periodos.Any(c => c.PeriodoInicio >= evento.DataInicio && c.PeriodoFim <= evento.DataInicio))
                {
                    throw new NegocioException("Não é permitido cadastrar um evento nesta data pois essa data não está dentro do 'Período Letivo'.");
                }
            }

            repositorioEvento.Salvar(evento);
        }
    }
}