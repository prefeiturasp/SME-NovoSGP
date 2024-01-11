using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Org.BouncyCastle.Asn1.Ocsp;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorPorRFUeDreAnoLetivoQueryHandler : IRequestHandler<ObterProfessorPorRFUeDreAnoLetivoQuery, ProfessorResumoDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterProfessorPorRFUeDreAnoLetivoQueryHandler(IHttpClientFactory httpClientFactory,IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<ProfessorResumoDto> Handle(ObterProfessorPorRFUeDreAnoLetivoQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            ValidarParametroDreId(request);
            var (paramUeId, paramDreId) = ObterParametrosDreUe(request);
            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_BUSCAR_POR_RF_DRE_UE, request.CodigoRF, request.AnoLetivo);
            var resposta = await httpClient.GetAsync($"{url}?{string.Concat(paramUeId, paramDreId)}buscarOutrosCargos={request.BuscarOutrosCargos}");
            var retorno = await TratarRepostaSemSucesso(resposta, request);
            if (retorno.NaoEhNulo())
                return retorno;

            var json = await resposta.Content.ReadAsStringAsync();
            retorno = JsonConvert.DeserializeObject<ProfessorResumoDto>(json);
            retorno.UsuarioId = await ObterIdUsuario(retorno.CodigoRF);
            return retorno;
        }

        private async Task<long> ObterIdUsuario(string codigoRf)
        {
            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRf));
            if (usuario.EhNulo())
                throw new NegocioException("Usuário não localizado.");
            return usuario.Id;
        }

        private async Task<ProfessorResumoDto> TratarRepostaSemSucesso(HttpResponseMessage resposta, ObterProfessorPorRFUeDreAnoLetivoQuery request)
        {
            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException("Ocorreu uma falha ao consultar o professor");

            if (resposta.StatusCode == HttpStatusCode.NoContent)
            {
                var dadosUsuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                var ehGestorEscolar = dadosUsuarioLogado.PossuiPerfilGestorEscolar();
                if (!dadosUsuarioLogado.EhProfessorCj() && !ehGestorEscolar)
                    throw new NegocioException($"Não foi encontrado professor com RF {request.CodigoRF}");

                if (ehGestorEscolar)
                {
                    if (await EhFuncionarioGestorEscolarDaUe(dadosUsuarioLogado.CodigoRf, request))
                        return new ProfessorResumoDto { CodigoRF = request.CodigoRF, Nome = dadosUsuarioLogado.Nome, UsuarioId = dadosUsuarioLogado.Id };
                }
                else
                {
                    var obterAtribuicoesCJAtivas = await mediator.Send(new ObterAtribuicoesCJAtivasQuery(request.CodigoRF, false));
                    if (!obterAtribuicoesCJAtivas.Any())
                        throw new NegocioException($"Não foi encontrado professor com RF {request.CodigoRF}");

                    var possuiAtribuicaoNaUE = obterAtribuicoesCJAtivas.Any(a => a.UeId == request.UeId);
                    if (possuiAtribuicaoNaUE)
                        return new ProfessorResumoDto { CodigoRF = request.CodigoRF, Nome = dadosUsuarioLogado.Nome, UsuarioId = dadosUsuarioLogado.Id };
                }

                throw new NegocioException($"Não foi encontrado professor com RF {request.CodigoRF}");
            }

            return null;
        }

        private async Task<bool> EhFuncionarioGestorEscolarDaUe(string codigoRfUsuarioLogado, ObterProfessorPorRFUeDreAnoLetivoQuery request)
        {
            if (request.CodigoRF.EhLoginCpf())
            {
                var funcionariosExternosDaUe = await mediator.Send(new ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery(request.UeId,
                    new List<int> { (int)FuncaoExterna.AD, (int)FuncaoExterna.Diretor, (int)FuncaoExterna.CP }, request.DreId));
                return funcionariosExternosDaUe.Any(f => f.FuncionarioCpf == codigoRfUsuarioLogado);
            }
            var funcionariosDaUe = await mediator.Send(new ObterFuncionariosCargosPorUeCargosQuery(request.UeId,
                                                            new List<int> { (int)Cargo.AD, (int)Cargo.Diretor, (int)Cargo.CP }, request.DreId));
            return funcionariosDaUe.Any(f => f.FuncionarioRF == codigoRfUsuarioLogado);
        }
                    

private static void ValidarParametroDreId(ObterProfessorPorRFUeDreAnoLetivoQuery request)
        {
            if (!request.BuscarPorTodasDre 
                && string.IsNullOrWhiteSpace(request.DreId))
                throw new NegocioException("É necessario informar o codigoRF Dre, UE e o ano letivo");
        }

        private static (string paramUeId, string paramDreId) ObterParametrosDreUe(ObterProfessorPorRFUeDreAnoLetivoQuery request)
        {
            string paramUeId = string.Empty, paramDreId = string.Empty;
            if (!request.BuscarPorTodasDre)
            {
                paramUeId = !string.IsNullOrWhiteSpace(request.UeId) ? $"ueId={request.UeId}&" : string.Empty;
                paramDreId = !string.IsNullOrWhiteSpace(request.DreId) ? $"dreId={request.DreId}&" : string.Empty;
            }
            return (paramUeId, paramDreId);
        }
    }
}
