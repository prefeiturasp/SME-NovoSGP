using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheDeAtividadeAvaliativaPorTurmaCommandHandler : IRequestHandler<CriarCacheDeAtividadeAvaliativaPorTurmaCommand, IEnumerable<NotaConceito>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public CriarCacheDeAtividadeAvaliativaPorTurmaCommandHandler(IRepositorioCache repositorioCache, IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<IEnumerable<NotaConceito>> Handle(CriarCacheDeAtividadeAvaliativaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var nomeChave = string.Format(NomeChaveCache.ATIVIDADES_AVALIATIVAS_TURMA, request.CodigoTurma);
            
            var atividadeAvaliativas = await repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativasPorTurmaAsync(request.CodigoTurma);
            await repositorioCache.SalvarAsync(nomeChave, atividadeAvaliativas);

            return atividadeAvaliativas;
        }
    }
}
