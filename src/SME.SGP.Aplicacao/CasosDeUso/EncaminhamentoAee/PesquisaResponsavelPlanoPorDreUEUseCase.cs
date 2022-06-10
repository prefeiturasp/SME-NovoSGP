using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PesquisaResponsavelPlanoPorDreUEUseCase : AbstractUseCase, IPesquisaResponsavelPlanoPorDreUEUseCase
    {
        public PesquisaResponsavelPlanoPorDreUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<UsuarioEolRetornoDto>> Executar(FiltroPesquisaFuncionarioDto request)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));
            var funcionarios = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery(request.CodigoRF, request.Nome, turma?.Ue.Dre.CodigoDre, turma?.Ue.CodigoUe, usuario: usuario));
            var limite = request.Limite > 0 ? request.Limite : 10;

            return new PaginacaoResultadoDto<UsuarioEolRetornoDto>()
            {
                Items = funcionarios
                    .OrderBy(a => a.NomeServidor)
                    .Take(limite),
                TotalPaginas = 1,
                TotalRegistros = Math.Min(funcionarios.Count(), limite)
            };
        }
    }
}
