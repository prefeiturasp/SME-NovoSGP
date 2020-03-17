﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultasPeriodoEscolar : IConsultasPeriodoEscolar
    {
        private readonly IRepositorioPeriodoEscolar repositorio;

        public ConsultasPeriodoEscolar(IRepositorioPeriodoEscolar repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId)
        {
            var lista = repositorio.ObterPorTipoCalendario(tipoCalendarioId);

            if (lista == null || !lista.Any())
                return null;

            return EntidadeParaDto(lista);
        }

        public DateTime ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia)
        {
            var periodos = repositorio.ObterPorTipoCalendario(tipoCalendarioId);
            if (periodos == null || !periodos.Any())
                throw new NegocioException("Não foi possível obter os períodos deste tipo de calendário.");

            DateTime fimRecorrencia = DateTime.MinValue;
            if (recorrencia == RecorrenciaAula.RepetirBimestreAtual)
            {
                // Busca ultimo dia do periodo atual
                fimRecorrencia = periodos.Where(a => a.PeriodoFim >= inicioRecorrencia)
                    .OrderBy(a => a.PeriodoInicio)
                    .FirstOrDefault().PeriodoFim;
            }
            else
            if (recorrencia == RecorrenciaAula.RepetirTodosBimestres)
            {
                // Busca ultimo dia do ultimo periodo
                fimRecorrencia = periodos.Max(a => a.PeriodoFim);
            }

            return fimRecorrencia;
        }

        public PeriodoEscolarDto ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo)
        {
            return MapearParaDto(repositorio.ObterPorTipoCalendarioData(tipoCalendarioId, dataPeriodo));
        }

        private PeriodoEscolarDto MapearParaDto(PeriodoEscolar periodo)
            => periodo == null ? null : new PeriodoEscolarDto()
            {
                Id = periodo.Id,
                Bimestre = periodo.Bimestre,
                Migrado = periodo.Migrado,
                PeriodoInicio = periodo.PeriodoInicio,
                PeriodoFim = periodo.PeriodoFim
            };

        private static PeriodoEscolarListaDto EntidadeParaDto(IEnumerable<PeriodoEscolar> lista)
        {
            return new PeriodoEscolarListaDto
            {
                TipoCalendario = lista.ElementAt(0).TipoCalendarioId,
                Periodos = lista.Select(x => new PeriodoEscolarDto
                {
                    Bimestre = x.Bimestre,
                    PeriodoInicio = x.PeriodoInicio,
                    PeriodoFim = x.PeriodoFim,
                    Migrado = x.Migrado,
                    Id = x.Id
                }).ToList()
            };
        }

        public int ObterBimestre(DateTime data, Modalidade modalidade)
            => ((data.Month + 2) / 3) - (modalidade == Modalidade.EJA && data.Month >= 6 ? 2 : 0);
    }
}