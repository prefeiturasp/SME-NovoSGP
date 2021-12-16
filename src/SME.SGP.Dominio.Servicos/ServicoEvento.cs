using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
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
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoLog servicoLog;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioEventoBimestre repositorioEventoBimestre;

        public ServicoEvento(IRepositorioEvento repositorioEvento,
                             IRepositorioEventoTipo repositorioEventoTipo,
                             IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                             IServicoUsuario servicoUsuario,
                             IRepositorioFeriadoCalendario repositorioFeriadoCalendario,
                             IRepositorioTipoCalendario repositorioTipoCalendario,
                             IComandosWorkflowAprovacao comandosWorkflowAprovacao,
                             IRepositorioAbrangencia repositorioAbrangencia, IConfiguration configuration,
                             IUnitOfWork unitOfWork, IServicoNotificacao servicoNotificacao, IServicoLog servicoLog, IMediator mediator,
                             IRepositorioEventoBimestre repositorioEventoBimestre)
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
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEventoBimestre = repositorioEventoBimestre ?? throw new ArgumentNullException(nameof(repositorioEventoBimestre));
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

        public async Task<string> Salvar(Evento evento, int[] bimestres = null, bool alterarRecorrenciaCompleta = false, bool dataConfirmada = false, bool unitOfWorkJaEmUso = false)
        {

            if ((TipoEvento)evento.TipoEventoId == TipoEvento.LiberacaoBoletim && bimestres.Length == 0)
                throw new NegocioException("Ao menos um bimestre deve ser selecionado para um evento do tipo Liberação do Boletim");

            ObterTipoEvento(evento);

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

            var periodos = await repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);

            if (evento.DeveSerEmDiaLetivo())
                evento.EstaNoPeriodoLetivo(periodos);

            bool devePassarPorWorkflowLiberacaoExcepcional = await ValidaDatasETiposDeEventos(evento, dataConfirmada, usuario, periodos);

            AtribuirNullSeVazio(evento);



            if (!unitOfWorkJaEmUso)
                unitOfWork.IniciarTransacao();

            await repositorioEvento.SalvarAsync(evento);

            if ((TipoEvento)evento.TipoEventoId == TipoEvento.LiberacaoBoletim)
                await IncluiBimestresDoEventoLiberacaoDeBoletim(evento, bimestres, ehAlteracao);


            // Envia para workflow apenas na Inclusão ou alteração apos aprovado
            var enviarParaWorkflow = !string.IsNullOrWhiteSpace(evento.UeId) && devePassarPorWorkflowLiberacaoExcepcional;
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

            // Verifica existencia de pendencia de calendario com dias letivos insuficientes
            if (evento.EhEventoDRE() && evento.EhEventoUE())
                await VerificaPendenciaDiasLetivosInsuficientes(ehAlteracao, enviarParaWorkflow, evento, usuario);

            if (evento.EhEventoUE())
                await VerificaPendenciaParametroEvento(evento, usuario);

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

        private async Task IncluiBimestresDoEventoLiberacaoDeBoletim(Evento evento, int[] bimestres, bool ehAlteracao)
        {
            if (!ehAlteracao)
                await ValidaSeExisteOutroEventoLiberacaoDeBoletimNaMesmaDataEMesmoTipoCalendario(evento);

            var bimestresDoTipoCalendarioJaCadastrados = await repositorioEventoBimestre.ObterBimestresPorTipoCalendarioDeOutrosEventos(evento.TipoCalendarioId, evento.Id);

            if (bimestresDoTipoCalendarioJaCadastrados.Length == 0 && !ehAlteracao)
            {
                await IncluiBimestresNoEventoDeLiberacaoDeBoletim(evento, bimestres);
                return;
            }

            if (bimestresDoTipoCalendarioJaCadastrados.Length > 0 && bimestresDoTipoCalendarioJaCadastrados.Contains((int)Bimestre.Todos))
                throw new NegocioException("Já existe outro evento do tipo liberação do boletim com todos os bimestres cadastrado para esse calendário.");

            if (bimestresDoTipoCalendarioJaCadastrados.Length > 0)
                VerificaSeOsBimestresJaExistemParaOCalendarioDoEvento(bimestres, bimestresDoTipoCalendarioJaCadastrados);

            await AlteraBimestresDoEvento(evento, bimestres);

        }

        private async Task ValidaSeExisteOutroEventoLiberacaoDeBoletimNaMesmaDataEMesmoTipoCalendario(Evento evento)
        {
            var jaExisteOutroEvento = await repositorioEventoBimestre.VerificaSeExiteEventoPorTipoCalendarioDataReferencia(evento.TipoCalendarioId, evento.DataInicio.Date);
            if (jaExisteOutroEvento)
                throw new NegocioException($"Já existe outro evento do tipo liberação do boletim com a data {evento.DataInicio.Date} cadastrada para esse calendário.");
        }

        private async Task AlteraBimestresDoEvento(Evento evento, int[] bimestres)
        {
            await repositorioEventoBimestre.ExcluiEventoBimestre(evento.Id);
            await IncluiBimestresNoEventoDeLiberacaoDeBoletim(evento, bimestres);
        }

        private void VerificaSeOsBimestresJaExistemParaOCalendarioDoEvento(int[] bimestres, int[] bimestresDoTipoCalendarioJaCadastrados)
        {
            List<string> BimestreDoEventoqueJaEstaCadastrado = ComparaListaDeBimestresERetornaOsQueJaEstaoCadastrados(bimestres, bimestresDoTipoCalendarioJaCadastrados);

            if (BimestreDoEventoqueJaEstaCadastrado.Count > 0)
            {
                string bimestresFormatados = string.Join(",", BimestreDoEventoqueJaEstaCadastrado);

                if (BimestreDoEventoqueJaEstaCadastrado.Count > 1)
                    throw new NegocioException($"Os bimestres {bimestresFormatados}  já estão cadastrados em outro evento para esse calendário");
                throw new NegocioException($"O bimestre {bimestresFormatados}  já está cadastrado em outro evento para esse calendário");

            }
        }

        private List<string> ComparaListaDeBimestresERetornaOsQueJaEstaoCadastrados(int[] bimestres, int[] bimestresDoTipoCalendarioJaCadastrados)
        {
            List<string> BimestreDoEventoqueJaEstaCadastrado = new List<string>();

            foreach (int bimestreCadastrado in bimestresDoTipoCalendarioJaCadastrados)
            {
                foreach (int bimestre in bimestres)
                {
                    if (bimestreCadastrado == bimestre)
                    {
                        var bimestreEnum = (Bimestre)bimestre;
                        BimestreDoEventoqueJaEstaCadastrado.Add(bimestreEnum.Name());
                    }

                }
            }

            return BimestreDoEventoqueJaEstaCadastrado;
        }

        private async Task IncluiBimestresNoEventoDeLiberacaoDeBoletim(Evento evento, int[] bimestres)
        {
            foreach (var bimestre in bimestres)
            {
                var eventoBimestre = new EventoBimestre()
                {
                    EventoId = evento.Id,
                    Bimestre = bimestre
                };
                await repositorioEventoBimestre.SalvarAsync(eventoBimestre);
            }
        }

        private async Task VerificaPendenciaParametroEvento(Evento evento, Usuario usuario)
        {
            if (evento.GeraPendenciaParametroEvento())
                await mediator.Send(new IncluirFilaVerificaExclusaoPendenciasParametroEventoCommand(evento.TipoCalendarioId, evento.UeId, (TipoEvento)evento.TipoEventoId, usuario));
        }

        private async Task VerificaPendenciaDiasLetivosInsuficientes(bool ehAlteracao, bool enviarParaWorkflow, Evento evento, Usuario usuario)
        {
            if (!enviarParaWorkflow && !ehAlteracao && evento.EhEventoLetivo())
                await mediator.Send(new IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand(evento.TipoCalendarioId, evento.DreId, evento.UeId, usuario));
        }


        public void SalvarEventoFeriadosAoCadastrarTipoCalendario(TipoCalendario tipoCalendario)
        {
            var feriados = ObterEValidarFeriados(tipoCalendario.AnoLetivo);

            var tipoEventoFeriado = ObterEValidarTipoEventoFeriado();

            var eventos = feriados
                .Select(x => MapearEntidade(tipoCalendario, x, tipoEventoFeriado));

            var feriadosErro = new List<long>();

            SalvarListaEventos(eventos, feriadosErro);

            if (feriadosErro.Any())
                TratarErros(feriadosErro);
        }

        public async Task SalvarRecorrencia(Evento evento, DateTime dataInicial, DateTime? dataFinal, int? diaDeOcorrencia, IEnumerable<DayOfWeek> diasDaSemana, PadraoRecorrencia padraoRecorrencia, PadraoRecorrenciaMensal? padraoRecorrenciaMensal, int repeteACada)
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
                var periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendario(evento.TipoCalendarioId);
                if (periodoEscolar == null || !periodoEscolar.Any())
                {
                    throw new NegocioException("Não é possível cadastrar o evento pois não existe período escolar cadastrado para este calendário.");
                }
                var periodoFinal = periodoEscolar.OrderByDescending(p => p.Bimestre).FirstOrDefault();
                dataFinal = periodoFinal.PeriodoFim;
            }
            var eventos = evento.ObterRecorrencia(padraoRecorrencia, padraoRecorrenciaMensal, dataInicial, dataFinal.Value, diasDaSemana, repeteACada, diaDeOcorrencia);
            var notificacoesSucesso = new List<DateTime>();
            var notificacoesFalha = new List<string>();
            foreach (var novoEvento in eventos)
            {
                try
                {
                    if (!await ValidaCadastroEvento(novoEvento.DataInicio,
                            novoEvento.TipoCalendarioId,
                            novoEvento.Letivo == EventoLetivo.Sim,
                            novoEvento.TipoEventoId))
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

        private async Task<bool> ValidaCadastroEvento(DateTime dataInicio, long tipoCalendarioId, bool ehLetivo, long tipoEventoId)
        {
            var periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioData(tipoCalendarioId, dataInicio, dataInicio);
            if (periodoEscolar == null)
                return false;

            if (ehLetivo && tipoEventoId != (int)TipoEvento.LiberacaoExcepcional)
              return ValidaSeEhFinalSemana(dataInicio, dataInicio);

            return true;
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

        private async Task<long> CriarWorkflowParaEventoExcepcionais(Evento evento, Dto.AbrangenciaUeRetorno escola, string linkParaEvento)
        {
            var tipoCalendario = evento.TipoCalendario ?? ObterTipoCalendario(evento);

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
                NotificacaoMensagem = $"O evento {evento.Nome} - {evento.DataInicio.Day}/{evento.DataInicio.Month}/{evento.DataInicio.Year} foi criado no calendário {tipoCalendario.Nome} da {escola.NomeSimples}. Para que este evento seja considerado válido, você precisa aceitar esta notificação. Para visualizar o evento clique <a href='{linkParaEvento}'>aqui</a>."
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

            return await comandosWorkflowAprovacao.Salvar(wfAprovacaoEvento);
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

        private Evento MapearEntidade(TipoCalendario tipoCalendario, FeriadoCalendario x, EventoTipo tipoEventoFeriado)
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

        private IEnumerable<FeriadoCalendario> ObterEValidarFeriados(int anoReferencia)
        {
            var feriadosMoveis = repositorioFeriadoCalendario
                .ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto 
                { 
                    Ano = anoReferencia, 
                    Tipo = TipoFeriadoCalendario.Movel 
                }).Result;
            
            var feriadosFixos = repositorioFeriadoCalendario
                .ObterFeriadosCalendario(new FiltroFeriadoCalendarioDto 
                { 
                    Tipo = TipoFeriadoCalendario.Fixo
                }).Result;

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
                idWorkflow = await CriarWorkflowParaEventoExcepcionais(evento, escola, linkParaEvento);

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
                                throw new NegocioException($"O tipo de evento '{((TipoEvento)evento.TipoEvento.Codigo).ObterAtributo<DisplayAttribute>().Name}' não pode ser cadastrado no recesso.");
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
                            if ((temEventoFeriado || temEventoSuspensaoAtividades || evento.DataInicio.DayOfWeek == DayOfWeek.Sunday || evento.DataFim.DayOfWeek == DayOfWeek.Sunday) && evento.Letivo == EventoLetivo.Sim)
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
                                else throw new NegocioException($"O tipo de evento '{((TipoEvento)evento.TipoEvento.Codigo).ObterAtributo<DisplayAttribute>().Name}' não pode ser cadastrado fora do período escolar.");
                            }
                        }
                    }
                }
            }

            return devePassarPorWorkflow;
        }

        public async Task Excluir(Evento evento)
        {
            evento.Excluir();
            repositorioEvento.Salvar(evento);

            await VerificaPendenciaDiasLetivosInsuficientesNaExclusao(evento);
        }

        private async Task VerificaPendenciaDiasLetivosInsuficientesNaExclusao(Evento evento)
        {
            if (evento.EhEventoNaoLetivo())
            {
                var usuario = await servicoUsuario.ObterUsuarioLogado();

                await mediator.Send(new IncluirFilaExcluirPendenciasDiasLetivosInsuficientesCommand(evento.TipoCalendarioId, evento.DreId, evento.UeId, usuario));
            }
        }

        private bool ValidaSeEhFinalSemana(DateTime inicio, DateTime fim)
        {
            for (DateTime data = inicio; data <= fim; data = data.AddDays(1))
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    return false;
            return true;
        }
    }
}