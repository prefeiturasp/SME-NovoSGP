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
        private readonly IRepositorioFechamento repositorioFechamento;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento,
                                 IServicoUsuario servicoUsuario,
                                 IRepositorioTipoCalendario repositorioTipoCalendario,
                                 IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, long? dreId, long? ueId)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var fechamentoSMEDre = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, dreId, null);
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
                            fechamentoSMEDre.AdicionarFechamentoBimestre(periodo, new FechamentoBimestre(periodo, periodo.PeriodoInicio, periodo.PeriodoFim));
                        }
                    }
                }
            }

            var fechamentoDreUe = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, dreId, ueId);
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
                    bimestreDreUe.InicioMinimo = bimestreSME.InicioDoFechamento;
                    bimestreDreUe.FinalMaximo = bimestreSME.FinalDoFechamento;
                }
            }
            return fechamentoDto;
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
                    Id = fechamentoBimestre.Id
                });
            }
            return listaFechamentoBimestre;
        }

        private IEnumerable<FechamentoDto> MapearParaDto(IEnumerable<Fechamento> fechamentos)
        {
            return fechamentos?.Select(c => MapearParaDto(c));
        }

        private FechamentoDto MapearParaDto(Fechamento fechamento)
        {
            return fechamento == null ? null : new FechamentoDto
            {
                Id = fechamento.Id,
                DreId = fechamento.DreId,
                TipoCalendarioId = fechamento.FechamentosBimestre.FirstOrDefault().PeriodoEscolar.TipoCalendarioId,
                UeId = fechamento.UeId,
                FechamentosBimestres = MapearFechamentoBimestreParaDto(fechamento)
            };
        }
    }
}