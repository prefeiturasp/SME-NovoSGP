using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAlunosDaTurmaUseCase : AbstractUseCase, IObterListaAlunosDaTurmaUseCase
    {
        public ObterListaAlunosDaTurmaUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AlunoSimplesDto>> Executar(string turmaCodigo)
        {
            var resultado = new PaginacaoResultadoDto<AlunoSimplesDto>();

            resultado.Items = await mediator.Send(new ObterAlunosSimplesDaTurmaQuery(turmaCodigo));
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
