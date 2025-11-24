using MediatR;
using SME.SGP.Aplicacao;
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
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                AlterarBimestresPeriodosComHierarquiaInferior(fechamento).Wait();

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task<List<PeriodoFechamentoBimestre>> ObterBimestresParaAlteracaoHierarquica(long? dreId, List<PeriodoFechamentoBimestre> fechamentosBimestre)
        {
            var listaPeriodosAlteracao = new List<PeriodoFechamentoBimestre>();
            // Carrega lista de Periodos a alterar
            foreach (var fechamentoBimestre in fechamentosBimestre)
            {
                // Obter Lista de PeriodoFechamentoBimestre por Dre e PeriodoEscolar
                var periodosFechamentoBimestre =
                    await repositorioPeriodoFechamentoBimestre.ObterBimestreParaAlteracaoHierarquicaAsync(
                        fechamentoBimestre.PeriodoEscolarId,
                        dreId,
                        fechamentoBimestre.InicioDoFechamento,
                        fechamentoBimestre.FinalDoFechamento);
                listaPeriodosAlteracao.AddRange(periodosFechamentoBimestre);
            }
            return listaPeriodosAlteracao;
        }

        private async Task AlterarBimestresPeriodosComHierarquiaInferior(PeriodoFechamento fechamento)
        {
            // Carrega lista de Periodos a alterar
            var listaPeriodosAlteracao = await ObterBimestresParaAlteracaoHierarquica(fechamento.DreId, fechamento.FechamentosBimestre);

            // Agrupa a lista em PeriodoEscolar (por UE)
            foreach (var periodosFechamentoBimestreUE in listaPeriodosAlteracao.GroupBy(a => a.PeriodoFechamentoId))
            {
                var periodoFechamento = listaPeriodosAlteracao.Select(a => a.PeriodoFechamento).FirstOrDefault(c => c.Id == periodosFechamentoBimestreUE.Key);

                // Atualiza os periodos bimestre alterados
                foreach (var periodoFechamentoBimestreUe in periodosFechamentoBimestreUE)
                {
                    var periodoFechamentoBimestreDre = fechamento.FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolarId == periodoFechamentoBimestreUe.PeriodoEscolarId);
                    if (periodoFechamentoBimestreDre.NaoEhNulo())
                    {
                        AtualizaDatasInicioEFim(periodoFechamentoBimestreDre, periodoFechamentoBimestreUe);
                        await repositorioPeriodoFechamentoBimestre.SalvarAsync(periodoFechamentoBimestreUe);

                        EventoFechamento fechamentoExistente = await mediator.Send(new ObterEventoFechamenoPorIdQuery(periodoFechamentoBimestreUe.Id));
                        AtualizaEventoDeFechamento(periodoFechamentoBimestreUe, fechamentoExistente);
                    }
                }
                await EnviarNotificacaoUeDre(periodosFechamentoBimestreUE, periodoFechamento.UeId, periodoFechamento.DreId);
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
            return await ObterPorTipoCalendarioSme(tipoCalendarioId, Aplicacao.SGP);
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioSme(long tipoCalendarioId, Aplicacao aplicacao)
        {
            var fechamentoSME = repositorioPeriodoFechamento.ObterPorFiltros(tipoCalendarioId, null, aplicacao);
            DateTime periodoInicio, periodoFim;

            if (fechamentoSME.EhNulo())
            {
                LimparCamposNaoUtilizadosRegistroPai(fechamentoSME);
                fechamentoSME = new PeriodoFechamento();
                fechamentoSME.Aplicacao = aplicacao;

                if (EhAplicacaoSondagem(aplicacao))
                {
                    for (int ciclo = 1; ciclo <= 5; ciclo++)
                    {
                        var periodo = new PeriodoEscolar
                        {
                            Bimestre = ciclo,
                            PeriodoInicio = new DateTime(DateTime.Now.Year, 01, 01),
                            PeriodoFim = new DateTime(DateTime.Now.Year, 12, 31),
                            TipoCalendarioId = tipoCalendarioId,
                        };

                        fechamentoSME.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamentoSME.Id, periodo, null, null));
                    }

                    return MapearParaDto(fechamentoSME);
                }

                var tipoCalendario = await repositorioTipoCalendario.ObterPorIdAsync(tipoCalendarioId);
                if (tipoCalendario.EhNulo())
                    throw new NegocioException("Tipo de calendário não encontrado.");

                var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
                if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                    throw new NegocioException("Período escolar não encontrado.");

                foreach (var periodo in periodosEscolares)
                {
                    periodoInicio = periodo.PeriodoInicio;
                    periodoFim = periodo.PeriodoFim;

                    periodo.AdicionarTipoCalendario(tipoCalendario);

                    if (periodoInicio.EhNulo() || periodoFim.EhNulo())
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
            if (registroFilho.NaoEhNulo() && registroFilho.Id > 0)
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
            var fechamento = await MapearParaDominioAsync(fechamentoDto);

            unitOfWork.IniciarTransacao();
            var id = repositorioPeriodoFechamento.Salvar(fechamento);
            if (fechamentoDto.EhAplicacaoSondagem)
                repositorioPeriodoFechamento.SalvarCiclosSondagem(fechamento.FechamentosCicloSondagem, id);
            else
                repositorioPeriodoFechamento.SalvarBimestres(fechamento.FechamentosBimestre, id);
            unitOfWork.PersistirTransacao();

            if (!fechamentoDto.EhAplicacaoSondagem)
                await CriarEventoFechamento(fechamento);
        }

        private static Notificacao MontaNotificacao(string nomeEntidade, string tipoEntidade, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, string codigoUe, string codigoDre)
        {
            var mensagem = new StringBuilder();
            mensagem.AppendLine($"A {tipoEntidade} realizou alterações em datas de abertura do período de fechamento de bimestre e as datas definidas pela ");
            mensagem.Append($"{nomeEntidade} foram ajustadas.");
            mensagem.AppendLine("<br> As novas datas são: <br><br>");

            foreach (var bimestre in fechamentosBimestre.OrderBy(a => a.PeriodoEscolar.Bimestre))
            {
                mensagem.AppendLine($"{bimestre.PeriodoEscolar.TipoCalendario.Nome} - {bimestre.PeriodoEscolar.TipoCalendario.AnoLetivo}");
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
            if (fechamentoExistente.EhNulo())
                return;
            var eventoExistente = fechamentoExistente.Evento ?? repositorioEvento.ObterPorId(fechamentoExistente.EventoId);
            if (eventoExistente.NaoEhNulo())
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

            if (tipoEvento.EhNulo())
                throw new NegocioException("Tipo de evento de fechamento de bimestre não encontrado na base de dados.");

            foreach (var bimestre in fechamento.FechamentosBimestre)
            {
                EventoFechamento fechamentoExistente = await mediator.Send(new ObterEventoFechamenoPorIdQuery(bimestre.Id));

                if (fechamentoExistente.NaoEhNulo())
                    AtualizaEventoDeFechamento(bimestre, fechamentoExistente);

                else
                    CriaEventoDeFechamento(fechamento, tipoEvento, bimestre);
            }
        }

        private async Task EnviarNotificacaoUeDre(IEnumerable<PeriodoFechamentoBimestre> periodosFechamentoBimestreUE, long? ueId, long? dreId)
        {
            // Notifica Alteração dos Periodos
            if (ueId.HasValue)
                await EnviaNotificacaoParaUe(periodosFechamentoBimestreUE, ueId.Value);
            else
                await EnviaNotificacaoParaDre(dreId.Value, periodosFechamentoBimestreUE);
        }

        private async Task EnviaNotificacaoParaDre(long dreId, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre)
        {
            var dre = repositorioDre.ObterPorId(dreId);
            if (dre.NaoEhNulo())
            {

                Notificacao notificacao = MontaNotificacao(dre.Nome, "SME", fechamentosBimestre, null, dre.CodigoDre);
                var adminsSgpDre = await mediator.Send(new ObterAdministradoresPorUEQuery(dre.CodigoDre));
                if (adminsSgpDre.NaoEhNulo() && adminsSgpDre.Any())
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
            if (ue.NaoEhNulo())
            {
                var nomeUe = $"{ue.TipoEscola.ObterNomeCurto()} {ue.Nome}";

                Notificacao notificacao = MontaNotificacao(nomeUe, "DRE", fechamentosBimestre, ue.CodigoUe, null);
                var diretores = await mediator.Send(
                    new ObterFuncionariosPorCargoUeQuery(ue.CodigoUe, (long)Cargo.Diretor));
                if (diretores.EhNulo() || !diretores.Any())
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

                var admsUe = await mediator.Send(new ObterAdministradoresPorUEQuery(ue.CodigoUe));

                if (admsUe.EhNulo() || !admsUe.Any())
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
            foreach (var fechamentoBimestre in fechamento?.FechamentosBimestre)
            {
                var dto = new FechamentoBimestreDto
                {
                    Id = fechamentoBimestre.Id,
                    Bimestre = fechamentoBimestre.PeriodoEscolar.Bimestre,
                    InicioDoFechamento = fechamentoBimestre.InicioDoFechamento != DateTime.MinValue ? fechamentoBimestre.InicioDoFechamento : (DateTime?)null,
                    FinalDoFechamento = fechamentoBimestre.FinalDoFechamento != DateTime.MinValue ? fechamentoBimestre.FinalDoFechamento : (DateTime?)null,
                    PeriodoEscolar = fechamentoBimestre.PeriodoEscolar,
                    InicioMinimo = new DateTime(fechamentoBimestre.PeriodoEscolar.PeriodoInicio.Year, 01, 01),
                    FinalMaximo = new DateTime(fechamentoBimestre.PeriodoEscolar.PeriodoInicio.Year, 12, 31),
                };

                if (!EhAplicacaoSondagem(fechamento.Aplicacao))
                {
                    dto.PeriodoEscolarId = fechamentoBimestre.PeriodoEscolarId;
                }

                listaFechamentoBimestre.Add(dto);
            }
            return listaFechamentoBimestre;
        }

        private async Task<PeriodoFechamento> MapearParaDominioAsync(FechamentoDto fechamentoDto)
        {
            var fechamento = repositorioPeriodoFechamento.ObterPorFiltros(fechamentoDto.TipoCalendarioId.Value, null, fechamentoDto.Aplicacao);

            if (fechamento.EhNulo())
                fechamento = new PeriodoFechamento();

            fechamento.Aplicacao = fechamentoDto.Aplicacao;
            fechamento.Migrado = fechamentoDto.Migrado;

            if (fechamentoDto.DreId != 0 && fechamentoDto.DreId > 0)
            {
                var dre = await repositorioDre.ObterPorIdAsync(Convert.ToInt32(fechamentoDto.DreId));
                if (dre.NaoEhNulo())
                    fechamento.AdicionarDre(dre);
            }

            if (fechamentoDto.UeId != 0 && fechamentoDto.UeId > 0)
            {
                var ue = repositorioUe.ObterPorId(Convert.ToInt32(fechamentoDto.UeId));
                if (ue.NaoEhNulo())
                    fechamento.AdicionarUe(ue);
            }

            var tipoCalendario = repositorioTipoCalendario.ObterPorId(fechamentoDto.TipoCalendarioId.Value);
            if (tipoCalendario.EhNulo())
            {
                throw new NegocioException("Tipo calendário não encontrado.");
            }

            if (fechamentoDto.EhAplicacaoSondagem && fechamentoDto.FechamentosBimestres.NaoEhNulo() && fechamentoDto.FechamentosBimestres.Any())
            {
                foreach (var bimestre in fechamentoDto.FechamentosBimestres)
                {
                    var ciclo = new PeriodoFechamentoCicloSondagem
                    {
                        Id = bimestre.Id,
                        PeriodoFechamentoId = fechamento.Id,
                        Ciclo = bimestre.Bimestre,
                        InicioDoFechamento = (DateTime)bimestre.InicioDoFechamento,
                        FinalDoFechamento = (DateTime)bimestre.FinalDoFechamento
                    };

                    var cicloExistente = fechamento.ObterFechamentoCicloSondagem(ciclo);

                    if (cicloExistente.NaoEhNulo())
                    {
                        fechamento.ValidarPeriodoCicloSondagemInicioFim(ciclo);
                        fechamento.ValidarPeriodoCicloSondagemConcomitante(ciclo);
                        cicloExistente.AtualizarDatas(ciclo.InicioDoFechamento, ciclo.FinalDoFechamento);
                    }
                    else
                    {
                        fechamento.AdicionarFechamentoCicloSondagem(ciclo);
                    }
                }
            }

            if (!fechamentoDto.EhAplicacaoSondagem && fechamentoDto.FechamentosBimestres.NaoEhNulo() && fechamentoDto.FechamentosBimestres.Any())
            {
                foreach (var bimestre in fechamentoDto.FechamentosBimestres)
                {
                    var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(bimestre.PeriodoEscolarId);
                    PeriodoFechamentoBimestre fechamentoBimestreExistente = fechamento.ObterFechamentoBimestre(bimestre.PeriodoEscolarId);
                    if (fechamentoBimestreExistente.NaoEhNulo())
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
            return fechamento.EhNulo() ? null : new FechamentoDto
            {
                Id = fechamento.Id,
                DreId = fechamento?.DreId,
                TipoCalendarioId = fechamento.FechamentosBimestre.FirstOrDefault()?.PeriodoEscolar.TipoCalendarioId,
                UeId = fechamento.UeId,
                FechamentosBimestres = MapearFechamentoBimestreParaDto(fechamento)?.OrderBy(c => c.Bimestre),
                AlteradoEm = fechamento?.AlteradoEm,
                AlteradoPor = fechamento?.AlteradoPor,
                AlteradoRF = fechamento?.AlteradoRF,
                CriadoEm = fechamento.CriadoEm,
                CriadoPor = fechamento?.CriadoPor,
                CriadoRF = fechamento?.CriadoRF,
                Migrado = fechamento.Migrado,
                Aplicacao = fechamento.Aplicacao,
            };
        }

        private bool EhAplicacaoSondagem(Aplicacao aplicacao)
        {
            return aplicacao == Aplicacao.SondagemAplicacao || aplicacao == Aplicacao.SondagemDigitacao;
        }
    }
}