using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EhGestorDaEscolaQueryHandler : IRequestHandler<EhGestorDaEscolaQuery, bool>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public EhGestorDaEscolaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<bool> Handle(EhGestorDaEscolaQuery request, CancellationToken cancellationToken)
        {
            var cargo = ObterCargoPorPerfil(request.Perfil);
            var funcaoExterna = ObterFuncaoExternaPorPerfil(request.Perfil);

            using (var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO))
            {
                HttpResponseMessage resposta;
                if (request.UsuarioRf.EhLoginCpf())
                    resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ESCOLAS_FUNCIONARIOS_FUNCOES_EXTERNAS, request.UeCodigo, funcaoExterna));
                else
                    resposta = await httpClient.GetAsync($"/api/" + string.Format(ServicosEolConstants.URL_ESCOLAS_FUNCIONARIOS_CARGOS, request.UeCodigo, cargo));
                    
                if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
                {
                    var json = await resposta.Content.ReadAsStringAsync();
                    var funcionariosEOL = JsonConvert.DeserializeObject<IEnumerable<UsuarioEolRetornoDto>>(json);

                    return funcionariosEOL.Any(c => c.CodigoRf == request.UsuarioRf);
                }
            }

            return false;
        }

        private int ObterCargoPorPerfil(Guid perfil)
        {
            if (perfil == Perfis.PERFIL_CP)
                return (int)Cargo.CP;
            if (perfil == Perfis.PERFIL_AD)
                return (int)Cargo.AD;
            if (perfil == Perfis.PERFIL_DIRETOR)
                return (int)Cargo.Diretor;

            return 0;
        }

        private int ObterFuncaoExternaPorPerfil(Guid perfil)
        {
            if (perfil == Perfis.PERFIL_CP)
                return (int)FuncaoExterna.CP;
            if (perfil == Perfis.PERFIL_AD)
                return (int)FuncaoExterna.AD;
            if (perfil == Perfis.PERFIL_DIRETOR)
                return (int)FuncaoExterna.Diretor;

            return 0;
        }
    }
}
