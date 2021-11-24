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

            foreach(var pendenciaPerfil in perfisPendenciaSemAtribuicao)
            {
                switch (pendenciaPerfil.PerfilCodigo)
                {
                    case Dominio.PerfilUsuario.CP:
                    case Dominio.PerfilUsuario.AD:
                    case Dominio.PerfilUsuario.DIRETOR:
                        var dreUe = await ObterCodigoDREUE(filtro.UeId);
                        var funcionarios = await mediator.Send(new ObterFuncionariosPorCargoHierarquicoQuery(dreUe.UeCodigo, pendenciaPerfil.PerfilCodigo.ObterCargoPorPerfil()));

                        foreach (var funcionario in funcionarios)
                        {
                            var usuarioId = await ObterUsuarioId(funcionario.FuncionarioRF);
                            await AtribuirPerfilUsuario(usuarioId, funcionario.CargoId.ObterPerfilPorCargo(), pendenciaPerfil.Id);
                        }

                        break;
                    case Dominio.PerfilUsuario.CEFAI:
                        var dre = await ObterCodigoDREUE(filtro.UeId);
                        var CEFAIs = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dre.DreCodigo));

                        foreach(var cefai in CEFAIs)
                        {
                            await AtribuirPerfilUsuario(cefai, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        }

                        break;
                    case Dominio.PerfilUsuario.ADMUE:
                        var ue = await ObterCodigoDREUE(filtro.UeId);
                        var admsUE = await ObterAdministradoresPorUE(ue.UeCodigo);

                        foreach(var adm in admsUE)
                        {
                            await AtribuirPerfilUsuario(adm, pendenciaPerfil.PerfilCodigo, pendenciaPerfil.Id);
                        }

                        break;
                    case Dominio.PerfilUsuario.ADMSME:
                        break;
                    case Dominio.PerfilUsuario.ADMCOTIC:
                        break;
                    case Dominio.PerfilUsuario.ADMDRE:
                        break;
                    case Dominio.PerfilUsuario.POA:
                        break;
                    case Dominio.PerfilUsuario.SECRETARIO:
                        break;
                    case Dominio.PerfilUsuario.SUPERVISOR:
                        break;
                    case Dominio.PerfilUsuario.PAEE:
                        break;
                    case Dominio.PerfilUsuario.PAP:
                        break;
                    case Dominio.PerfilUsuario.POEI:
                        break;
                    case Dominio.PerfilUsuario.POED:
                        break;
                    case Dominio.PerfilUsuario.POSL:
                        break;
                    case Dominio.PerfilUsuario.COMUNICADOS_UE:
                        break;
                    default:
                        break;
                }

            }

            return true;
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

        private async Task<DreUeDto> ObterCodigoDREUE(long ueId)
            => await mediator.Send(new ObterCodigoUEDREPorIdQuery(ueId));

        private async Task AtribuirPerfilUsuario(long usuarioId, PerfilUsuario perfil, long pendenciaPerfilId)
        {
            bool verificaExistenciaPendencia = await mediator.Send(new VerificaExistenciaDePendenciaPerfilUsuarioQuery(pendenciaPerfilId, usuarioId));

            if(!verificaExistenciaPendencia)
                await mediator.Send(new SalvarPendenciaPerfilUsuarioCommand(pendenciaPerfilId, usuarioId, perfil));
        }
    }
}
