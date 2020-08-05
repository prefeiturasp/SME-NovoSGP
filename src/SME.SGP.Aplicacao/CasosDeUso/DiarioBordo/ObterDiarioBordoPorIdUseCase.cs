using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class ObterDiarioBordoPorIdUseCase
    {
        public static async Task<DiarioBordoDto> Executar(IMediator mediator, long diarioBordoId)
        {
            DiarioBordo diarioBordo = await mediator.Send(new ObterDiarioBordoPorIdQuery(diarioBordoId));
            if (diarioBordo == null || diarioBordo.Excluido)
                throw new NegocioException($"Diário de Bordo de id {diarioBordoId} não encontrado");

            var dto = MapearParaDto(diarioBordo);

            return dto;
        }

        private static DiarioBordoDto MapearParaDto(DiarioBordo diarioBordo)
        {
            return new DiarioBordoDto
            {
                AulaId = diarioBordo.AulaId,
                DevolutivaId = diarioBordo.DevolutivaId,
                Excluido = diarioBordo.Excluido,
                Migrado = diarioBordo.Migrado,
                Planejamento = diarioBordo.Planejamento,
                ReflexoesReplanejamento = diarioBordo.ReflexoesReplanejamento
            };
        }
    }
}
