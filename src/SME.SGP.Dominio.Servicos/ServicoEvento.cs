using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static DateTime ObterProximoDiaDaSemana(DateTime data, DayOfWeek diaDaSemana)
        {
            int diasParaAdicionar = ((int)diaDaSemana - (int)data.DayOfWeek + 7) % 7;
            return data.AddDays(diasParaAdicionar);
        }

        public void GravarRecorrencia(Evento evento, DateTime datatInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal padraoRecorrenciaMensal, int repeteACada)
        {
            if (!dataFinal.HasValue)
            {
                var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);
                var periodoAtual = periodoEscolar.FirstOrDefault(c => DateTime.Now >= c.PeriodoInicio && DateTime.Now <= c.PeriodoFim);
                dataFinal = periodoAtual.PeriodoFim;
            }
            var eventos = evento.ObterRecorrencia(padraoRecorrencia, padraoRecorrenciaMensal, datatInicial, dataFinal.Value, diasDaSemana, repeteACada, diaDeOcorrencia);
            foreach (var novoEvento in eventos)
            {
                try
                {
                    Salvar(novoEvento);
                }
                catch (NegocioException nex)
                {
                    //TODO GERAR NOTIFICAÇÃO DE FEEDBACK
                }
                catch (Exception ex)
                {
                    //TODO GERAR NOTIFICAÇÃO DE FEEDBACK
                }
            }
        }

        public DateTime ObterDomingo(DateTime data)
        {
            int diferenca = (7 + (data.DayOfWeek - DayOfWeek.Monday)) % 7;
            return data.AddDays(-1 * diferenca).Date;
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