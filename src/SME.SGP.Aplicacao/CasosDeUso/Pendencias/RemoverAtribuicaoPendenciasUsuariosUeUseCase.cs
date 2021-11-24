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
    public class RemoverAtribuicaoPendenciasUsuariosUeUseCase : AbstractUseCase, IRemoverAtribuicaoPendenciasUsuariosUeUseCase
    {
        public RemoverAtribuicaoPendenciasUsuariosUeUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var filtro = mensagem.ObterObjetoMensagem<FiltroRemoverAtribuicaoPendenciaDto>();

                var dicUePerfilCodigoFuncionarios = new Dictionary<string, IEnumerable<FuncionarioCargoDTO>>();
                var lstCefais = new List<long>();
                var lstAdmUes = new List<long>();

                var agruparUePerfilCodigo = ObterAgrupamentoUePerfil(filtro.PendenciasFuncionarios);

                await ObterFuncionariosCefaisAdmUes(agruparUePerfilCodigo, dicUePerfilCodigoFuncionarios, lstCefais, lstAdmUes);

                foreach (var pendenciaFuncionario in filtro.PendenciasFuncionarios)
                {
                    var validaExistenciaFunc = dicUePerfilCodigoFuncionarios.FirstOrDefault(w => w.Key == $"{pendenciaFuncionario.UeId}_{pendenciaFuncionario.PerfilCodigo}");
                    bool eraCefai = false;
                    bool eraAdmUe = false;

                    FuncionarioCargoDTO funcionarioAtual = null;
                    if (pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.CP || pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.AD || pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.DIRETOR)
                    {
                        funcionarioAtual = dicUePerfilCodigoFuncionarios.FirstOrDefault(w => w.Key == $"{pendenciaFuncionario.UeId}_{pendenciaFuncionario.PerfilCodigo}")
                                                                                                   .Value
                                                                                                   .FirstOrDefault(s => s.FuncionarioRF.Equals(pendenciaFuncionario.CodigoRf));
                    }
                    else if(pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.CEFAI)   
                        eraCefai = lstCefais.Any(usuarioCefai => usuarioCefai != pendenciaFuncionario.UsuarioId);
                    else if (pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.ADMUE)
                        eraAdmUe = lstAdmUes.Any(usuarioAdmUe => usuarioAdmUe != pendenciaFuncionario.UsuarioId);

                    var filtroPendenciaPerfilUsuarioCefaiAdmUeDto = new FiltroPendenciaPerfilUsuarioCefaiAdmUeDto(funcionarioAtual, eraCefai, eraAdmUe, pendenciaFuncionario);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario, filtroPendenciaPerfilUsuarioCefaiAdmUeDto, Guid.NewGuid(), null));
                }
            }
            catch (Exception)
            {
                throw new Exception($"Erro na remoção de atribuição de Pendência Perfil Usuário por UE.");
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
