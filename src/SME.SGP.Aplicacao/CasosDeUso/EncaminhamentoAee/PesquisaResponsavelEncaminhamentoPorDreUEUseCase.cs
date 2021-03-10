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
            var codigos = await ObterCodigos(request.CodigoTurma, request.CodigoDRE, usuario);
            var funcaoAtividadePesquisa = ObterFuncaoAtividadeAPesquisarPorPerfil(usuario.PerfilAtual);

            var funcionarios = await mediator.Send(new PesquisaFuncionariosPorDreUeQuery(request.CodigoRF, request.Nome, codigos.codigoDRE, codigos.codigoUE, usuario: usuario));
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

        private async Task<(string codigoDRE, string codigoUE)> ObterCodigos(string codigoTurma, string codigoDRE, Usuario usuario)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(codigoTurma));
            return usuario.EhCoordenadorCEFAI() ?
                (turma.Ue.Dre.CodigoDre, "") :
                (turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe);
        }

        // CEFAI Pesquisa por perfil PAAI pois a abrangencia é DRE, outros perfis pesquisa PAEE com abrangencia UE
        private int ObterFuncaoAtividadeAPesquisarPorPerfil(Guid perfilAtual)
            => perfilAtual == Perfis.PERFIL_CEFAI ? FUNCAO_ATIVIDADE_PAAI : FUNCAO_ATIVIDADE_PAEE;
    }
}
