using MediatR;
using SME.SGP.Aplicacao.Queries.DiarioBordo.ObterDiarioBordoPorId;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.DiarioBordo
{
    public static class ObterDiarioBordoPorIdUseCase
    {
        public static async Task<DiarioBordoDto> Executar(IMediator mediator, long diarioBordoId)
        {
            var diarioBordo = await mediator.Send(new ObterDiarioBordoPorIdQuery(diarioBordoId));
            if (diarioBordo == null || diarioBordo.Excluido)
                throw new NegocioException($"Diário de Bordo de id {diarioBordoId} não encontrado");

        }
    }
}
