using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheDeAvaliacaoAvaliativaPorTurmaCommandHandler : IRequestHandler<CriarCacheDeAvaliativaAvaliativaPorTurmaCommand, IEnumerable<NotaConceito>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public CriarCacheDeAvaliacaoAvaliativaPorTurmaCommandHandler(IRepositorioCache repositorioCache, IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new System.ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<IEnumerable<NotaConceito>> Handle(CriarCacheDeAvaliativaAvaliativaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var atividadeAvaliativas = await repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativasPorTurmaAsync(request.CodigoTurma);
            await repositorioCache.SalvarAsync(request.NomeChave, atividadeAvaliativas);

            return atividadeAvaliativas;
        }
    }
}
