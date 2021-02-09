using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PesquisaResponsavelEncaminhamentoPorDreUEUseCase : AbstractUseCase, IPesquisaResponsavelEncaminhamentoPorDreUEUseCase
    {
        const int FUNCAO_ATIVIDADE_PAAI = 29;
        const int FUNCAO_ATIVIDADE_PAEE = 6;

        public PesquisaResponsavelEncaminhamentoPorDreUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<UsuarioEolRetornoDto>> Executar(FiltroPesquisaFuncionarioDto request)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var codigoDRE = await ObterCodigos(request.CodigoTurma, request.CodigoDRE);
            var funcaoAtividadePesquisa = ObterFuncaoAtividadeAPesquisarPorPerfil(usuario.PerfilAtual);

            var funcionarios = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery(request.CodigoRF, request.Nome, codigoDRE, usuario: usuario));
            var limite = request.Limite > 0 ? request.Limite : 10;

            return new PaginacaoResultadoDto<UsuarioEolRetornoDto>()
            {
                Items = funcionarios
                    .Where(c => c.CodigoFuncaoAtividade == funcaoAtividadePesquisa)
                    .OrderBy(a => a.NomeServidor)
                    .Take(limite),
                TotalPaginas = 1,
                TotalRegistros = Math.Min(funcionarios.Count(), limite)
            };
        }

        private async Task<string> ObterCodigos(string codigoTurma, string codigoDRE)
        {
            if (!string.IsNullOrEmpty(codigoTurma))
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
                return turma.Ue.Dre.CodigoDre;
            }

            return codigoDRE;
        }

        // CEFAI Pesquisa por perfil PAAI pois a abrangencia é DRE, outros perfis pesquisa PAEE com abrangencia UE
        private int ObterFuncaoAtividadeAPesquisarPorPerfil(Guid perfilAtual)
            => perfilAtual == Perfis.PERFIL_CEFAI ? FUNCAO_ATIVIDADE_PAAI : FUNCAO_ATIVIDADE_PAEE;
    }
}
