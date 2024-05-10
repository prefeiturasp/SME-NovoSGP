using MediatR;
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

        public ObterNotasPorAlunosAtividadesAvaliativasQueryHandler(IRepositorioNotasConceitosConsulta repositorioNotasConceitos, IRepositorioCache repositorioCache)
        {
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<NotaConceito>> Handle(ObterNotasPorAlunosAtividadesAvaliativasQuery request, CancellationToken cancellationToken)
        {
            var atividadeAvaliativas = await ObterCacheAtividadeAvaliativa(request);


            var notaConceitoFiltrada = from aac in atividadeAvaliativas.ToList()
                                       join aai in request.AtividadesAvaliativasId.Distinct() on aac.AtividadeAvaliativaID equals aai
                                       select aac;

            return notaConceitoFiltrada;
        }

        private Task<IEnumerable<NotaConceito>> ObterCacheAtividadeAvaliativa(ObterNotasPorAlunosAtividadesAvaliativasQuery request)
        {
            var nomeChave = string.Format(NomeChaveCache.ATIVIDADES_AVALIATIVAS_TURMA, request.CodigoTurma);

            return repositorioCache.ObterAsync(nomeChave,
                     async () => await repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativasPorTurmaAsync(request.CodigoTurma),
                     "Obter atividade avaliativa");
        }
    }
}
