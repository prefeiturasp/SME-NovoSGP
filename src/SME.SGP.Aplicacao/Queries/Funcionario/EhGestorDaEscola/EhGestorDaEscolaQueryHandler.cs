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

            using (var httpClient = httpClientFactory.CreateClient("servicoEOL"))
            {
                var resposta = await httpClient.GetAsync($"/api/escolas/{request.UeCodigo}/funcionarios/cargos/{cargo}");

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
    }
}
