using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarAtribuicaoPendenciasUsuariosUseCase : AbstractUseCase, ITratarAtribuicaoPendenciasUsuariosUseCase
    {
        public TratarAtribuicaoPendenciasUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroTratamentoAtribuicaoPendenciaDto>();

            var perfisPendencia = await mediator.Send(new ObterPendenciaPerfilPorPendenciaIdQuery(filtro.PendenciaId));

            var perfisPendenciaSemAtribuicao = perfisPendencia.Where(c => !c.PendenciasPerfilUsuarios.Any());

            foreach (var pendenciaPerfil in perfisPendenciaSemAtribuicao)
            {
                switch (pendenciaPerfil.PerfilCodigo)
                {
                    case Dominio.PerfilUsuario.CP:
                    case Dominio.PerfilUsuario.AD:
                    case Dominio.PerfilUsuario.DIRETOR:
                        await TratarAtribuicaoPerfisGestao(filtro.UeId, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        break;
                        await TratarAtribuicaoPerfisGestao(filtro.UeId, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        break;
                    case Dominio.PerfilUsuario.CEFAI:
                        var dre = await ObterCodigoDREUE(filtro.UeId);
                        var CEFAIs = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dre.DreCodigo));
                        await TratarAtribuicaoPerfilCEFAI(CEFAIs, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        break;

                    case Dominio.PerfilUsuario.ADMUE:
                        var ue = await ObterCodigoDREUE(filtro.UeId);
                        var admsUE = await ObterAdministradoresPorUE(ue.UeCodigo);
                        await TratarAtribuicaoPerfilAdmUe(admsUE, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        break;
                    case Dominio.PerfilUsuario.ADMSME:
                    case Dominio.PerfilUsuario.ADMCOTIC:
                    case Dominio.PerfilUsuario.ADMDRE:
                    case Dominio.PerfilUsuario.SECRETARIO:
                    case Dominio.PerfilUsuario.SUPERVISOR:
                    case Dominio.PerfilUsuario.PAEE:
                    case Dominio.PerfilUsuario.PAP:
                    case Dominio.PerfilUsuario.POEI:
                    case Dominio.PerfilUsuario.POED:
                    case Dominio.PerfilUsuario.POSL:
                    case Dominio.PerfilUsuario.COMUNICADOS_UE:
                        break;
                    default:
                        break;
                }

            }

            return true;
        }

        private async Task TratarAtribuicaoPerfisGestao(long ueId, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            var dreUe = await ObterCodigoDREUE(ueId);
            var tipoEscola = await ObterTipoEscolaUE(dreUe.UeCodigo);

            if (tipoEscola == TipoEscola.CIEJA)
            {
                await AtribuirPerfisUsuarioPorFuncaoAtividade(dreUe.UeCodigo, perfilUsuario, pendenciaPerfilId);
                return;
            }

            if (tipoEscola == TipoEscola.CEIINDIR || tipoEscola == TipoEscola.CRPCONV)
            {
                await AtribuirPerfisUsuarioPorFuncaoExterna(dreUe.UeCodigo, perfilUsuario, pendenciaPerfilId);
                return;
            }

            var atribuirPerfisPorCargo = await AtribuirPerfisUsuarioPorCargo(dreUe.UeCodigo, perfilUsuario, pendenciaPerfilId);
            if (!atribuirPerfisPorCargo)
                await AtribuirPerfisUsuarioPorFuncaoExterna(dreUe.UeCodigo, perfilUsuario, pendenciaPerfilId);
        }

        private async Task TratarAtribuicaoPerfilCEFAI(IEnumerable<long> CEFAIs, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            foreach (var cefai in CEFAIs)
                await AtribuirPerfilUsuario(cefai, perfilUsuario, pendenciaPerfilId);
        }

        private async Task TratarAtribuicaoPerfilAdmUe(IEnumerable<long> AdmsUe, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            foreach (var adm in AdmsUe)
                await AtribuirPerfilUsuario(adm, perfilUsuario, pendenciaPerfilId);
        }

        private async Task<long> ObterUsuarioId(string rf)
            => await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rf));

        private async Task<List<long>> ObterAdministradoresPorUE(string CodigoUe)
        {
            var administradoresId = await mediator.Send(new ObterAdministradoresPorUEQuery(CodigoUe));
            var AdministradoresUeId = new List<long>();

            foreach (var adm in administradoresId)
            {
                AdministradoresUeId.Add(await ObterUsuarioId(adm));
            }
            return AdministradoresUeId;
        }

        private async Task<DreUeCodigoDto> ObterCodigoDREUE(long ueId)
            => await mediator.Send(new ObterCodigoUEDREPorIdQuery(ueId));

        private async Task<TipoEscola> ObterTipoEscolaUE(string codigoUE)
            => await mediator.Send(new ObterTipoEscolaPorCodigoUEQuery(codigoUE));
 

        private async Task AtribuirPerfilUsuario(long usuarioId, PerfilUsuario perfil, long pendenciaPerfilId)
        {
            bool verificaExistenciaPendencia = await mediator.Send(new VerificaExistenciaDePendenciaPerfilUsuarioQuery(pendenciaPerfilId, usuarioId));

            if (!verificaExistenciaPendencia)
                await mediator.Send(new SalvarPendenciaPerfilUsuarioCommand(pendenciaPerfilId, usuarioId, perfil));
        }

        private async Task<bool> AtribuirPerfisUsuarioPorCargo(string ueCodigo, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            var funcionariosCargo = await mediator.Send(new ObterFuncionariosPorCargoHierarquicoQuery(ueCodigo, perfilUsuario.ObterCargoPorPerfil()));

            if (funcionariosCargo.NaoEhNulo() && funcionariosCargo.Any())
            {
                foreach (var funcionario in funcionariosCargo)
                {
                    var usuarioId = await ObterUsuarioId(funcionario.FuncionarioRF);
                    await AtribuirPerfilUsuario(usuarioId, funcionario.CargoId.ObterPerfilPorCargo(), pendenciaPerfilId);
                }
                return true;
            }
            return false;
        }

        private async Task<bool> AtribuirPerfisUsuarioPorFuncaoExterna(string ueCodigo, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            var funcionariosFuncaoExterna = await mediator.Send(new ObterFuncionariosPorFuncaoExternaHierarquicoQuery(ueCodigo, perfilUsuario.ObterFuncaoExternaPorPerfil()));
            if (funcionariosFuncaoExterna.NaoEhNulo() && funcionariosFuncaoExterna.Any())
            {
                foreach (var funcionario in funcionariosFuncaoExterna)
                {
                    var usuarioId = await ObterUsuarioId(funcionario.FuncionarioCpf);
                    await AtribuirPerfilUsuario(usuarioId, funcionario.FuncaoExternaId.ObterPerfilPorFuncaoExterna(), pendenciaPerfilId);
                }
                return true;
            }
            return false;
        }

        private async Task<bool> AtribuirPerfisUsuarioPorFuncaoAtividade(string ueCodigo, PerfilUsuario perfilUsuario, long pendenciaPerfilId)
        {
            var funcionariosFuncoeAtividade = await mediator.Send(new ObterFuncionariosPorFuncaoAtividadeHierarquicoQuery(ueCodigo, perfilUsuario.ObterFuncaoAtividadePorPerfil()));
            if (funcionariosFuncoeAtividade.NaoEhNulo() && funcionariosFuncoeAtividade.Any())
            {
                foreach (var funcionario in funcionariosFuncoeAtividade)
                {
                    var usuarioId = await ObterUsuarioId(funcionario.FuncionarioRf);
                    await AtribuirPerfilUsuario(usuarioId, funcionario.CodigoFuncaoAtividade.ObterPerfilPorFuncaoAtividade(), pendenciaPerfilId);
                }
                return true;
            }
            return false;
        }
    }
}
