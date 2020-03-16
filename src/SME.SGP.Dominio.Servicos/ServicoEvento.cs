using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public async Task<string> Salvar(Evento evento, bool alterarRecorrenciaCompleta = false, bool dataConfirmada = false, bool unitOfWorkJaEmUso = false)
        {
            ObterTipoEvento(evento);

            TipoCalendario tipoCalendario = ObterTipoCalendario(evento);

            evento.ValidaPeriodoEvento();

            var usuario = await servicoUsuario.ObterUsuarioLogado();

            bool ehAlteracao = true;

            if (evento.Id == 0)
            {
                ehAlteracao = false;
                evento.TipoPerfilCadastro = usuario.ObterTipoPerfilAtual();
            }
            else
            {
                var entidadeNaoModificada = repositorioEvento.ObterPorId(evento.Id);
                ObterTipoEvento(entidadeNaoModificada);
                usuario.PodeAlterarEvento(entidadeNaoModificada);
            }

            usuario.PodeCriarEvento(evento);

            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

            if (evento.DeveSerEmDiaLetivo())
                evento.EstaNoPeriodoLetivo(periodos);

            bool devePassarPorWorkflowLiberacaoExcepcional = await ValidaDatasETiposDeEventos(evento, dataConfirmada, usuario, periodos);

            AtribuirNullSeVazio(evento);

            if (!unitOfWorkJaEmUso)
                unitOfWork.IniciarTransacao();

            repositorioEvento.Salvar(evento);

            // Envia para workflow apenas na Inclusão ou alteração apos aprovado
            var enviarParaWorkflow = !string.IsNullOrWhiteSpace(evento.UeId) && (devePassarPorWorkflowLiberacaoExcepcional && evento.TipoEvento.Codigo != (long)TipoEvento.LiberacaoExcepcional);
            if (!ehAlteracao || (evento.Status == EntidadeStatus.Aprovado))
            {
                if (enviarParaWorkflow)
                    await PersistirWorkflowEvento(evento, devePassarPorWorkflowLiberacaoExcepcional);
            }

            if (!unitOfWorkJaEmUso)
                unitOfWork.PersistirTransacao();

            if (evento.EventoPaiId.HasValue && evento.EventoPaiId > 0 && alterarRecorrenciaCompleta)
            {
                SME.Background.Core.Cliente.Executar(() => AlterarRecorrenciaEventos(evento, alterarRecorrenciaCompleta));
            }

            if (ehAlteracao)
            {
                if (enviarParaWorkflow)
                    return "Evento alterado e será válido após aprovação.";
                else return "Evento alterado com sucesso.";
            }
            else
            {
                if (enviarParaWorkflow)
                    return "Evento cadastrado e será válido após aprovação.";
                else return "Evento cadastrado com sucesso.";
            }
        }

        public void SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario)
        {
            var feriados = ObterEValidarFeriados();

            var tipoEventoFeriado = ObterEValidarTipoEventoFeriado();

            var eventos = feriados.Select(x => MapearEntidade(tipoCalendario, x, tipoEventoFeriado));

            var feriadosErro = new List<long>();

            SalvarListaEventos(eventos, feriadosErro);

            if (feriadosErro.Any())
                TratarErros(feriadosErro);
        }

        public void SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada)
        {
            if (evento.DataInicio.Date != evento.DataFim.Date)
            {
                throw new NegocioException("A recorrência somente é permitida quando o evento possui data única.");
            }

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

        private long CriarWorkflowParaDataPassada(Evento evento, AbrangenciaUeRetorno escola, string linkParaEvento)
        {
            var wfAprovacaoEvento = new WorkflowAprovacaoDto()
            {
                Ano = evento.DataInicio.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = evento.Id,
                Tipo = WorkflowAprovacaoTipo.Evento_Data_Passada,
                UeId = evento.UeId,
                DreId = evento.DreId,
                NotificacaoTitulo = "Criação de evento com data passada",
                NotificacaoTipo = NotificacaoTipo.Calendario,
                NotificacaoMensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} foi criado no calendário {evento.TipoCalendario.Nome} da {escola.Nome}. Para que este evento seja considerado válido, você precisa aceitar esta notificação. Para visualizar o evento clique <a href='{linkParaEvento}'>aqui</a>."
            };

            wfAprovacaoEvento.Niveis.Add(new WorkflowAprovacaoNivelDto()
            {
                Cargo = Cargo.Supervisor,
                Nivel = 1
            });

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }

        private long CriarWorkflowParaEventoExcepcionais(Evento evento, Dto.AbrangenciaUeRetorno escola, string linkParaEvento)
        {
            var wfAprovacaoEvento = new WorkflowAprovacaoDto()
            {
                Ano = evento.DataInicio.Year,
                NotificacaoCategoria = NotificacaoCategoria.Workflow_Aprovacao,
                EntidadeParaAprovarId = evento.Id,
                Tipo = WorkflowAprovacaoTipo.Evento_Liberacao_Excepcional,
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

            return comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
        }

        private void EnviarNotificacaoRegistroDeRecorrencia(Evento evento, List<DateTime> notificacoesSucesso, List<string> notificacoesFalha, long usuarioId)
        {
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(evento.TipoCalendarioId);

            var mensagemNotificacao = new StringBuilder();
            if (notificacoesSucesso.Any())
            {
                var textoInicial = notificacoesSucesso.Count > 1 ? "Foram" : "Foi";
                mensagemNotificacao.Append($"<br>{textoInicial} cadastrado(s) {notificacoesSucesso.Count} evento(s) de '{evento.TipoEvento.Descricao}' no calendário '{tipoCalendario.Nome}' de {tipoCalendario.AnoLetivo} nas seguintes datas:<br>");
                notificacoesSucesso.ForEach(data => mensagemNotificacao.AppendLine($"<br>{data.ToString("dd/MM/yyyy")}"));
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

        private IEnumerable<FeriadoCalendario> ObterEValidarFeriados()
        {
            var feriadosMoveis = repositorioFeriadoCalendario.ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto { Ano = DateTime.Now.Year, Tipo = TipoFeriadoCalendario.Movel }).Result;
            var feriadosFixos = repositorioFeriadoCalendario.ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto { Tipo = TipoFeriadoCalendario.Fixo }).Result;

            var feriados = feriadosFixos?.ToList();
            feriados?.AddRange(feriadosMoveis);

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

        private TipoCalendario ObterTipoCalendario(Evento evento)
        {
            var tipoCalendario = repositorioTipoCalendario.ObterPorId(evento.TipoCalendarioId);
            if (tipoCalendario == null)
                throw new NegocioException("Calendário não encontrado.");

            evento.AdicionarTipoCalendario(tipoCalendario);
            return tipoCalendario;
        }

        private void ObterTipoEvento(Evento evento)
        {
            var tipoEvento = repositorioEventoTipo.ObterPorId(evento.TipoEventoId);

            if (tipoEvento == null)
                throw new NegocioException("O tipo do evento deve ser informado.");

            evento.AdicionarTipoEvento(tipoEvento);
        }

        private async Task PersistirWorkflowEvento(Evento evento, bool workflowDeLiberacaoExcepcional)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var escola = await repositorioAbrangencia.ObterUe(evento.UeId, loginAtual, perfilAtual);

            if (escola == null)
                throw new NegocioException($"Não foi possível localizar a escola da criação do evento.");

            var linkParaEvento = $"{configuration["UrlFrontEnd"]}calendario-escolar/eventos/editar/{evento.Id}/";

            long idWorkflow = 0;

            if (workflowDeLiberacaoExcepcional)
                idWorkflow = CriarWorkflowParaEventoExcepcionais(evento, escola, linkParaEvento);

            evento.EnviarParaWorkflowDeAprovacao(idWorkflow);

            repositorioEvento.Salvar(evento);
        }

        private void SalvarListaEventos(IEnumerable<Evento> eventos, List<long> feriadosErro)
        {
            foreach (var evento in eventos)
            {
                try
                {
                    repositorioEvento.Salvar(evento);
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

        private async Task<bool> ValidaDatasETiposDeEventos(Evento evento, bool dataConfirmada, Usuario usuario, IEnumerable<PeriodoEscolar> periodos)
        {
            var devePassarPorWorkflow = false;

            if (evento.TipoEvento.Codigo == (int)TipoEvento.FechamentoBimestre)
                return false;

            if (evento.TipoEvento.Codigo == (int)TipoEvento.LiberacaoExcepcional)
            {
                evento.PodeCriarEventoLiberacaoExcepcional(usuario, dataConfirmada, periodos);
            }
            else
            {
                if (evento.EhTipoEventoFechamento())
                {
                    throw new NegocioException("Não é possível criar eventos do tipo selecionado.");
                }

                if (evento.TipoEvento.Codigo == (long)TipoEvento.Recesso || evento.TipoEvento.Codigo == (long)TipoEvento.ReposicaoNoRecesso)
                {
                    return devePassarPorWorkflow;
                }
                else
                {
                    var temEventoLiberacaoExcepcional = await repositorioEvento.TemEventoNosDiasETipo(evento.DataInicio.Date, evento.DataFim.Date, TipoEvento.LiberacaoExcepcional, evento.TipoCalendarioId, evento.UeId, evento.DreId);

                    if (await repositorioEvento.TemEventoNosDiasETipo(evento.DataInicio.Date, evento.DataFim.Date, TipoEvento.ReposicaoNoRecesso, evento.TipoCalendarioId, string.Empty, string.Empty))
                    {
                        if (evento.TipoEvento.LocalOcorrencia == EventoLocalOcorrencia.UE)
                        {
                            if (temEventoLiberacaoExcepcional)
                                return true;

                            if (evento.TipoEvento.Codigo == (long)TipoEvento.ReposicaoDoDia || evento.TipoEvento.Codigo == (long)TipoEvento.ReposicaoDeAula)
                            {
                                return devePassarPorWorkflow;
                            }
                            else
                                throw new NegocioException($"O tipo de evento '{((TipoEvento)evento.TipoEvento.Codigo).GetAttribute<DisplayAttribute>().Name}' não pode ser cadastrado no recesso.");
                        }
                        else return devePassarPorWorkflow;
                    }
                    else
                    {
                        var estaNoPeriodoEscolar = periodos.Any(c => c.PeriodoInicio.Date <= evento.DataInicio.Date && c.PeriodoFim.Date >= evento.DataFim.Date);

                        if (estaNoPeriodoEscolar)
                        {
                            var temEventoSuspensaoAtividades = await repositorioEvento.TemEventoNosDiasETipo(evento.DataInicio.Date, evento.DataFim.Date, TipoEvento.SuspensaoAtividades, evento.TipoCalendarioId, evento.UeId, evento.DreId, escopoRetroativo: true);
                            var temEventoFeriado = await repositorioEvento.TemEventoNosDiasETipo(evento.DataInicio.Date, evento.DataFim.Date, TipoEvento.Feriado, evento.TipoCalendarioId, string.Empty, string.Empty);
                            if ((temEventoFeriado || temEventoSuspensaoAtividades || evento.DataInicio.FimDeSemana() || evento.DataFim.FimDeSemana()) && evento.Letivo == EventoLetivo.Sim)
                            {
                                if (temEventoLiberacaoExcepcional)
                                    return true;
                                else
                                {
                                    if (temEventoFeriado)
                                        throw new NegocioException("Não é possível cadastrar o evento pois há feriado na data selecionada.");
                                    else if (temEventoSuspensaoAtividades)
                                        throw new NegocioException("Não é possível cadastrar o evento pois há evento de suspensão de atividades na data informada.");
                                    else if (evento.DataInicio.DayOfWeek == DayOfWeek.Sunday)
                                        throw new NegocioException("Não é possível cadastrar o evento letivo no domingo.");
                                }
                            }
                        }
                        else
                        {
                            if (evento.TipoEvento.Codigo == (long)TipoEvento.OrganizacaoEscolar || evento.TipoEvento.Codigo == (long)TipoEvento.RecreioNasFerias)
                            {
                                return devePassarPorWorkflow;
                            }
                            else
                            {
                                if (temEventoLiberacaoExcepcional)
                                    return true;
                                else if (evento.TipoEvento.Codigo == (long)TipoEvento.Outros)
                                    return devePassarPorWorkflow;
                                else throw new NegocioException($"O tipo de evento '{((TipoEvento)evento.TipoEvento.Codigo).GetAttribute<DisplayAttribute>().Name}' não pode ser cadastrado fora do período escolar.");
                            }
                        }
                    }
                }
            }

            return devePassarPorWorkflow;
        }

        private async Task VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(Evento evento, Usuario usuario)
        {
            var eventos = await repositorioEvento.ObterEventosPorTipoETipoCalendario((long)TipoEvento.OrganizacaoEscolar, evento.TipoCalendarioId);
            evento.VerificaSeEventoAconteceJuntoComOrganizacaoEscolar(eventos, usuario);
        }
    }
}