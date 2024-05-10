using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
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
        { }

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

                //funciona para criar PendenciaPerfilUsuario para PendenciaPerfil que já tenha atribuição
                await VerificaPerfisNovosEOL(dicUePerfilCodigoFuncionarios, filtro.PendenciasFuncionarios);

                foreach (var pendenciaFuncionario in filtro.PendenciasFuncionarios)
                {
                    var dadosFuncionarioPendencia = ObterDadosFuncionarioPendencia(pendenciaFuncionario, dicUePerfilCodigoFuncionarios, lstCefais, lstAdmUes);
                    var filtroPendenciaPerfilUsuarioCefaiAdmUeDto = new FiltroPendenciaPerfilUsuarioCefaiAdmUeDto(dadosFuncionarioPendencia.FuncionarioAtual,
                                                                                                                  dadosFuncionarioPendencia.EraCefai,
                                                                                                                  dadosFuncionarioPendencia.EraAdmUe, pendenciaFuncionario);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario, filtroPendenciaPerfilUsuarioCefaiAdmUeDto, Guid.NewGuid(), null));
                }
            }
            catch (Exception)
            {
                throw new NegocioException($"Erro na remoção de atribuição de Pendência Perfil Usuário por UE.");
            }
            return true;
        }

        private (FuncionarioCargoDTO FuncionarioAtual, bool EraCefai, bool EraAdmUe) ObterDadosFuncionarioPendencia(PendenciaPerfilUsuarioDto pendenciaFuncionario,
                                                                                                                    Dictionary<string, IEnumerable<FuncionarioCargoDTO>> dicUePerfilCodigoFuncionarios,
                                                                                                                    List<long> lstCefais, List<long> lstAdmUes)
        {
            if (pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.CP || pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.AD || pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.DIRETOR)
            {
                var funcionarioAtual = (dicUePerfilCodigoFuncionarios.PossuiRegistros())
                                    ? dicUePerfilCodigoFuncionarios.FirstOrDefault(w => w.Key == $"{pendenciaFuncionario.UeId}_{pendenciaFuncionario.PerfilCodigo}")
                                                                                            .Value
                                                                                            .FirstOrDefault(s => s.FuncionarioRF.Equals(pendenciaFuncionario.CodigoRf))
                                                                                            : null;
                return (funcionarioAtual, false, false);
            }

            if (pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.CEFAI)
                return (null,
                        lstCefais.NaoPossuiRegistros() || lstCefais.Any(usuarioCefai => usuarioCefai != pendenciaFuncionario.UsuarioId),
                        false);

            if (pendenciaFuncionario.PerfilCodigo == (int)PerfilUsuario.ADMUE)
                return (null,
                        false,
                        lstAdmUes.NaoPossuiRegistros() || lstAdmUes.Any(usuarioAdmUe => usuarioAdmUe != pendenciaFuncionario.UsuarioId));

            return (null, false, false);
        }

        private async Task VerificaPerfisNovosEOL(Dictionary<string, IEnumerable<FuncionarioCargoDTO>> dicUePerfilCodigoFuncionarios, IEnumerable<PendenciaPerfilUsuarioDto> filtro)
        {
            foreach (var dados in dicUePerfilCodigoFuncionarios)
            {
                foreach (var valorDados in dados.Value)
                {
                    if (!filtro.FuncionarioPossuiPendenciaPerfil(valorDados.FuncionarioRF))
                    {
                        var (PerfilCodigo, _) = ObterParametrosKey(dados);
                        if (dados.ExistemNFuncionariosMesmoPerfil())
                            await GerarPendenciasPerfilUsuario(PerfilCodigo, 
                                                               filtro, 
                                                               valorDados.FuncionarioRF);
                    }
                }
            }
        }

        private async Task GerarPendenciasPerfilUsuario(int perfilCodigo, IEnumerable<PendenciaPerfilUsuarioDto> filtro, string rfFuncionario)
        {
            var listaPendenciasParaOCargo = filtro.Where(pf => pf.PerfilCodigo == perfilCodigo);
            foreach (var valorLista in listaPendenciasParaOCargo)
            {
                var dadospendenciaPerfilId = await mediator.Send(new ObterPendenciaPerfilPorPendenciaIdQuery(valorLista.PendenciaId));
                var valorPendenciaPerfil = dadospendenciaPerfilId.Where(p => p.PerfilCodigo == (PerfilUsuario)perfilCodigo).Select(d => d.Id).FirstOrDefault();
                var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rfFuncionario));

                if (valorPendenciaPerfil > 0 && usuarioId > 0)
                    await mediator.Send(new SalvarPendenciaPerfilUsuarioCommand(valorPendenciaPerfil, usuarioId, (PerfilUsuario)perfilCodigo));
            }
        }

        private (int PerfilCodigo, long UeId) ObterParametrosKey(KeyValuePair<string, IEnumerable<FuncionarioCargoDTO>> valorDicionarioFuncionarioCargo)
        {
            var itensKey = valorDicionarioFuncionarioCargo.Key.Split('_');
            int perfilCodigo = Convert.ToInt32(itensKey[1]);
            long ueId = Convert.ToInt64(itensKey[0]);
            return (perfilCodigo, ueId);
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
    
                        if (funcionarios.NaoEhNulo() && funcionarios.Any())
                            dicUePerfilCodigoFuncionarios.Add($"{uePerfilCodigo.UeId}_{uePerfilCodigo.PerfilCodigo}", funcionarios);
                        
                        break;

                    case PerfilUsuario.CEFAI:
                        var cefais = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreUe.DreCodigo));

                        if (cefais.NaoEhNulo() && cefais.Any())
                            lstCefais.AddRange(cefais);

                        break;

                    case PerfilUsuario.ADMUE:
                        var admUes = await ObterAdministradoresPorUE(dreUe.UeCodigo);

                        if (admUes.NaoEhNulo() && admUes.Any())
                            lstAdmUes.AddRange(admUes);

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

        private async Task<DreUeCodigoDto> ObterCodigoDREUE(long ueId)
            => await mediator.Send(new ObterCodigoUEDREPorIdQuery(ueId));
    }
}
