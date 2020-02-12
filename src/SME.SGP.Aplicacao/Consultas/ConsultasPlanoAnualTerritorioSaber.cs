using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoAnualTerritorioSaber : IConsultasPlanoAnualTerritorioSaber
    {
        private readonly IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ConsultasPlanoAnualTerritorioSaber(IRepositorioPlanoAnualTerritorioSaber repositorioPlanoAnualTerritorioSaber,
                                                  IRepositorioTurma repositorioTurma,
                                                  IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                                  IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.repositorioPlanoAnualTerritorioSaber = repositorioPlanoAnualTerritorioSaber ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnualTerritorioSaber));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public long ObterIdPlanoAnualTerritorioSaberPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long territorioExperienciaId)
        {
            var plano = repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberSimplificadoPorAnoEscolaBimestreETurma(ano, escolaId, turmaId, bimestre, territorioExperienciaId);
            return plano != null ? plano.Id : 0;
        }

        public PlanoAnualTerritorioSaberCompletoDto ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            var planoAnual = repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoEscolaBimestreETurma(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
            return planoAnual;
        }

        public IEnumerable<PlanoAnualTerritorioSaberCompletoDto> ObterPorUETurmaAnoETerritorioExperiencia(string ueId, string turmaId, int anoLetivo, long territorioExperienciaId)
        {
            var periodos = ObterPeriodoEscolar(turmaId, anoLetivo);
            var dataAtual = DateTime.Now.Date;
            var listaPlanoAnual = repositorioPlanoAnualTerritorioSaber.ObterPlanoAnualTerritorioSaberCompletoPorAnoUEETurma(anoLetivo, ueId, turmaId, territorioExperienciaId);
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

        private IEnumerable<PeriodoEscolar> ObterPeriodoEscolar(string turmaId, int anoLetivo)
        {
            var turma = repositorioTurma.ObterPorId(turmaId);
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }
            var modalidade = turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo de calendário não encontrado.");
            }

            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
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
