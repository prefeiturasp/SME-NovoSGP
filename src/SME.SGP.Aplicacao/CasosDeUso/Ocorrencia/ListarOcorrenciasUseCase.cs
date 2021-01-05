using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasUseCase : AbstractUseCase, IListarOcorrenciasUseCase
    {
        public ListarOcorrenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<OcorrenciaListagemDto>> Executar(FiltroOcorrenciaListagemDto dto)
        {
            var retorno = await mediator.Send(new ListarOcorrenciasQuery(dto.DataOcorrenciaInicio, dto.DataOcorrenciaFim, dto.AlunoNome, dto.Titulo, dto.TurmaId));
            return retorno;
        }
    }
}
