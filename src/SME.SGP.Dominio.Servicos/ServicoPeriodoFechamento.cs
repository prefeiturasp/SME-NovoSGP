﻿using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPeriodoFechamento : IServicoPeriodoFechamento
    {
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioEventoTipo repositorioTipoEvento;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IServicoEol servicoEol;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;


        public ServicoPeriodoFechamento(IRepositorioPeriodoFechamento repositorioFechamento,
                                 IRepositorioPeriodoFechamentoBimestre repositorioPeriodoFechamentoBimestre,
                                 IServicoUsuario servicoUsuario,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                 IRepositorioDreConsulta repositorioDre,
                                 IRepositorioUeConsulta repositorioUe,
                                 IRepositorioEventoFechamento repositorioEventoFechamento,
                                 IRepositorioEvento repositorioEvento,
                                 IRepositorioEventoTipo repositorioTipoEvento,
                                 IServicoEol servicoEol,
                                 IServicoNotificacao servicoNotificacao,
                                 IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioPeriodoFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.repositorioPeriodoFechamentoBimestre = repositorioPeriodoFechamentoBimestre ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamentoBimestre));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioEventoFechamento = repositorioEventoFechamento ?? throw new ArgumentNullException(nameof(repositorioEventoFechamento));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.repositorioTipoEvento = repositorioTipoEvento ?? throw new ArgumentNullException(nameof(repositorioTipoEvento));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento)
        {
            var somenteUe = fechamento.DreId != null && fechamento.DreId > 0;

            unitOfWork.IniciarTransacao();

            AlterarBimestresPeriodosComHierarquiaInferior(fechamento, somenteUe).Wait();

            unitOfWork.PersistirTransacao();
        }

        private async Task AlterarBimestresPeriodosComHierarquiaInferior(PeriodoFechamento fechamento, bool somenteUe)
        {
            var listaPeriodosAlteracao = new List<PeriodoFechamentoBimestre>();
            // Carrega lista de Periodos a alterar
            foreach (var fechamentoBimestre in fechamento.FechamentosBimestre)
            {
                // Obter Lista de PeriodoFechamentoBimestre por Dre e PeriodoEscolar
                var periodosFechamentoBimestre =
                    await repositorioPeriodoFechamentoBimestre.ObterBimestreParaAlteracaoHierarquicaAsync(
                        fechamentoBimestre.PeriodoEscolarId,
                        fechamento.DreId,
                        fechamentoBimestre.InicioDoFechamento,
                        fechamentoBimestre.FinalDoFechamento);

                if (periodosFechamentoBimestre != null && periodosFechamentoBimestre.Any())
                    listaPeriodosAlteracao.AddRange(periodosFechamentoBimestre);
            }

            // Agrupa a lista em PeriodoEscolar (por UE)
            foreach (var periodosFechamentoBimestreUE in listaPeriodosAlteracao.GroupBy(a => a.PeriodoFechamentoId))
            {
                var periodoFechamento = listaPeriodosAlteracao.Select(a => a.PeriodoFechamento).FirstOrDefault(c => c.Id == periodosFechamentoBimestreUE.Key);

                // Atualiza os periodos bimestre alterados
                foreach (var periodoFechamentoBimestreUe in periodosFechamentoBimestreUE)
                {
                    try
                    {
                        var periodoFechamentoBimestreDre = fechamento.FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolarId == periodoFechamentoBimestreUe.PeriodoEscolarId);
                        if (periodoFechamentoBimestreDre != null)
                        {
                            AtualizaDatasInicioEFim(periodoFechamentoBimestreDre, periodoFechamentoBimestreUe);
                            await repositorioPeriodoFechamentoBimestre.SalvarAsync(periodoFechamentoBimestreUe);

                            EventoFechamento fechamentoExistente = await mediator.Send(new ObterEventoFechamenoPorIdQuery(periodoFechamentoBimestreUe.Id));
                            
                            if (fechamentoExistente != null)
                                AtualizaEventoDeFechamento(periodoFechamentoBimestreUe, fechamentoExistente);

                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                // Notifica Alteração dos Periodos
                if (periodoFechamento.UeId.HasValue)
                    await EnviaNotificacaoParaUe(periodosFechamentoBimestreUE, periodoFechamento.UeId.Value);
                else
                    await EnviaNotificacaoParaDre(periodoFechamento.DreId.Value, periodosFechamentoBimestreUE);
            }
        }

        private void AtualizaDatasInicioEFim(PeriodoFechamentoBimestre periodoFechamentoBimestreDre, PeriodoFechamentoBimestre periodoFechamentoBimestreUe)
        {
            var inicioFechamentoDre = periodoFechamentoBimestreDre.InicioDoFechamento;
            var finalFechamentoDre = periodoFechamentoBimestreDre.FinalDoFechamento;
            var inicioFechamentoUe = periodoFechamentoBimestreUe.InicioDoFechamento;
            var finalFechamentoUe = periodoFechamentoBimestreUe.FinalDoFechamento;

            if (inicioFechamentoDre > inicioFechamentoUe || inicioFechamentoUe > finalFechamentoDre)
                periodoFechamentoBimestreUe.InicioDoFechamento = periodoFechamentoBimestreDre.InicioDoFechamento;

            if (finalFechamentoDre < finalFechamentoUe || finalFechamentoUe < periodoFechamentoBimestreUe.InicioDoFechamento)
                periodoFechamentoBimestreUe.FinalDoFechamento = periodoFechamentoBimestreDre.FinalDoFechamento;
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId)
        {
            return await ObterPorTipoCalendarioSme(tipoCalendarioId);
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioSme(long tipoCalendarioId)
        {
            var fechamentoSME = repositorioPeriodoFechamento.ObterPorFiltros(tipoCalendarioId, null);
            DateTime periodoInicio, periodoFim;            

            if (fechamentoSME == null)
            {
                LimparCamposNaoUtilizadosRegistroPai(fechamentoSME);
                fechamentoSME = new PeriodoFechamento();

                var tipoCalendario = await repositorioTipoCalendario.ObterPorIdAsync(tipoCalendarioId);
                if (tipoCalendario == null)
                    throw new NegocioException("Tipo de calendário não encontrado.");

                var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
                if (periodosEscolares == null || !periodosEscolares.Any())
                    throw new NegocioException("Período escolar não encontrado.");

                foreach (var periodo in periodosEscolares)
                {
                    periodoInicio = periodo.PeriodoInicio;
                    periodoFim = periodo.PeriodoFim;

                    periodo.AdicionarTipoCalendario(tipoCalendario);

                    if (periodoInicio == null || periodoFim == null)
                    {
                        fechamentoSME.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamentoSME.Id, periodo, null, null));
                    }
                    else
                    {
                        fechamentoSME.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamentoSME.Id, periodo, periodoInicio, periodoFim));
                    }
                }
            }

            return MapearParaDto(fechamentoSME);
        }


        private void LimparCamposNaoUtilizadosRegistroPai(PeriodoFechamento registroFilho)
        {
            if (registroFilho != null && registroFilho.Id > 0)
            {
                registroFilho.Id = 0;
                registroFilho.CriadoEm = DateTime.MinValue;
                registroFilho.CriadoPor = null;
                registroFilho.CriadoRF = null;
                registroFilho.AlteradoEm = DateTime.MinValue;
                registroFilho.AlteradoPor = null;
                registroFilho.AlteradoRF = null;
            }
        }


        public async Task Salvar(FechamentoDto fechamentoDto)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var fechamento = MapearParaDominio(fechamentoDto);

            unitOfWork.IniciarTransacao();
            var id = repositorioPeriodoFechamento.Salvar(fechamento);
            repositorioPeriodoFechamento.SalvarBimestres(fechamento.FechamentosBimestre, id);
            unitOfWork.PersistirTransacao();
            await CriarEventoFechamento(fechamento);
        }

        private static Notificacao MontaNotificacao(string nomeEntidade, string tipoEntidade, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, string codigoUe, string codigoDre)
        {
            var mensagem = new StringBuilder();
            mensagem.AppendLine($"A { tipoEntidade} realizou alterações em datas de abertura do período de fechamento de bimestre e as datas definidas pela ");
            mensagem.Append($"{nomeEntidade} foram ajustadas.");
            mensagem.AppendLine("<br> As novas datas são: <br><br>");

            foreach (var bimestre in fechamentosBimestre.OrderBy(a => a.PeriodoEscolar.Bimestre))
            {
                mensagem.AppendLine($"{ bimestre.PeriodoEscolar.TipoCalendario.Nome } - { bimestre.PeriodoEscolar.TipoCalendario.AnoLetivo }");
                mensagem.Append($" {bimestre.PeriodoEscolar.Bimestre}º Bimestre - ");
                mensagem.Append($"Início: {bimestre.InicioDoFechamento.ToString("dd/MM/yyyy")} - ");
                mensagem.Append($"Fim: {bimestre.FinalDoFechamento.ToString("dd/MM/yyyy")}<br>");
            }

            var notificacao = new Notificacao()
            {
                UeId = codigoUe,
                Ano = fechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendario.AnoLetivo,
                Categoria = NotificacaoCategoria.Alerta,
                DreId = codigoDre,
                Titulo = "Alteração em datas de fechamento de bimestre",
                Tipo = NotificacaoTipo.Calendario,
                Mensagem = mensagem.ToString()
            };
            return notificacao;
        }

        private void AtualizaEventoDeFechamento(PeriodoFechamentoBimestre bimestre, EventoFechamento fechamentoExistente)
        {
            var eventoExistente = fechamentoExistente.Evento ?? repositorioEvento.ObterPorId(fechamentoExistente.EventoId);
            if (eventoExistente != null)
            {
                eventoExistente.DataInicio = bimestre.InicioDoFechamento;
                eventoExistente.DataFim = bimestre.FinalDoFechamento;
                eventoExistente.Excluido = false;
                repositorioEvento.Salvar(eventoExistente);
            }
        }

        private void CriaEventoDeFechamento(PeriodoFechamento fechamento, EventoTipo tipoEvento, PeriodoFechamentoBimestre bimestre)
        {
            var evento = new Evento()
            {
                DataInicio = bimestre.InicioDoFechamento,
                DataFim = bimestre.FinalDoFechamento,
                DreId = fechamento.Dre?.CodigoDre,
                UeId = fechamento.Ue?.CodigoUe,
                Nome = $"Fechamento do {bimestre.PeriodoEscolar?.Bimestre}º bimestre",
                TipoEventoId = tipoEvento.Id,
                TipoCalendarioId = bimestre.PeriodoEscolar.TipoCalendarioId
            };
            var eventoId = repositorioEvento.Salvar(evento);
            repositorioEventoFechamento.Salvar(new EventoFechamento()
            {
                FechamentoId = bimestre.Id,
                EventoId = eventoId
            });
        }

        private async Task CriarEventoFechamento(PeriodoFechamento fechamento)
        {
            var tipoEvento = repositorioTipoEvento.ObterTipoEventoPorTipo(TipoEvento.FechamentoBimestre);
        
            if (tipoEvento == null)
                throw new NegocioException("Tipo de evento de fechamento de bimestre não encontrado na base de dados.");

                foreach (var bimestre in fechamento.FechamentosBimestre)
                {
                    EventoFechamento fechamentoExistente = await mediator.Send(new ObterEventoFechamenoPorIdQuery(bimestre.Id));

                if (fechamentoExistente != null)
                    AtualizaEventoDeFechamento(bimestre, fechamentoExistente);
                
                else
                    CriaEventoDeFechamento(fechamento, tipoEvento, bimestre);
            }
        }

        private async Task EnviaNotificacaoParaDre(long dreId, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre)
        {
            var dre = repositorioDre.ObterPorId(dreId);
            if (dre != null)
            {

                Notificacao notificacao = MontaNotificacao(dre.Nome, "SME", fechamentosBimestre, null, dre.CodigoDre);
                var adminsSgpDre = servicoEol.ObterAdministradoresSGPParaNotificar(dre.CodigoDre).Result;
                if (adminsSgpDre != null && adminsSgpDre.Any())
                {
                    foreach (var adminSgpUe in adminsSgpDre)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                        notificacao.UsuarioId = usuario.Id;

                        await servicoNotificacao.Salvar(notificacao);
                    }
                }
            }
        }

        private async Task EnviaNotificacaoParaUe(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long UeId)
        {

            var ue = repositorioUe.ObterPorId(UeId);
            if (ue != null)
            {
                var nomeUe = $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}";

                Notificacao notificacao = MontaNotificacao(nomeUe, "DRE", fechamentosBimestre, ue.CodigoUe, null);
                var diretores = await mediator.Send(
                    new ObterFuncionariosPorCargoUeQuery(ue.CodigoUe, (long)Cargo.Diretor));
                if (diretores == null || !diretores.Any())
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foram localizados diretores para Ue {ue.CodigoUe} para Enviar notificação para a UE.", LogNivel.Negocio, LogContexto.Fechamento));
                }
                else
                {
                    foreach (var diretor in diretores)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                        notificacao.UsuarioId = usuario.Id;

                        await servicoNotificacao.Salvar(notificacao);
                    }
                }

                var admsUe = await servicoEol.ObterAdministradoresSGPParaNotificar(ue.CodigoUe);

                if (admsUe == null || !admsUe.Any())
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foram localizados os ADMs para Ue {ue.CodigoUe}.", LogNivel.Negocio, LogContexto.Fechamento));
                else
                {
                    var usuarios = await mediator.Send(new ObterUsuariosPorRfOuCriaQuery(admsUe, true));

                    foreach (var usuario in usuarios.Where(u => u.PossuiPerfilAdmUE()))
                    {
                        notificacao.UsuarioId = usuario.Id;

                        await servicoNotificacao.Salvar(notificacao);
                    }
                }
            }

        }

        private IEnumerable<FechamentoBimestreDto> MapearFechamentoBimestreParaDto(PeriodoFechamento fechamento)
        {
            var listaFechamentoBimestre = new List<FechamentoBimestreDto>();
            foreach (var fechamentoBimestre in fechamento.FechamentosBimestre)
            {
                listaFechamentoBimestre.Add(new FechamentoBimestreDto
                {
                    InicioDoFechamento = fechamentoBimestre.InicioDoFechamento != DateTime.MinValue ? fechamentoBimestre.InicioDoFechamento : (DateTime?)null,
                    FinalDoFechamento = fechamentoBimestre.FinalDoFechamento != DateTime.MinValue ? fechamentoBimestre.FinalDoFechamento : (DateTime?)null,
                    Bimestre = fechamentoBimestre.PeriodoEscolar.Bimestre,
                    Id = fechamentoBimestre.Id,
                    PeriodoEscolarId = fechamentoBimestre.PeriodoEscolarId,
                    PeriodoEscolar = fechamentoBimestre.PeriodoEscolar,
                    InicioMinimo = new DateTime(fechamentoBimestre.PeriodoEscolar.PeriodoInicio.Year, 01, 01),
                    FinalMaximo = new DateTime(fechamentoBimestre.PeriodoEscolar.PeriodoInicio.Year, 12, 31)
            });
            }
            return listaFechamentoBimestre;
        }

        private PeriodoFechamento MapearParaDominio(FechamentoDto fechamentoDto)
        {
            var fechamento = repositorioPeriodoFechamento.ObterPorFiltros(fechamentoDto.TipoCalendarioId.Value, null);
            if (fechamento == null)
                fechamento = new PeriodoFechamento();

            var tipoCalendario = repositorioTipoCalendario.ObterPorId(fechamentoDto.TipoCalendarioId.Value);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo calendário não encontrado.");
            }

            if (fechamentoDto.FechamentosBimestres != null && fechamentoDto.FechamentosBimestres.Any())
            {
                foreach (var bimestre in fechamentoDto.FechamentosBimestres)
                {
                    var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(bimestre.PeriodoEscolarId);
                    PeriodoFechamentoBimestre fechamentoBimestreExistente = fechamento.ObterFechamentoBimestre(bimestre.PeriodoEscolarId);
                    if (fechamentoBimestreExistente != null)
                    {
                        var periodo = new PeriodoFechamentoBimestre(fechamento.Id, periodoEscolar, bimestre.InicioDoFechamento, bimestre.FinalDoFechamento);
                        fechamento.ValidarPeriodoInicioFim(periodo);
                        fechamento.ValidarPeriodoConcomitante(periodo);
                        fechamentoBimestreExistente.AtualizarDatas(bimestre.InicioDoFechamento, bimestre.FinalDoFechamento);
                    }
                    else
                        fechamento.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamento.Id, periodoEscolar, bimestre.InicioDoFechamento, bimestre.FinalDoFechamento));

                    bimestre.PeriodoEscolar.TipoCalendario = tipoCalendario;
                }
            }
            return fechamento;
        }

        private FechamentoDto MapearParaDto(PeriodoFechamento fechamento)
        {
            return fechamento == null ? null : new FechamentoDto
            {
                Id = fechamento.Id,
                DreId = fechamento.Dre?.CodigoDre,
                TipoCalendarioId = fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId,
                UeId = fechamento.Ue?.CodigoUe,
                FechamentosBimestres = MapearFechamentoBimestreParaDto(fechamento).OrderBy(c => c.Bimestre),
                AlteradoEm = fechamento.AlteradoEm,
                AlteradoPor = fechamento.AlteradoPor,
                AlteradoRF = fechamento.AlteradoRF,
                CriadoEm = fechamento.CriadoEm,
                CriadoPor = fechamento.CriadoPor,
                CriadoRF = fechamento.CriadoRF,
                Migrado = fechamento.Migrado
            };
        }

        private (Dre, Ue) ObterDreEUe(string codigoDre, string codigoUe)
        {
            Dre dre = null;
            if (!string.IsNullOrWhiteSpace(codigoDre))
            {
                dre = repositorioDre.ObterPorCodigo(codigoDre.ToString());
            }
            Ue ue = null;
            if (!string.IsNullOrWhiteSpace(codigoUe))
            {
                ue = repositorioUe.ObterPorCodigo(codigoUe.ToString());
            }
            return (dre, ue);
        }
    }
}