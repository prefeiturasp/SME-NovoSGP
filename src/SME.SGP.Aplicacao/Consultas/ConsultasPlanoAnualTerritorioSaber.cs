﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoAnualTerritorioSaber : IConsultasPlanoAnualTerritorioSaber
    {
        private readonly IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IMediator mediator;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;

        public ConsultasPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                                  IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                  IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
                                                  IMediator mediator)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<long> ObterIdPlanoAnualTerritorioSaberPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId)
        {
            var plano = await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(ano, escolaId, turmaId, bimestre, territorioExperienciaId);
            return plano != null ? plano.Id : 0;
        }

        public async Task<PlanoAnualTerritorioSaberCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            var planoAnual = await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoEscolaBimestreETurma(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
            return planoAnual;
        }

        public async Task<IEnumerable<PlanoAnualTerritorioSaberCompletoDto>> ObterPorUETurmaAnoETerritorioExperiencia(string ueId, string turmaId, int anoLetivo, long territorioExperienciaId)
        {
            var periodos = await ObterPeriodoEscolar(turmaId, anoLetivo);
            var dataAtual = DateTime.Now.Date;
            var listaPlanoAnual = await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(anoLetivo, ueId, turmaId, territorioExperienciaId);
            if (listaPlanoAnual != null && listaPlanoAnual.Any())
            {
                if (listaPlanoAnual.Count() != periodos.Count())
                {
                    var periodosFaltantes = periodos.Where(c => !listaPlanoAnual.Any(p => p.Bimestre == c.Bimestre));
                    var planosFaltantes = ObterNovoPlanoAnualTerritorioSaberCompleto(turmaId, anoLetivo, ueId, periodosFaltantes, dataAtual).ToList();
                    planosFaltantes.AddRange(listaPlanoAnual);
                    listaPlanoAnual = planosFaltantes;
                }
            }
            else
                listaPlanoAnual = ObterNovoPlanoAnualTerritorioSaberCompleto(turmaId, anoLetivo, ueId, periodos, dataAtual);
            return listaPlanoAnual.OrderBy(c => c.Bimestre);
        }

        private IEnumerable<PlanoAnualTerritorioSaberCompletoDto> ObterNovoPlanoAnualTerritorioSaberCompleto(string turmaId, int anoLetivo, string ueId, IEnumerable<PeriodoEscolar> periodos, DateTime dataAtual)
        {
            var listaPlanoAnual = new List<PlanoAnualTerritorioSaberCompletoDto>();
            foreach (var periodo in periodos)
            {
                var obrigatorio = false;
                if (periodo.PeriodoFim.Local() >= dataAtual && periodo.PeriodoInicio.Local() <= dataAtual)
                {
                    obrigatorio = true;
                }
                listaPlanoAnual.Add(ObterPlanoAnualPorBimestre(turmaId, anoLetivo, ueId, periodo.Bimestre, obrigatorio));
            }
            return listaPlanoAnual;
        }

        private async Task<IEnumerable<PeriodoEscolar>> ObterPeriodoEscolar(string turmaId, int anoLetivo)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }
            var modalidade = turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;
            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo de calendário não encontrado.");
            }

            var periodos = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodos == null)
            {
                throw new NegocioException("Período escolar não encontrado.");
            }
            return periodos;
        }

        private static PlanoAnualTerritorioSaberCompletoDto ObterPlanoAnualPorBimestre(string turmaId, int anoLetivo, string ueId, int bimestre, bool obrigatorio)
        {
            return new PlanoAnualTerritorioSaberCompletoDto
            {
                Bimestre = bimestre,
                EscolaId = ueId,
                TurmaId = turmaId,
                AnoLetivo = anoLetivo
            };
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }
    }
}
