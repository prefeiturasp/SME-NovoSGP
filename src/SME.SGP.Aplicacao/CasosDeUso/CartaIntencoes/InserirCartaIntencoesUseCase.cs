using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class InserirCartaIntencoesUseCase : AbstractUseCase, IInserirCartaIntencoesUseCase
    {
        public InserirCartaIntencoesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<List<AuditoriaDto>> Executar(InserirCartaIntencoesDto param)
        {
            List<AuditoriaDto> auditorias = new List<AuditoriaDto>();

            foreach (CartaIntencoesDto carta in param.Cartas)
            {
                AuditoriaDto auditoria = await mediator.Send(new InserirCartaIntencoesCommand(carta));

                auditorias.Add(auditoria);
            }

            return auditorias;
        }
    }
}
