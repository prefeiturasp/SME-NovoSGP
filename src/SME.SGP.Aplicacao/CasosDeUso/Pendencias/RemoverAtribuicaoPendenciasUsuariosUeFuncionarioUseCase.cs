using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
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

                        if (filtro.FuncionarioAtual == null || (int)EnumHelper.ObterPerfilPorCargo(filtro.FuncionarioAtual.CargoId) != filtro.PendenciaFuncionario.PerfilCodigo)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);

                        break;

                    case PerfilUsuario.CEFAI:
                        if (filtro.EhCefai)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);
                        break;

                    case PerfilUsuario.ADMUE:
                        if (filtro.EhAdmUe)
                            await RemoverTratarAtribuicao(filtro.PendenciaFuncionario);
                        break;
                }
            }
            catch (Exception)
            {
                throw new Exception($"Erro na remoção de atribuição de Pendência Perfil Usuário por UE e Funcionário.");
            }
            return true;
        }

        private async Task RemoverTratarAtribuicao(PendenciaPerfilUsuarioDto pendenciaFuncionario)
        {
            await mediator.Send(new ExcluirPendenciaPerfilUsuarioCommand(pendenciaFuncionario.Id));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaFuncionario.PendenciaId, pendenciaFuncionario.UeId.Value), Guid.NewGuid()));
        }

        private IEnumerable<PendenciaPerfilUsuarioUePerfilDto> ObterAgrupamentoUePerfil(IEnumerable<PendenciaPerfilUsuarioDto> pendenciaFuncionarios)
        {
            var agruparUePerfilCodigoAnonymous = pendenciaFuncionarios.Where(w => w.UeId.HasValue).Select(s => new { UeId = s.UeId.Value, s.PerfilCodigo }).Distinct();
            return agruparUePerfilCodigoAnonymous.Select(s => new PendenciaPerfilUsuarioUePerfilDto() { UeId = s.UeId, PerfilCodigo = s.PerfilCodigo });
        }

        private async Task ObterFuncionariosCefaisAdmUes(IEnumerable<PendenciaPerfilUsuarioUePerfilDto> agruparUePerfilCodigo, Dictionary<string, IEnumerable<FuncionarioCargoDTO>> dicUePerfilCodigoFuncionarios, List<long> lstCefais, List<long> lstAdmUes)
        {
            foreach (var uePerfilCodigo in agruparUePerfilCodigo)
            {
                var dreUe = await ObterCodigoDREUE(uePerfilCodigo.UeId);

                var perfilUsuario = (PerfilUsuario)uePerfilCodigo.PerfilCodigo;

                switch (perfilUsuario)
                {
                    case PerfilUsuario.CP:
                    case PerfilUsuario.AD:
                    case PerfilUsuario.DIRETOR:
                        var funcionarios = await mediator.Send(new ObterFuncionariosPorCargoHierarquicoQuery(dreUe.UeCodigo, EnumHelper.ObterCargoPorPerfil(uePerfilCodigo.PerfilCodigo)));
                        dicUePerfilCodigoFuncionarios.Add($"{uePerfilCodigo.UeId}_{uePerfilCodigo.PerfilCodigo}", funcionarios);
                        break;

                    case PerfilUsuario.CEFAI:
                        lstCefais.AddRange(await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreUe.DreCodigo)));
                        break;

                    case PerfilUsuario.ADMUE:
                        lstAdmUes.AddRange(await ObterAdministradoresPorUE(dreUe.UeCodigo));
                        break;
                }
            }
        }

        private async Task<List<long>> ObterAdministradoresPorUE(string CodigoUe)
        {
            var administradoresId = await mediator.Send(new ObterAdministradoresPorUEQuery(CodigoUe));
            var AdministradoresUeId = new List<long>();

            foreach (var adm in administradoresId)
                AdministradoresUeId.Add(await ObterUsuarioId(adm));

            return AdministradoresUeId;
        }

        private async Task<long> ObterUsuarioId(string rf)
            => await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rf));

        private async Task<DreUeDto> ObterCodigoDREUE(long ueId)
            => await mediator.Send(new ObterCodigoUEDREPorIdQuery(ueId));
    }
}
