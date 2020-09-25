using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivaPorIdUseCase : AbstractUseCase, IObterDevolutivaPorIdUseCase
    {
        public ObterDevolutivaPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<DevolutivaDto> Executar(long devolutivaId)
        {
            var devolutivaDto = await ObterDevolutiva(devolutivaId);
            devolutivaDto.DiariosIds = await ObterIdsDiariosBordo(devolutivaId);

            return devolutivaDto;
        }

        private async Task<IEnumerable<long>> ObterIdsDiariosBordo(long devolutivaId)
            => await mediator.Send(new ObterIdsDiariosBordoPorDevolutivaQuery(devolutivaId));

        private async Task<DevolutivaDto> ObterDevolutiva(long devolutivaId)
        {
            var devolutiva = await mediator.Send(new ObterDevolutivaPorIdQuery(devolutivaId));

            if (devolutiva == null)
                throw new NegocioException("Devolutiva não localizada");

            return MapearParaDto(devolutiva);
        }

        private DevolutivaDto MapearParaDto(Devolutiva devolutiva)
            => new DevolutivaDto()
            {
                CodigoComponenteCurricular = devolutiva.CodigoComponenteCurricular,
                Descricao = devolutiva.Descricao,
                PeriodoInicio = devolutiva.PeriodoInicio,
                PeriodoFim = devolutiva.PeriodoFim,
                Auditoria = (AuditoriaDto)devolutiva
            };
    }
}
