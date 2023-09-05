﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IMediator mediator;

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
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaId));
            var bimestresAbertoFechado = new List<PeriodoEscolarPorTurmaDto>();
            IEnumerable<ComponenteCurricularEol> componentesCurricularesEolProfessor = Enumerable.Empty<ComponenteCurricularEol>();
            IList<(string codigo, string codigoComponentePai, string codigoTerritorioSaber)> componentesCurricularesDoProfessorCj = new List<(string, string, string)>();
            var componenteCurricularId = territorioExperienciaId;
            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(componenteCurricularId));

            periodos.Select(s => s.Bimestre).Distinct().ToList().ForEach(bimestre =>
            {
                bimestresAbertoFechado.Add(new PeriodoEscolarPorTurmaDto()
                {
                    Bimestre = bimestre,
                    PeriodoAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, turma.AnoLetivo == DateTime.Today.Year)).Result,
                });
            });
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (!usuarioLogado.EhProfessorCj())
                componentesCurricularesEolProfessor = await mediator
                    .Send(new ObterComponentesCurricularesDoProfessorNaTurmaQuery(turmaId,
                                                                                  usuarioLogado.Login,
                                                                                  usuarioLogado.PerfilAtual,
                                                                                  usuarioLogado.EhProfessorInfantilOuCjInfantil()));

            if (usuarioLogado.EhProfessorCj())
            {
                var componentesCurricularesDoProfessorCJ = await mediator
                   .Send(new ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(usuarioLogado.Login));

                if (componentesCurricularesDoProfessorCJ.Any())
                {
                    var dadosComponentes = await mediator.Send(new ObterDisciplinasPorIdsQuery(componentesCurricularesDoProfessorCJ.Select(c => c.DisciplinaId).ToArray()));
                    if (dadosComponentes.Any())
                    {
                        componentesCurricularesDoProfessorCj = dadosComponentes
                            .Select(d => (d.CodigoComponenteCurricular.ToString(), d.CdComponenteCurricularPai.ToString(), d.TerritorioSaber
                                ? d.CodigoComponenteCurricular.ToString() : "0")).ToArray();
                    }
                }
            }

            var componenteCorrespondente = !usuarioLogado.EhProfessorCj() && componentesCurricularesEolProfessor != null && (componentesCurricularesEolProfessor.Any(x => x.Regencia) || usuarioLogado.EhProfessor())
                    ? componentesCurricularesEolProfessor.FirstOrDefault(cp => cp.CodigoComponenteCurricularPai == territorioExperienciaId || (componenteCurricular != null && cp.Codigo.ToString() == componenteCurricular.CdComponenteCurricularPai.ToString()) || cp.Codigo == territorioExperienciaId || cp.CodigoComponenteTerritorioSaber == territorioExperienciaId)
                    : new ComponenteCurricularEol
                    {
                        Codigo = territorioExperienciaId > 0 ? territorioExperienciaId : 0,
                        CodigoComponenteCurricularPai = componentesCurricularesDoProfessorCj.Select(c => long.TryParse(c.codigoComponentePai, out long codigoPai) ? codigoPai : 0).FirstOrDefault(),
                        CodigoComponenteTerritorioSaber = componentesCurricularesDoProfessorCj.Select(c => long.TryParse(c.codigoTerritorioSaber, out long codigoTerritorio) ? codigoTerritorio : 0).FirstOrDefault()
                    };

            var listaPlanoAnual = await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(anoLetivo, ueId, turmaId, componenteCorrespondente?.CodigoComponenteTerritorioSaber ?? 0, usuarioLogado.EhProfessor() ? usuarioLogado.CodigoRf : null);
            if (listaPlanoAnual != null && listaPlanoAnual.Any())
            {
                if (listaPlanoAnual.Count() != periodos.Count())
                {
                    var periodosFaltantes = periodos.Where(c => !listaPlanoAnual.Any(p => p.Bimestre == c.Bimestre));
                    var planosFaltantes = ObterNovoPlanoAnualTerritorioSaberCompleto(turma, anoLetivo, ueId, periodosFaltantes, dataAtual).ToList();
                    planosFaltantes.AddRange(listaPlanoAnual);
                    listaPlanoAnual = planosFaltantes;
                }
            }
            else
                listaPlanoAnual = ObterNovoPlanoAnualTerritorioSaberCompleto(turma, anoLetivo, ueId, periodos, dataAtual);

            listaPlanoAnual.ToList().ForEach(planoAnual =>
            {
                planoAnual.PeriodoAberto = bimestresAbertoFechado.FirstOrDefault(f => f.Bimestre == planoAnual.Bimestre).PeriodoAberto;
            });

            return listaPlanoAnual.OrderBy(c => c.Bimestre);
        }

        private IEnumerable<PlanoAnualTerritorioSaberCompletoDto> ObterNovoPlanoAnualTerritorioSaberCompleto(Turma turma, int anoLetivo, string ueId, IEnumerable<PeriodoEscolar> periodos, DateTime dataAtual)
        {
            var listaPlanoAnual = new List<PlanoAnualTerritorioSaberCompletoDto>();
            foreach (var periodo in periodos)
            {
                listaPlanoAnual.Add(ObterPlanoAnualPorBimestre(turma.CodigoTurma, anoLetivo, ueId, periodo.Bimestre));
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

        private static PlanoAnualTerritorioSaberCompletoDto ObterPlanoAnualPorBimestre(string turmaId, int anoLetivo, string ueId, int bimestre)
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
