using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase : AbstractUseCase, IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase
    {
        public RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroPendenciaPerfilUsuarioCefaiAdmUeDto>();

                var perfilUsuario = (PerfilUsuario)filtro.PendenciaFuncionario.PerfilCodigo;

                switch (perfilUsuario)
                {
                    case PerfilUsuario.CP:
                    case PerfilUsuario.AD:
                    case PerfilUsuario.DIRETOR:

                        if (filtro.FuncionarioAtual.EhNulo() || (int)EnumHelper.ObterPerfilPorCargo(filtro.FuncionarioAtual.CargoId) != filtro.PendenciaFuncionario.PerfilCodigo)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);

                        break;

                    case PerfilUsuario.CEFAI:
                        if (filtro.EraCefai)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);
                        break;

                    case PerfilUsuario.ADMUE:
                        if (filtro.EraAdmUe)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);
                        break;
                }
            }
            catch (Exception)
            {
                throw new NegocioException($"Erro na remoção de atribuição de Pendência Perfil Usuário por UE e Funcionário.");
            }
            return true;
        }

        private async Task RemoverTratarAtribuicao(PendenciaPerfilUsuarioDto pendenciaFuncionario)
        {
            await mediator.Send(new ExcluirPendenciaPerfilUsuarioCommand(pendenciaFuncionario.Id));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaFuncionario.PendenciaId, pendenciaFuncionario.UeId.Value), Guid.NewGuid()));
        }
    }
}
