using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class PesquisaResponsavelPlanoPorDreUEUseCase : AbstractUseCase, IPesquisaResponsavelPlanoPorDreUEUseCase
    {
        public PesquisaResponsavelPlanoPorDreUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<UsuarioEolRetornoDto>> Executar(FiltroPesquisaFuncionarioDto request)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var codigoDre = request.CodigoDRE;
            var codigoUe = request.CodigoUE;

            if (!string.IsNullOrEmpty(request.CodigoTurma))
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(request.CodigoTurma));
                codigoDre = turma?.Ue.Dre.CodigoDre;
                codigoUe = turma?.Ue.CodigoUe;
            }

            var funcionarios = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery(request.CodigoRF, request.Nome, codigoDre, codigoUe, usuario: usuario));
            var funcionariosFiltrados = funcionarios.GroupBy(x => x.UsuarioId).SelectMany(y => y.OrderBy(a => a.UsuarioId).Take(1));
            var limite = request.Limite > 0 ? request.Limite : 10;

            return new PaginacaoResultadoDto<UsuarioEolRetornoDto>()
            {
                Items = funcionariosFiltrados
                    .OrderBy(a => a.NomeServidor)
                    .Take(limite),
                TotalPaginas = 1,
                TotalRegistros = Math.Min(funcionariosFiltrados.Count(), limite)
            };
        }
    }
}
