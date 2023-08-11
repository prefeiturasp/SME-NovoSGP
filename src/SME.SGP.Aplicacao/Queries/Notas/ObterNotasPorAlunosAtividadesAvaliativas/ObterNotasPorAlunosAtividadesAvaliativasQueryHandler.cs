using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorAlunosAtividadesAvaliativasQueryHandler : IRequestHandler<ObterNotasPorAlunosAtividadesAvaliativasQuery, IEnumerable<NotaConceito>>
    {
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;
        public ObterNotasPorAlunosAtividadesAvaliativasQueryHandler(IRepositorioNotasConceitosConsulta repositorioNotasConceitos, IRepositorioCache repositorioCache, IMediator mediator)
        {
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<NotaConceito>> Handle(ObterNotasPorAlunosAtividadesAvaliativasQuery request, CancellationToken cancellationToken)
        {
            var atividadeAvaliativas = await ObterCacheAtividadeAvaliativa(request);


            var notaConceitoFiltrada = from aac in atividadeAvaliativas.ToList()
                                       join aai in request.AtividadesAvaliativasId.Distinct() on aac.AtividadeAvaliativaID equals aai
                                       select aac;

            return notaConceitoFiltrada;
        }

        private async Task<IEnumerable<NotaConceito>> ObterCacheAtividadeAvaliativa(ObterNotasPorAlunosAtividadesAvaliativasQuery request)
        {
            var nomeChave = string.Format(NomeChaveCache.ATIVIDADES_AVALIATIVAS_TURMA, request.CodigoTurma);
            var atividadesAvaliativasNoCache = await repositorioCache.ObterAsync(nomeChave);

            if (string.IsNullOrEmpty(atividadesAvaliativasNoCache))
            {
                return await mediator.Send(new CriarCacheDeAtividadeAvaliativaPorTurmaCommand(request.CodigoTurma));
            }

            return JsonConvert.DeserializeObject<IEnumerable<NotaConceito>>(atividadesAvaliativasNoCache);
        }
    }
}
