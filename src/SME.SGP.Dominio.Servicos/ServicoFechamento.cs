using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IServicoUsuario servicoUsuario,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                 IRepositorioDre repositorioDre,
                                 IRepositorioUe repositorioUe,
                                 IUnitOfWork unitOfWork)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var (dre, ue) = ObterDreEUe(dreId, ueId);

            var fechamentoSMEDre = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, usuarioLogado.EhPerfilDRE() ? null : dre?.Id, null);
            if (fechamentoSMEDre == null)
            {
                fechamentoSMEDre = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, null, null);
                if (fechamentoSMEDre == null)
                {
                    if (!usuarioLogado.EhPerfilSME())
                        throw new NegocioException("Fechamento da SME/Dre não encontrado para este tipo de calendário.");
                    else
                    {
                        fechamentoSMEDre = new Fechamento(null, null);

                        var tipoCalendario = repositorioTipoCalendario.ObterPorId(tipoCalendarioId);
                        if (tipoCalendario == null)
                            throw new NegocioException("Tipo de calendário não encontrado.");

                        var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendarioId);
                        if (periodoEscolar == null)
                            throw new NegocioException("Período escolar não encontrado.");

                        foreach (var periodo in periodoEscolar)
                        {
                            periodo.AdicionarTipoCalendario(tipoCalendario);
                            fechamentoSMEDre.AdicionarFechamentoBimestre(new FechamentoBimestre(fechamentoSMEDre.Id, periodo, periodo.PeriodoInicio, periodo.PeriodoFim));
                        }
                    }
                }
            }

            var fechamentoDreUe = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, dre?.Id, ue?.Id);
            if (fechamentoDreUe == null)
            {
                if (!usuarioLogado.EhPerfilSME() && !usuarioLogado.EhPerfilDRE())
                    throw new NegocioException("Fechamento da Ue não encontrado para este tipo de calendário.");
                fechamentoDreUe = fechamentoSMEDre;
            }

            var fechamentoDto = MapearParaDto(fechamentoDreUe);

            foreach (var bimestreSME in fechamentoSMEDre.FechamentosBimestre)
            {
                var bimestreDreUe = fechamentoDto.FechamentosBimestres.FirstOrDefault(c => c.Bimestre == bimestreSME.PeriodoEscolar.Bimestre);
                if (bimestreDreUe != null)
                {
                    if (fechamentoSMEDre.Id > 0)
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
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                var id = repositorioFechamento.Salvar(fechamento);
                repositorioFechamento.SalvarBimestres(fechamento.FechamentosBimestre, id);
                unitOfWork.PersistirTransacao();
            }
        }

        private IEnumerable<FechamentoBimestreDto> MapearFechamentoBimestreParaDto(Fechamento fechamento)
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

        private Fechamento MapearParaDominio(FechamentoDto fechamentoDto)
        {
            var (dre, ue) = ObterDreEUe(fechamentoDto.DreId, fechamentoDto.UeId);
            var fechamento = repositorioFechamento.ObterPorTipoCalendarioDreEUE(fechamentoDto.TipoCalendarioId, dre?.Id, ue?.Id);
            if (fechamento == null)
                fechamento = new Fechamento(dre?.Id, ue?.Id);

            var tipoCalendario = repositorioTipoCalendario.ObterPorId(fechamentoDto.TipoCalendarioId);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo calendário não encontrado.");
            }

            if (fechamentoDto.FechamentosBimestres != null && fechamentoDto.FechamentosBimestres.Any())
            {
                foreach (var bimestre in fechamentoDto.FechamentosBimestres)
                {
                    var periodoEscolar = repositorioPeriodoEscolar.ObterPorId(bimestre.PeriodoEscolarId);
                    FechamentoBimestre fechamentoBimestreExistente = fechamento.ObterFechamentoBimestre(bimestre.PeriodoEscolarId);
                    if (fechamentoBimestreExistente != null)
                    {
                        fechamentoBimestreExistente.AtualizarDatas(bimestre.InicioDoFechamento, bimestre.FinalDoFechamento);
                    }
                    else
                        fechamento.AdicionarFechamentoBimestre(new FechamentoBimestre(fechamento.Id, periodoEscolar, bimestre.InicioDoFechamento, bimestre.FinalDoFechamento));
                }
            }
            return fechamento;
        }

        private FechamentoDto MapearParaDto(Fechamento fechamento)
        {
            return fechamento == null ? null : new FechamentoDto
            {
                Id = fechamento.Id,
                DreId = fechamento.Dre?.CodigoDre,
                TipoCalendarioId = fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId,
                UeId = fechamento.Ue?.CodigoUe,
                FechamentosBimestres = MapearFechamentoBimestreParaDto(fechamento)
            };
        }

        private (Dre, Ue) ObterDreEUe(string codigoDre, string codigoUe)
        {
            Dre dre = null;
            Ue ue = null;
            if (!string.IsNullOrWhiteSpace(codigoDre))
            {
                dre = repositorioDre.ObterPorCodigo(codigoDre.ToString());
            }
            ue = null;
            if (!string.IsNullOrWhiteSpace(codigoUe))
            {
                ue = repositorioUe.ObterPorCodigo(codigoUe.ToString());
            }
            return (dre, ue);
        }

        private void ValidarCamposObrigatorios(bool ehSme, bool ehDre, Fechamento fechamento)
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

        private void ValidarHierarquiaPeriodos(bool ehSme, bool ehDre, Fechamento fechamento)
        {
            Fechamento fechamentoParaValidacao = null;
            if (ehDre)
            {
                fechamentoParaValidacao = repositorioFechamento.ObterPorTipoCalendarioDreEUE(fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId, null, null);
            }
            else
            {
                if (!ehSme)
                {
                    fechamentoParaValidacao = repositorioFechamento.ObterPorTipoCalendarioDreEUE(fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId, fechamento.DreId, null);
                }
            }
            if (fechamentoParaValidacao != null)
                fechamento.ValidarIntervaloDatasDreEUe(fechamentoParaValidacao.FechamentosBimestre.ToList());
        }
    }
}