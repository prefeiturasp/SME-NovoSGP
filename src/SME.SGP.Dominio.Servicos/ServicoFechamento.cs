using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoFechamento : IServicoFechamento
    {
        private readonly IRepositorioFechamento repositorioFechamento;

        public ServicoFechamento(IRepositorioFechamento repositorioFechamento)
        {
            this.repositorioFechamento = repositorioFechamento ?? throw new ArgumentNullException(nameof(repositorioFechamento));
        }

        public FechamentoDto ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId)
        {
            var fechamentoSMEDre = repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, dreId, string.Empty);
            if (fechamentoSMEDre == null)
            {
                throw new NegocioException("Fechamento da SME não encontrado para este tipo de calendário.");
            }

            var fechamentoDreUe = MapearParaDto(repositorioFechamento.ObterPorTipoCalendarioDreEUE(tipoCalendarioId, dreId, ueId));
            if (fechamentoDreUe == null)
            {
                throw new NegocioException("Fechamento da Dre/Ue não encontrado para este tipo de calendário.");
            }

            foreach (var bimestreSME in fechamentoSMEDre.FechamentosBimestre)
            {
                var bimestreDreUe = fechamentoDreUe.FechamentosBimestres.FirstOrDefault(c => c.Bimestre == bimestreSME.PeriodoEscolar.Bimestre);
                if (bimestreDreUe != null)
                {
                    bimestreDreUe.InicioMinimo = bimestreSME.InicioDoFechamento;
                    bimestreDreUe.FinalMaximo = bimestreSME.FinalDoFechamento;
                }
            }
            return fechamentoDreUe;
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