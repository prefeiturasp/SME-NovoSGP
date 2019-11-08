using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
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
        private readonly IRepositorioFeriadoCalendario repositorioFeriadoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo,
                             IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                             IServicoUsuario servicoUsuario,
                             IRepositorioFeriadoCalendario repositorioFeriadoCalendario,
                             IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFeriadoCalendario = repositorioFeriadoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioFeriadoCalendario));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public static DateTime ObterProximoDiaDaSemana(DateTime data, DayOfWeek diaDaSemana)
        {
            int diasParaAdicionar = ((int)diaDaSemana - (int)data.DayOfWeek + 7) % 7;
            return data.AddDays(diasParaAdicionar);
        }

        public async Task Salvar(Evento evento, bool dataConfirmada = false)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);

            if (tipoEvento == null)
            {
                throw new NegocioException("O tipo do evento deve ser informado.");
            }

            var tipoCalendario = repositorioTipoCalendario.ObterPorId(evento.TipoCalendarioId);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Calendário não encontrado.");
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

            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

            if (evento.DeveSerEmDiaLetivo())
            {
                evento.EstaNoPeriodoLetivo(periodos);
            }

            await VerificaParticularidadesSME(evento, usuario, periodos, dataConfirmada);

            repositorioEvento.Salvar(evento);
        }

        public async Task SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario)
        {
            var feriados = await ObterEValidarFeriados();

            var tipoEventoFeriado = ObterEValidarTipoEventoFeriado();

            var eventos = feriados.Select(x => MapearEntidade(tipoCalendario, x, tipoEventoFeriado));

            var feriadosErro = new List<long>();

            await SalvarListaEventos(eventos, feriadosErro);

            if (feriadosErro.Any())
                TratarErros(feriadosErro);
        }

        public async Task SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada)
        {
            if (!dataFinal.HasValue)
            {
                var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);
                var periodoAtual = periodoEscolar.FirstOrDefault(c => DateTime.Now >= c.PeriodoInicio && DateTime.Now <= c.PeriodoFim);
                dataFinal = periodoAtual.PeriodoFim;
            }
            var eventos = evento.ObterRecorrencia(padraoRecorrencia, padraoRecorrenciaMensal, dataInicial, dataFinal.Value, diasDaSemana, repeteACada, diaDeOcorrencia);
            foreach (var novoEvento in eventos)
            {
                try
                {
                    await Salvar(novoEvento);
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

        private Evento MapearEntidade(TipoCalendario tipoCalendario, FeriadoCalendario x, Entidades.EventoTipo tipoEventoFeriado)
        {
            return new Evento
            {
                FeriadoCalendario = x,
                DataFim = new DateTime(tipoCalendario.AnoLetivo, x.DataFeriado.Month, x.DataFeriado.Day),
                DataInicio = new DateTime(tipoCalendario.AnoLetivo, x.DataFeriado.Month, x.DataFeriado.Day),
                Descricao = x.Nome,
                Nome = x.Nome,
                FeriadoId = x.Id,
                Letivo = tipoEventoFeriado.Letivo,
                TipoCalendario = tipoCalendario,
                TipoCalendarioId = tipoCalendario.Id,
                TipoEvento = tipoEventoFeriado,
                TipoEventoId = tipoEventoFeriado.Id,
                Excluido = false
            };
        }

        private async Task<IEnumerable<FeriadoCalendario>> ObterEValidarFeriados()
        {
            var feriadosMoveis = await repositorioFeriadoCalendario.ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto { Ano = DateTime.Now.Year, Tipo = TipoFeriadoCalendario.Movel });
            var feriadosFixos = await repositorioFeriadoCalendario.ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto { Tipo = TipoFeriadoCalendario.Fixo });

            var feriados = feriadosFixos.ToList();
            feriados.AddRange(feriadosMoveis);

            if (feriados == null || !feriados.Any())
                throw new NegocioException("Nenhum feriado foi encontrado");
            return feriados;
        }

        private EventoTipo ObterEValidarTipoEventoFeriado()
        {
            var tipoEventoFeriado = repositorioEventoTipo.ObtenhaTipoEventoFeriado();

            if (tipoEventoFeriado == null || tipoEventoFeriado.Id == 0)
                throw new NegocioException("Nenhum tipo de evento de feriado foi encontrado");
            return tipoEventoFeriado;
        }

        private async Task SalvarListaEventos(IEnumerable<Evento> eventos, List<long> feriadosErro)
        {
            foreach (var evento in eventos)
            {
                try
                {
                    await repositorioEvento.SalvarAsync(evento);
                }
                catch (Exception ex)
                {
                    feriadosErro.Add(evento.FeriadoId.Value);
                }
            }
        }

        private void TratarErros(List<long> feriadosErro)
        {
            var multiplosErros = feriadosErro.Count > 1;

            var mensagemErro = multiplosErros ? $"Os eventos dos feriados {string.Join(",", feriadosErro)} não foram cadastrados" :
                $"O evento do feriado {feriadosErro.First()} não foi cadastrado";

            throw new NegocioException(mensagemErro);
        }

        private async Task VerificaParticularidadesSME(Evento evento, Usuario usuario, IEnumerable<PeriodoEscolar> periodos, bool dataConfirmada)
        {
            usuario.PodeCriarEventoComDataPassada(evento);
            evento.PodeCriarEventoOrganizacaoEscolar(usuario);
            await VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(evento, usuario);

            evento.PodeCriarEventoLiberacaoExcepcional(evento, usuario, dataConfirmada, periodos);
        }

        private async Task VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(Evento evento, Usuario usuario)
        {
            var eventos = await repositorioEvento.ObterEventosPorTipoETipoCalendario((long)TipoEventoEnum.OrganizacaoEscolar, evento.TipoCalendarioId);
            evento.VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(eventos, usuario);
        }
    }
}