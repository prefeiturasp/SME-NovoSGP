using SME.Background.Core;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoPeriodoFechamento : IServicoPeriodoFechamento
    {
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IRepositorioEventoFechamento repositorioEventoFechamento;
        private readonly IRepositorioPeriodoFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioEventoTipo repositorioTipoEvento;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEOL servicoEol;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoPeriodoFechamento(IRepositorioPeriodoFechamento repositorioFechamento,
                                 IServicoUsuario servicoUsuario,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IRepositorioDre repositorioDre,
                                 IRepositorioUe repositorioUe,
                                 IRepositorioEventoFechamento repositorioEventoFechamento,
                                 IRepositorioEvento repositorioEvento,
                                 IRepositorioEventoTipo repositorioTipoEvento,
                                 IServicoEOL servicoEol,
                                 IServicoNotificacao servicoNotificacao,
                                 IUnitOfWork unitOfWork)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
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
        }

        public async Task AlterarPeriodosComHierarquiaInferior(PeriodoFechamento fechamento)
        {
            var ehParaUe = fechamento.DreId != null && fechamento.DreId > 0;
            unitOfWork.IniciarTransacao();
            foreach (var fechamentoBimestre in fechamento.FechamentosBimestre)
            {
                repositorioFechamento.AlterarPeriodosComHierarquiaInferior(fechamentoBimestre.InicioDoFechamento, fechamentoBimestre.FinalDoFechamento, fechamentoBimestre.PeriodoEscolarId, fechamento.DreId);
            }
            EnviarNotificacaoDeAlteracaoDePeriodo(fechamento, fechamento.FechamentosBimestre, ehParaUe);
            unitOfWork.PersistirTransacao();
        }


        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId)
        {
            var (dre, ue) = ObterDreEUe(dreId, ueId);
            return await ObterPorTipoCalendarioDreEUe(tipoCalendarioId, dre, ue);
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, Dre dre, Ue ue)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var dreIdFiltro = !(dre == null) || usuarioLogado.EhPerfilUE() ? dre?.Id : null;

            var fechamentoSMEDre = repositorioFechamento.ObterPorFiltros(tipoCalendarioId, dreIdFiltro, null, null);
            var ehRegistroExistente = dre == null && fechamentoSMEDre != null;
            if (fechamentoSMEDre == null)
            {
                fechamentoSMEDre = repositorioFechamento.ObterPorFiltros(tipoCalendarioId, null, null, null);
                ehRegistroExistente = fechamentoSMEDre != null;
                if (fechamentoSMEDre == null)
                {
                    if (!usuarioLogado.EhPerfilSME())
                        throw new NegocioException("Fechamento da SME/Dre não encontrado para este tipo de calendário.");
                    else
                    {
                        fechamentoSMEDre = new PeriodoFechamento(null, null);

                        var tipoCalendario = repositorioTipoCalendario.ObterPorId(tipoCalendarioId);
                        if (tipoCalendario == null)
                            throw new NegocioException("Tipo de calendário não encontrado.");

                        var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
                        if (periodoEscolar == null)
                            throw new NegocioException("Período escolar não encontrado.");

                        foreach (var periodo in periodoEscolar)
                        {
                            periodo.AdicionarTipoCalendario(tipoCalendario);
                            fechamentoSMEDre.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamentoSMEDre.Id, periodo, periodo.PeriodoInicio, periodo.PeriodoFim));
                        }
                    }
                }
            }

            var fechamentoDreUe = repositorioFechamento.ObterPorFiltros(tipoCalendarioId, dre?.Id, ue?.Id, null);
            if (fechamentoDreUe == null)
            {
                fechamentoDreUe = fechamentoSMEDre;
                fechamentoDreUe.Dre = dre;
                fechamentoDreUe.Ue = ue;
            }

            var fechamentoDto = MapearParaDto(fechamentoDreUe);

            foreach (var bimestreSME in fechamentoSMEDre.FechamentosBimestre)
            {
                var bimestreDreUe = fechamentoDto.FechamentosBimestres.FirstOrDefault(c => c.Bimestre == bimestreSME.PeriodoEscolar.Bimestre);
                if (bimestreDreUe != null)
                {
                    bimestreDreUe.PeriodoEscolar = bimestreSME.PeriodoEscolar;
                    if (fechamentoSMEDre.Id > 0 && !(dre == null) || !(ue == null))
                    {
                        bimestreDreUe.InicioMinimo = bimestreSME.InicioDoFechamento;
                        bimestreDreUe.FinalMaximo = bimestreSME.FinalDoFechamento;
                    }
                    else
                    {
                        bimestreDreUe.InicioMinimo = new DateTime(bimestreSME.InicioDoFechamento.Year, 01, 01);
                        bimestreDreUe.FinalMaximo = new DateTime(bimestreSME.InicioDoFechamento.Year, 12, 31);
                    }
                }
            }
            return fechamentoDto;
        }

        public async Task Salvar(FechamentoDto fechamentoDto)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var fechamento = MapearParaDominio(fechamentoDto);
            var (ehSme, ehDre) = (usuarioLogado.EhPerfilSME(), usuarioLogado.EhPerfilDRE());

            ValidarCamposObrigatorios(ehSme, ehDre, fechamento);
            ValidarHierarquiaPeriodos(ehSme, ehDre, fechamento);
            ValidarRegistrosForaDoPeriodo(fechamentoDto, fechamento, ehSme, ehDre);            

            unitOfWork.IniciarTransacao();
            var id = repositorioFechamento.Salvar(fechamento);
            repositorioFechamento.SalvarBimestres(fechamento.FechamentosBimestre, id);
            unitOfWork.PersistirTransacao();

            ExecutaAlterarPeriodosComHierarquiaInferior(fechamentoDto, fechamento, ehSme);
            CriarEventoFechamento(fechamento);
        }

        private static void ExecutaAlterarPeriodosComHierarquiaInferior(FechamentoDto fechamentoDto, PeriodoFechamento fechamento, bool ehSme)
        {
            if ((ehSme && !fechamento.DreId.HasValue) || (fechamento.DreId.HasValue && !fechamento.UeId.HasValue) && fechamentoDto.ConfirmouAlteracaoHierarquica)
                Cliente.Executar<IServicoPeriodoFechamento>(c => c.AlterarPeriodosComHierarquiaInferior(fechamento));
        }

        private static Notificacao MontaNotificacao(string nomeEntidade, string tipoEntidade, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, string codigoUe, string codigoDre)
        {
            var mensagem = new StringBuilder();
            mensagem.AppendLine($"A { tipoEntidade} realizou alterações em datas de abertura do período de fechamento de bimestre e as datas definidas pela ");
            mensagem.Append($"{nomeEntidade} foram ajustadas.");
            mensagem.AppendLine("<br> As novas datas são: <br><br>");

            foreach (var bimestre in fechamentosBimestre)
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
            var eventoExistente = repositorioEvento.ObterPorId(fechamentoExistente.EventoId);
            if (eventoExistente != null)
            {
                eventoExistente.DataInicio = bimestre.InicioDoFechamento;
                eventoExistente.DataFim = bimestre.FinalDoFechamento;
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

        private void CriarEventoFechamento(PeriodoFechamento fechamento)
        {
            if (fechamento.UeId > 0)
            {
                var tipoEvento = repositorioTipoEvento.ObterTipoEventoPorTipo(TipoEvento.FechamentoBimestre);
                if (tipoEvento == null)
                {
                    throw new NegocioException("Tipo de evento de fechamento de bimestre não encontrado na base de dados.");
                }

                foreach (var bimestre in fechamento.FechamentosBimestre)
                {
                    EventoFechamento fechamentoExistente = repositorioEventoFechamento.ObterPorIdFechamento(bimestre.Id);

                    if (fechamentoExistente != null)
                    {
                        AtualizaEventoDeFechamento(bimestre, fechamentoExistente);
                    }
                    else
                    {
                        CriaEventoDeFechamento(fechamento, tipoEvento, bimestre);
                    }
                }
            }
        }

        private void EnviaNotificacaoParaDre(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre)
        {
            var dres = repositorioDre.ObterTodas();
            if (dres != null && dres.Any())
            {
                foreach (var dre in dres)
                {
                    Notificacao notificacao = MontaNotificacao(dre.Nome, "SME", fechamentosBimestre, null, dre.CodigoDre);
                    var adminsSgpDre = servicoEol.ObterAdministradoresSGPParaNotificar(dre.CodigoDre).Result;
                    if (adminsSgpDre != null && adminsSgpDre.Any())
                    {
                        foreach (var adminSgpUe in adminsSgpDre)
                        {
                            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(adminSgpUe);
                            notificacao.UsuarioId = usuario.Id;

                            servicoNotificacao.Salvar(notificacao);
                        }
                    }
                }
            }
        }

        private void EnviaNotificacaoParaUe(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long dreId)
        {
            var ues = repositorioUe.ObterPorDre(dreId);
            if (ues != null && ues.Any())
            {
                foreach (var ue in ues)
                {
                    var nomeUe = $"{ue.TipoEscola.GetAttribute<DisplayAttribute>().ShortName} {ue.Nome}";

                    Notificacao notificacao = MontaNotificacao(nomeUe, "DRE", fechamentosBimestre, null, ue.CodigoUe);
                    var diretores = servicoEol.ObterFuncionariosPorCargoUe(ue.CodigoUe, (long)Cargo.Diretor);
                    if (diretores == null || !diretores.Any())
                        throw new NegocioException($"Não foram localizados diretores para Ue {ue.CodigoUe}.");

                    foreach (var diretor in diretores)
                    {
                        var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(diretor.CodigoRf);
                        notificacao.UsuarioId = usuario.Id;

                        servicoNotificacao.Salvar(notificacao);
                    }
                }
            }
        }

        private void EnviarNotificacaoDeAlteracaoDePeriodo(PeriodoFechamento fechamento, IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, bool paraUe)
        {
            if (paraUe)
            {
                EnviaNotificacaoParaUe(fechamentosBimestre, fechamento.DreId.Value);
            }
            else
            {
                EnviaNotificacaoParaDre(fechamentosBimestre);
            }
        }

        private IEnumerable<FechamentoBimestreDto> MapearFechamentoBimestreParaDto(PeriodoFechamento fechamento)
        {
            var listaFechamentoBimestre = new List<FechamentoBimestreDto>();
            foreach (var fechamentoBimestre in fechamento.FechamentosBimestre)
            {
                listaFechamentoBimestre.Add(new FechamentoBimestreDto
                {
                    FinalDoFechamento = fechamentoBimestre.FinalDoFechamento,
                    InicioDoFechamento = fechamentoBimestre.InicioDoFechamento,
                    Bimestre = fechamentoBimestre.PeriodoEscolar.Bimestre,
                    Id = fechamentoBimestre.Id,
                    PeriodoEscolarId = fechamentoBimestre.PeriodoEscolarId
                });
            }
            return listaFechamentoBimestre;
        }

        private PeriodoFechamento MapearParaDominio(FechamentoDto fechamentoDto)
        {
            var (dre, ue) = ObterDreEUe(fechamentoDto.DreId, fechamentoDto.UeId);
            var fechamento = repositorioFechamento.ObterPorFiltros(fechamentoDto.TipoCalendarioId.Value, dre?.Id, ue?.Id, null);
            if (fechamento == null)
                fechamento = new PeriodoFechamento(dre, ue);

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

        private void ValidarCamposObrigatorios(bool ehSme, bool ehDre, PeriodoFechamento fechamento)
        {
            if (!fechamento.UeId.HasValue)
            {
                if (!ehDre && !ehSme)
                    throw new NegocioException("O campo UE é obrigatório.");
            }
            else if (!fechamento.DreId.HasValue && !ehSme)
            {
                throw new NegocioException("O campo DRE é obrigatório.");
            }
        }

        private void ValidarHierarquiaPeriodos(bool ehSme, bool ehDre, PeriodoFechamento fechamento)
        {
            PeriodoFechamento fechamentoParaValidacao = null;
            if (ehDre)
            {
                fechamentoParaValidacao = repositorioFechamento.ObterPorFiltros(fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId, null, null, null);
            }
            else
            {
                if (!ehSme)
                {
                    fechamentoParaValidacao = repositorioFechamento.ObterPorFiltros(fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId, fechamento.DreId, null, null);
                }
            }
            if (fechamentoParaValidacao != null)
                fechamento.ValidarIntervaloDatasDreEUe(fechamentoParaValidacao.FechamentosBimestre.ToList());
        }

        private void ValidarRegistrosForaDoPeriodo(FechamentoDto fechamentoDto, PeriodoFechamento fechamento, bool ehSme, bool ehDre)
        {
            if ((ehDre || ehSme) && string.IsNullOrWhiteSpace(fechamentoDto.UeId) && fechamento.Id > 0 && !fechamentoDto.ConfirmouAlteracaoHierarquica)
            {
                foreach (var fechamentoBimestre in fechamento.FechamentosBimestre)
                {
                    var existeRegistroForaDoPeriodo = repositorioFechamento.ValidaRegistrosForaDoPeriodo(fechamentoBimestre.InicioDoFechamento, fechamentoBimestre.FinalDoFechamento, fechamento.Id, fechamentoBimestre.PeriodoEscolarId, fechamento.DreId);
                    if (existeRegistroForaDoPeriodo)
                    {
                        var textoSme = ehDre ? "" : "DRE's/";
                        throw new NegocioException($"A alteração que você está fazendo afetará datas de fechamento definidas pelas {textoSme}UE's. Deseja Continuar?", 602);
                    }
                }
            }
        }
    }
}