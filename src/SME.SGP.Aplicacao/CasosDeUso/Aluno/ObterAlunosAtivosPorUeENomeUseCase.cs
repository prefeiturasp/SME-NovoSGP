using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
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
            
            var listaRetorno = new List<AlunoParaAutoCompleteAtivoDto>();

            var listaAlunosEol = await mediator.Send(new ObterAlunosAtivosPorUeENomeQuery(filtro.UeCodigo, filtro.DataReferencia, filtro.AlunoNome, alunoCodigo));
          
            //TODO: Proteção para caso a sincronização não esteja 100%
            if (listaAlunosEol.Any())
            {
                var turmasCodigos = listaAlunosEol.Select(a => a.CodigoTurma.ToString()).Distinct().ToList();
           
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));

                foreach (var item in turmas)
                {
                    listaRetorno.AddRange(listaAlunosEol.Where(a => a.CodigoTurma == long.Parse(item.CodigoTurma)).ToList());;
                }                
            }
            resultado.Items = listaRetorno.OrderBy( a => a.NomeFinal).ToList();
            resultado.TotalPaginas = 1;
            resultado.TotalRegistros = resultado.Items.Count();

            return resultado;
        }
    }
}
