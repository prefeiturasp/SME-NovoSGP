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

        public async Task<DiarioBordoDto> Executar(long id)
        {
            var diarioBordo = await mediator.Send(new ObterDiarioDeBordoPorIdQuery(id));
            if (diarioBordo == null)
                throw new NegocioException($"Diário de bordo não encontrado", 204);

            if (diarioBordo.DevolutivaId != null)
            {
                var devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(diarioBordo.DevolutivaId.GetValueOrDefault()));

                if (devolutiva != null)
                    diarioBordo.Devolutivas = devolutiva.Descricao;
            }

            Aula aula = null;

            if (diarioBordo.AulaId > 0)
            {
                aula = await mediator.Send(new ObterAulaPorIdQuery(diarioBordo.AulaId));
            }

            var dto = MapearParaDto(diarioBordo, aula);

            return dto;
        }

        private DiarioBordoDto MapearParaDto(DiarioBordoDetalhesDto diarioBordo, Aula aula)
        {
            AulaDto aulaDto = MapearAulaDto(aula);

            return new DiarioBordoDto
            {
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Devolutivas = diarioBordo.Devolutivas,
                Planejamento = diarioBordo.Planejamento,
                ReflexoesReplanejamento = diarioBordo.ReflexoesReplanejamento,
                Auditoria = diarioBordo.Auditoria,
                Aula = aulaDto,
                Data = aulaDto?.DataAula,
                Observacoes = diarioBordo.Observacoes
            };
        }

        private AulaDto MapearAulaDto(Aula aula)
        {
            return new AulaDto()
            {
                AulaCJ = aula.AulaCJ,
                DataAula = aula.DataAula,
                DisciplinaCompartilhadaId = aula.DisciplinaCompartilhadaId,
                DisciplinaId = aula.DisciplinaId,
                DisciplinaNome = aula.DisciplinaNome,
                Id = Convert.ToInt32(aula.Id),
                Quantidade = aula.Quantidade,
                RecorrenciaAula = aula.RecorrenciaAula,
                TipoAula = aula.TipoAula,
                TipoCalendarioId = aula.TipoCalendarioId,
                TurmaId = aula.TurmaId,
                UeId = aula.UeId
            };
        }
    }
}
