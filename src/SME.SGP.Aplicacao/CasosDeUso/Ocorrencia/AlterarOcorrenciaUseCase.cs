using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaUseCase : AbstractUseCase, IAlterarOcorrenciaUseCase
    {
        public AlterarOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarOcorrenciaDto dto)
        {
            var retorno = await mediator.Send(new AlterarOcorrenciaCommand(dto.Id, dto.DataOcorrencia, dto.HoraOcorrencia, dto.Titulo, dto.Descricao, dto.OcorrenciaTipoId, dto.CodigosAlunos));
            return retorno;
        }
    }
}
