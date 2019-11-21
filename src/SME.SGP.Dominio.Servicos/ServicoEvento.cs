using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoEvento : IServicoEvento
    {
        private readonly IComandosWorkflowAprovacao comandosWorkflowAprovacao;
        private readonly IConfiguration configuration;
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoTipo repositorioEventoTipo;
        private readonly IRepositorioFeriadoCalendario repositorioFeriadoCalendario;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IServicoLog servicoLog;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo,
                             IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                             IServicoUsuario servicoUsuario,
                             IRepositorioFeriadoCalendario repositorioFeriadoCalendario,
                             IRepositorioTipoCalendario repositorioTipoCalendario,
                             IComandosWorkflowAprovacao comandosWorkflowAprovacao,
                             IRepositorioAbrangencia repositorioAbrangencia, IConfiguration configuration,
                             IUnitOfWork unitOfWork, IServicoNotificacao servicoNotificacao, IServicoLog servicoLog, IServicoDiaLetivo servicoDiaLetivo)
        {
            this.repositorioEvento = repositorioEvento ?? throw new System.ArgumentNullException(nameof(repositorioEvento));
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new System.ArgumentNullException(nameof(repositorioEventoTipo));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.repositorioFeriadoCalendario = repositorioFeriadoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioFeriadoCalendario));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.comandosWorkflowAprovacao = comandosWorkflowAprovacao ?? throw new ArgumentNullException(nameof(comandosWorkflowAprovacao));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
            this.configuration = configuration;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new ArgumentNullException(nameof(servicoDiaLetivo));
        }

        public static DateTime ObterProximoDiaDaSemana(DateTime data, DayOfWeek diaDaSemana)
        {
            int diasParaAdicionar = ((int)diaDaSemana - (int)data.DayOfWeek + 7) % 7;
            return data.AddDays(diasParaAdicionar);
        }

        public void AlterarRecorrenciaEventos(Evento evento, bool alterarRecorrenciaCompleta)
        {
            if (evento.EventoPaiId.HasValue && evento.EventoPaiId > 0 && alterarRecorrenciaCompleta)
            {
                IEnumerable<Evento> eventos = repositorioEvento.ObterEventosPorRecorrencia(evento.Id, evento.EventoPaiId.Value, evento.DataInicio);
                if (eventos != null && eventos.Any())
                {
                    foreach (var eventoASerAlterado in eventos)
                    {
                        var eventoAlterado = AlterarEventoDeRecorrencia(evento, eventoASerAlterado);
                        repositorioEvento.Salvar(eventoAlterado);
                    }
                }
            }
        }

        public async Task<string> Salvar(Evento evento, bool alterarRecorrenciaCompleta = false, bool dataConfirmada = false)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);

            if (tipoEvento == null)
                throw new NegocioException("O tipo do evento deve ser informado.");

            evento.AdicionarTipoEvento(tipoEvento);

            var tipoCalendario = repositorioTipoCalendario.ObterPorId(evento.TipoCalendarioId);
            if (tipoCalendario == null)
                throw new NegocioException("Calendário não encontrado.");

            evento.AdicionarTipoCalendario(tipoCalendario);

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

            await VerificarParticularidadesSME(evento, usuario, periodos, dataConfirmada);

            AtribuirNullSeVazio(evento);

            repositorioEvento.Salvar(evento);

            SME.Background.Core.Cliente.Executar<IServicoEvento>(x => x.AlterarRecorrenciaEventos(evento, alterarRecorrenciaCompleta));

            var mensagemRetornoSucesso = "Evento cadastrado com sucesso.";

            if (evento.TipoEvento.Codigo != (int)TipoEventoEnum.LiberacaoExcepcional)
            {
                var temEventoDeLiberacaoExcepcional = await repositorioEvento.TemEventoNosDiasETipo(evento.DataInicio, evento.DataFim, TipoEventoEnum.LiberacaoExcepcional,
                tipoCalendario.Id, evento.UeId, evento.DreId);

                if (temEventoDeLiberacaoExcepcional)
                {
                    await PersistirWorkflowEvento(evento);
                    mensagemRetornoSucesso = "Evento cadastrado e será válido após aprovação.";
                }
            }

            return mensagemRetornoSucesso;
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

        public void SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada)
        {
            if (evento.EventoPaiId.HasValue && evento.EventoPaiId > 0)
            {
                throw new NegocioException("Este evento já pertence a uma recorrência, por isso não é permitido gerar uma nova.");
            }
            if (!dataFinal.HasValue)
            {
                var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);
                if (periodoEscolar == null || !periodoEscolar.Any())
                {
                    throw new NegocioException("Não é possível cadastrar o evento pois não existe período escolar cadastrado para este calendário.");
                }
                var periodoAtual = periodoEscolar.FirstOrDefault(c => DateTime.Now >= c.PeriodoInicio && DateTime.Now <= c.PeriodoFim);
                dataFinal = periodoAtual.PeriodoFim;
            }
            var eventos = evento.ObterRecorrencia(padraoRecorrencia, padraoRecorrenciaMensal, dataInicial, dataFinal.Value, diasDaSemana, repeteACada, diaDeOcorrencia);
            var notificacoesSucesso = new List<DateTime>();
            var notificacoesFalha = new List<string>();
            foreach (var novoEvento in eventos)
            {
                try
                {
                    if (!servicoDiaLetivo.ValidarSeEhDiaLetivo(novoEvento.DataInicio, novoEvento.DataInicio, novoEvento.TipoCalendarioId, novoEvento.Letivo == EventoLetivo.Sim, novoEvento.TipoEventoId))
                    {
                        notificacoesFalha.Add($"{novoEvento.DataInicio.ToShortDateString()} - Não é possível cadastrar esse evento pois a data informada está fora do período letivo.");
                    }
                    else
                    {
                        Salvar(novoEvento).Wait();
                        notificacoesSucesso.Add(novoEvento.DataInicio);
                    }
                }
                catch (NegocioException nex)
                {
                    notificacoesFalha.Add($"{novoEvento.DataInicio.ToShortDateString()} - {nex.Message}");
                }
                catch (Exception ex)
                {
                    notificacoesFalha.Add($"{novoEvento.DataInicio.ToShortDateString()} - Ocorreu um erro interno.");
                    servicoLog.Registrar(ex);
                }
            }
            var usuarioLogado = servicoUsuario.ObterUsuarioLogado().Result;
            EnviarNotificacaoRegistroDeRecorrencia(evento, notificacoesSucesso, notificacoesFalha, usuarioLogado.Id);
        }

        private static void AtribuirNullSeVazio(Evento evento)
        {
            if (string.IsNullOrWhiteSpace(evento.DreId))
                evento.DreId = null;

            if (string.IsNullOrWhiteSpace(evento.UeId))
                evento.UeId = null;
        }

        private Evento AlterarEventoDeRecorrencia(Evento evento, Evento eventoASerAlterado)
        {
            eventoASerAlterado.Descricao = evento.Descricao;
            eventoASerAlterado.DreId = evento.DreId;
            eventoASerAlterado.FeriadoId = evento.FeriadoId;
            eventoASerAlterado.Letivo = evento.Letivo;
            eventoASerAlterado.Nome = evento.Nome;
            eventoASerAlterado.TipoCalendarioId = evento.TipoCalendarioId;
            eventoASerAlterado.TipoEventoId = evento.TipoEventoId;
            eventoASerAlterado.UeId = evento.UeId;
            return eventoASerAlterado;
        }

        private void EnviarNotificacaoRegistroDeRecorrencia(Evento evento, List<DateTime> notificacoesSucesso, List<string> notificacoesFalha, long usuarioId)
        {
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(evento.TipoCalendarioId);

            var mensagemNotificacao = new StringBuilder();
            if (notificacoesSucesso.Any())
            {
                var textoInicial = notificacoesSucesso.Count > 1 ? "Foram" : "Foi";
                mensagemNotificacao.Append($"<br>{textoInicial} cadastrado(s) {notificacoesSucesso.Count} evento(s) de '{evento.TipoEvento.Descricao}' no calendário '{tipoCalendario.Nome}' de {tipoCalendario.AnoLetivo} nas seguintes datas:<br>");
                notificacoesSucesso.ForEach(data => mensagemNotificacao.AppendLine($"<br>{data.ToShortDateString()}"));
            }
            if (notificacoesFalha.Any())
            {
                mensagemNotificacao.AppendLine($"<br>Não foi possível cadastrar o(s) evento(s) na(s) seguinte(s) data(s)<br>");
                notificacoesFalha.ForEach(mensagem => mensagemNotificacao.AppendLine($"<br>{mensagem}"));
            }
            var notificacao = new Notificacao()
            {
                Titulo = $"Criação de Eventos Recorrentes - {evento.Nome}",
                Mensagem = mensagemNotificacao.ToString(),
                UsuarioId = usuarioId,
                Tipo = NotificacaoTipo.Calendario,
                Categoria = NotificacaoCategoria.Aviso
            };
            servicoNotificacao.Salvar(notificacao);
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

        private async Task PersistirWorkflowEvento(Evento evento)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var escola = await repositorioAbrangencia.ObterUe(evento.UeId, loginAtual, perfilAtual);

            if (escola == null)
                throw new NegocioException($"Não foi possível localizar a escola da criação do evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/:{evento.Id}/";

            var wfAprovacaoEvento = new WorkflowAprovacaoDto()
            {
                Ano = evento.DataInicio.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = evento.Id,
                Tipo = WorkflowAprovacaoTipo.Evento,
                UeId = evento.UeId,
                DreId = evento.DreId,
                NotificacaoTitulo = "Criação de Eventos Excepcionais",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} foi criado no calendário {evento.TipoCalendario.Nome} da {escola.Nome}. Para que este evento seja considerado válido, você precisa aceitar esta notificação. Para visualizar o evento clique <a href='{linkParaEvento}'>aqui</a>."
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Diretor,
                Nivel = 1
            });
            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 2
            });

            var idWorkflow = comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);

            evento.EnviarParaWorkflowDeAprovacao(idWorkflow);

            repositorioEvento.Salvar(evento);
        }

        private async Task SalvarListaEventos(IEnumerable<Evento> eventos, List<long> feriadosErro)
        {
            foreach (var evento in eventos)
            {
                try
                {
                    await repositorioEvento.SalvarAsync(evento);
                }
                catch (Exception)
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

        private void ValidaLiberacaoExcepcional(Evento evento, Usuario usuario, IEnumerable<PeriodoEscolar> periodos, bool dataConfirmada)
        {
            evento.PodeCriarEventoLiberacaoExcepcional(usuario, dataConfirmada, periodos);
        }

        private async Task VerificarParticularidadesSME(Evento evento, Usuario usuario, IEnumerable<PeriodoEscolar> periodos, bool dataConfirmada)
        {
            usuario.PodeCriarEventoComDataPassada(evento);
            evento.PodeCriarEventoOrganizacaoEscolar(usuario);
            await VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(evento, usuario);

            if (evento.TipoEvento.Codigo == (int)TipoEventoEnum.LiberacaoExcepcional)
                ValidaLiberacaoExcepcional(evento, usuario, periodos, dataConfirmada);
        }

        private async Task VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(Evento evento, Usuario usuario)
        {
            var eventos = await repositorioEvento.ObterEventosPorTipoETipoCalendario((long)TipoEventoEnum.OrganizacaoEscolar, evento.TipoCalendarioId);
            evento.VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(eventos, usuario);
        }
    }
}