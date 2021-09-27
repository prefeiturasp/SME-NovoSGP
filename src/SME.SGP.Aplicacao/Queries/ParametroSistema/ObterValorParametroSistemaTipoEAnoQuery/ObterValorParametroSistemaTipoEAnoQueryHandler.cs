using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterValorParametroSistemaTipoEAnoQueryHandler : IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IRepositorioCache repositorioCache;

        public ObterValorParametroSistemaTipoEAnoQueryHandler(IRepositorioParametrosSistema repositorioParametrosSistema, IRepositorioCache repositorioCache)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<string> Handle(ObterValorParametroSistemaTipoEAnoQuery request, CancellationToken cancellationToken)
        {
            var nomeCache = $"{request.Tipo}-{request.Ano}";
            var parametrosNoCache = repositorioCache.Obter($"parametros-sistema-{request.Ano}");

            IEnumerable<ParametrosSistema> parametrosDoSistema;

            if (string.IsNullOrEmpty(parametrosNoCache))
            {
                parametrosDoSistema = await repositorioParametrosSistema.ObterParametrosPorAnoAsync(request.Ano);
                await repositorioCache.SalvarAsync(nomeCache, parametrosDoSistema);
            }
            else parametrosDoSistema = JsonConvert.DeserializeObject<List<ParametrosSistema>>(parametrosNoCache);

            return parametrosDoSistema.FirstOrDefault(a => a.Tipo == request.Tipo).Valor;
        }
    }
}
