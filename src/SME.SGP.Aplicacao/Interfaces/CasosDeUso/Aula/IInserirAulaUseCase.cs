using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Aula;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IInserirAulaUseCase
    {
        Task<RetornoBaseDto> Executar(IMediator mediator, InserirAulaDto inserirAulaDto);
    }
}