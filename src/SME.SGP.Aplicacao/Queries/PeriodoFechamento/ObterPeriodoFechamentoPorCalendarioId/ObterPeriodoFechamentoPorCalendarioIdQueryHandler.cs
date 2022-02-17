﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PeriodoFechamento.ObterPeriodoFechamentoPorCalendarioId
{
    public class ObterPeriodoFechamentoPorCalendarioIdQueryHandler : IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdQuery, FechamentoDto>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;

        public ObterPeriodoFechamentoPorCalendarioIdQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento,
                                                                            IRepositorioTipoCalendario repositorioTipoCalendario,
                                                                            IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new ArgumentNullException(nameof(repositorioPeriodoFechamento));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<FechamentoDto> Handle(ObterPeriodoFechamentoPorCalendarioIdQuery request, CancellationToken cancellationToken)
        {
            var fechamentoSME = await repositorioPeriodoFechamento.ObterPorFiltrosAsync(request.TipoCalendarioId, null);

            if (fechamentoSME == null)
            {
                LimparCamposNaoUtilizadosRegistroPai(fechamentoSME);
                fechamentoSME = new Dominio.PeriodoFechamento();

                var tipoCalendario = await repositorioTipoCalendario.ObterPorIdAsync(request.TipoCalendarioId);
                if (tipoCalendario == null)
                    throw new NegocioException("Tipo de calendário não encontrado.");

                var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(request.TipoCalendarioId);
                if (periodosEscolares == null || !periodosEscolares.Any())
                    throw new NegocioException("Período escolar não encontrado.");

                foreach (var periodo in periodosEscolares)
                {
                    periodo.AdicionarTipoCalendario(tipoCalendario);
                    fechamentoSME.AdicionarFechamentoBimestre(new PeriodoFechamentoBimestre(fechamentoSME.Id, periodo, null, null));
                }
            }

            return MapearParaDto(fechamentoSME);
        }

        private void LimparCamposNaoUtilizadosRegistroPai(Dominio.PeriodoFechamento registroFilho)
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

        private FechamentoDto MapearParaDto(Dominio.PeriodoFechamento fechamento)
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

        private IEnumerable<FechamentoBimestreDto> MapearFechamentoBimestreParaDto(Dominio.PeriodoFechamento fechamento)
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
    }
}
