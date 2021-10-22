using MediatR;
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
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IMediator mediator;

        public ConsultasPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                                  IRepositorioTurma repositorioTurma,
                                                  IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                  IRepositorioTipoCalendario repositorioTipoCalendario, 
                                                  IMediator mediator)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
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
            periodos.Select(s => s.Bimestre).Distinct().ToList().ForEach(bimestre =>
            {
                bimestresAbertoFechado.Add(new PeriodoEscolarPorTurmaDto()
                {
                    Bimestre = bimestre,
                    PeriodoAberto = mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestre, turma.AnoLetivo == DateTime.Today.Year)).Result,
                });
            });
            var listaPlanoAnual = await repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(anoLetivo, ueId, turmaId, territorioExperienciaId);
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
            var turma = await repositorioTurma.ObterPorCodigo(turmaId);
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
