using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosPorUeENomeUseCase : AbstractUseCase, IObterAlunosAtivosPorUeENomeUseCase
    {
        public ObterAlunosAtivosPorUeENomeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AlunoParaAutoCompleteAtivoDto>> Executar(FiltroBuscaEstudantesAtivoDto filtro)
        {            
            var resultado = new PaginacaoResultadoDto<AlunoParaAutoCompleteAtivoDto>();
            long alunoCodigo = filtro.AlunoCodigo ?? 0;
            resultado.Items = await mediator.Send(new ObterAlunosAtivosPorUeENomeQuery(filtro.UeCodigo, filtro.DataReferencia, filtro.AlunoNome, alunoCodigo));
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
