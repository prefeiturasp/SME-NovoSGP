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

            if (!request.BuscarPorTodasDre && string.IsNullOrWhiteSpace(request.DreId))
                throw new NegocioException("É necessario informar o codigoRF Dre, UE e o ano letivo");

            string paramUeId = string.Empty, paramDreId = string.Empty;

            if (!request.BuscarPorTodasDre)
            {
                paramUeId = !string.IsNullOrWhiteSpace(request.UeId) ? $"ueId={request.UeId}&" : string.Empty;
                paramDreId = !string.IsNullOrWhiteSpace(request.DreId) ? $"dreId={request.DreId}&" : string.Empty;
            }

            var url = string.Format(ServicosEolConstants.URL_PROFESSORES_BUSCAR_POR_RF_DRE_UE, request.CodigoRF, request.AnoLetivo);
            
            var resposta = await httpClient.GetAsync($"{url}?{string.Concat(paramUeId, paramDreId)}buscarOutrosCargos={request.BuscarOutrosCargos}");

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
                    bool ehFuncionarioGestorEscolarDaUe = false;
                    if (request.CodigoRF.EhLoginCpf())
                    {
                        var funcionariosExternosDaUe = await mediator.Send(new ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery(request.UeId,
                            new List<int> { (int)FuncaoExterna.AD, (int)FuncaoExterna.Diretor, (int)FuncaoExterna.CP }, request.DreId));
                        ehFuncionarioGestorEscolarDaUe = funcionariosExternosDaUe.Any(f => f.FuncionarioCpf == dadosUsuarioLogado.CodigoRf);
                    }
                    else
                    {
                        var funcionariosDaUe = await mediator.Send(new ObterFuncionariosCargosPorUeCargosQuery(request.UeId,
                            new List<int> { (int)Cargo.AD, (int)Cargo.Diretor, (int)Cargo.CP }, request.DreId));
                        ehFuncionarioGestorEscolarDaUe = funcionariosDaUe.Any(f => f.FuncionarioRF == dadosUsuarioLogado.CodigoRf);
                    }
                    

                    if (ehFuncionarioGestorEscolarDaUe)
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

            var json = await resposta.Content.ReadAsStringAsync();
            var retorno = JsonConvert.DeserializeObject<ProfessorResumoDto>(json);

            var usuario = await mediator.Send(new ObterUsuarioPorRfQuery(retorno.CodigoRF));

            if (usuario.EhNulo())
                throw new NegocioException("Usuário não localizado.");

            retorno.UsuarioId = usuario.Id;

            return retorno;
        }
    }
}
