using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPAAIPorDreUseCase : AbstractUseCase, IObterFuncionariosPAAIPorDreUseCase
    {
        public ObterFuncionariosPAAIPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> Executar(long dreId)
        {
            var dre = await mediator.Send(new ObterDREPorIdQuery(dreId));

            if (dre == null)
                throw new NegocioException("A DRE informada não foi encontrada");


            return await mediator.Send(new ObterFuncionariosPorDreECargoQuery(dre.CodigoDre, 29));
        }
    }
}
