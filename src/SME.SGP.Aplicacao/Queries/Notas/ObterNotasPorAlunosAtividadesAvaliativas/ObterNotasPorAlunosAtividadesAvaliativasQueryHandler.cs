using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
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
            var nomeChave = $"Atividade-Avaliativa-{request.CodigoTurma}";
            var atividadesAvaliativasNoCache = await repositorioCache.ObterAsync(nomeChave);

            IEnumerable<NotaConceito> atividadeAvaliativas;

            if (string.IsNullOrEmpty(atividadesAvaliativasNoCache))
            {
                atividadeAvaliativas = await repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativasPorTurmaAsync(request.CodigoTurma);
                await repositorioCache.SalvarAsync(nomeChave, atividadeAvaliativas);
            }
            else
                atividadeAvaliativas = JsonConvert.DeserializeObject<IEnumerable<NotaConceito>>(atividadesAvaliativasNoCache);

            return atividadeAvaliativas;
        }
    }
}
