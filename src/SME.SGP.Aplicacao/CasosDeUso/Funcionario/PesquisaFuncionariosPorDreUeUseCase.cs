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
    public class PesquisaFuncionariosPorDreUeUseCase : AbstractUseCase, IPesquisaFuncionariosPorDreUeUseCase
    {
        public PesquisaFuncionariosPorDreUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<UsuarioEolRetornoDto>> Executar(FiltroPesquisaFuncionarioDto request)
        {
            var funcionarios = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery(request.CodigoRF, request.Nome, request.CodigoDRE, request.CodigoUE, request.CodigoTurma));
            var limite = request.Limite > 0 ? request.Limite : 10;

            return new PaginacaoResultadoDto<UsuarioEolRetornoDto>()
            {
                Items = funcionarios.OrderBy(a => a.NomeServidor).Take(limite),
                TotalPaginas = 1,
                TotalRegistros = Math.Min(funcionarios.Count(), limite)
            };
        }
    }
}
