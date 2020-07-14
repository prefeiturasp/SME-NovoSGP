using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorCodigoEolNomeUseCase : AbstractUseCase, IObterAlunosPorCodigoEolNomeUseCase
    {
        public ObterAlunosPorCodigoEolNomeUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<PaginacaoResultadoDto<AlunoSimplesDto>> Executar(FiltroBuscaAlunosDto dto)
        {
            var resultado = new PaginacaoResultadoDto<AlunoSimplesDto>();

            resultado.Items = await mediator.Send(new ObterAlunosPorCodigoEolNomeQuery(dto));
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
