using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAlunosAtivosNaTurmaUseCase: AbstractUseCase, IObterListaAlunosAtivosNaTurmaUseCase
    {
        public ObterListaAlunosAtivosNaTurmaUseCase(IMediator mediator): base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<AlunoSituacaoDto>> Executar(string turmaCodigo)
        {
            var resultado = new PaginacaoResultadoDto<AlunoSituacaoDto>();

            var alunosAtivosNaTurma = await mediator.Send(new ObterAlunosAtivosSimplesDaTurmaQuery(turmaCodigo));

            resultado.Items = alunosAtivosNaTurma;
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
