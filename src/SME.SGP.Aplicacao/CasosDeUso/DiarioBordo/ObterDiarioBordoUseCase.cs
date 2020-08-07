using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoUseCase : AbstractUseCase, IObterDiarioBordoUseCase
    {
        public ObterDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DiarioBordoDto> Executar(long aulaId)
        {
            Aula aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));
            if (aula == null || aula.Excluido)
                throw new NegocioException($"Aula de id {aulaId} não encontrada");

            var aberto = await AulaDentroDoPeriodo(mediator, aula.TurmaId, aula.DataAula);

            DiarioBordo diarioBordo = await mediator.Send(new ObterDiarioBordoPorAulaIdQuery(aulaId));
            if (diarioBordo == null || diarioBordo.Excluido)
            {
                diarioBordo = new DiarioBordo
                {
                    AulaId = aulaId
                };

                return MapearParaDto(diarioBordo, aberto);
            }

            var dto = MapearParaDto(diarioBordo, aberto);

            return dto;
        }

        private async Task<bool> AulaDentroDoPeriodo(IMediator mediator, string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery()
            {
                Turma = turma,
                DataReferencia = dataAula
            });

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }

        private DiarioBordoDto MapearParaDto(DiarioBordo diarioBordo, bool aberto)
        {
            return new DiarioBordoDto
            {
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Excluido = diarioBordo.Excluido,
                Migrado = diarioBordo.Migrado,
                Planejamento = diarioBordo.Planejamento,
                ReflexoesReplanejamento = diarioBordo.ReflexoesReplanejamento,
                TemPeriodoAberto = aberto,
                Auditoria = (AuditoriaDto)diarioBordo
            };
        }
    }
}
