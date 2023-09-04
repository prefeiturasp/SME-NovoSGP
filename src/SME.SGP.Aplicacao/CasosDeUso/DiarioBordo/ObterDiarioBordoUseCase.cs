﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoUseCase : AbstractUseCase, IObterDiarioBordoUseCase
    {
        public ObterDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DiarioBordoDto> Executar(long aulaId, long componenteCurricularId)
        {
            Aula aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));
            if (aula == null || aula.Excluido)
                throw new NegocioException($"Diário de bordo não encontrado", 204);

            var aberto = await AulaDentroDoPeriodo(mediator, aula.TurmaId, aula.DataAula);

            IEnumerable<DiarioBordo> diariosBordos = await mediator.Send(new ObterDiariosDeBordosPorAulaQuery(aulaId));

            var diarioBordo = diariosBordos.FirstOrDefault(diario => diario.ComponenteCurricularId == componenteCurricularId);
            var diarioBordoIrmao = diariosBordos.FirstOrDefault(diario => diario.ComponenteCurricularId != componenteCurricularId);
            
            var codigosComponentes = diariosBordos.Select(diario => diario.ComponenteCurricularId).ToList();
            codigosComponentes.Add(componenteCurricularId);
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(codigosComponentes.Distinct().ToArray()));

            if (diarioBordo == null)
                return MapearParaDto(new DiarioBordo() { AulaId = aulaId, ComponenteCurricularId = componenteCurricularId }, aberto, diarioBordoIrmao, componentes);
            
            if (diarioBordo.DevolutivaId != null)
                diarioBordo.Devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(diarioBordo.DevolutivaId.GetValueOrDefault()));

            var dto = MapearParaDto(diarioBordo, aberto, diarioBordoIrmao, componentes);

            return dto;
        }

        private async Task<bool> AulaDentroDoPeriodo(IMediator mediator, string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(dataAula, turma));

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }

        private DiarioBordoDto MapearParaDto(DiarioBordo diarioBordo, bool aberto, DiarioBordo diarioBordoIrmao, IEnumerable<DisciplinaDto> disciplinas)
        {
            return new DiarioBordoDto
        {
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Devolutivas = diarioBordo.Devolutiva?.Descricao,
                Planejamento = diarioBordo.Planejamento,
                Excluido = diarioBordo.Excluido,
                Migrado = diarioBordo.Migrado,
                TemPeriodoAberto = aberto,
                Auditoria = (AuditoriaDto)diarioBordo,
                InseridoCJ = diarioBordo.InseridoCJ,
                PlanejamentoIrmao = diarioBordoIrmao?.Planejamento,
                NomeComponente = disciplinas.FirstOrDefault(disciplina => disciplina.CodigoComponenteCurricular == diarioBordo.ComponenteCurricularId)?.NomeComponenteInfantil,
                NomeComponenteIrmao = diarioBordoIrmao != null ? disciplinas.FirstOrDefault(disciplina => disciplina.CodigoComponenteCurricular == diarioBordoIrmao.ComponenteCurricularId)?.NomeComponenteInfantil : string.Empty
            };
        }
    }
}
