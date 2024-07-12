using MediatR;
using Minio.DataModel;
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

            if (request.AtividadesAvaliativasId.PossuiRegistros())
                atividadeAvaliativas = atividadeAvaliativas.Where(nc => request.AtividadesAvaliativasId.Contains(nc.AtividadeAvaliativaID));

            if (!string.IsNullOrEmpty(request.ComponenteCurricularId))
                atividadeAvaliativas = atividadeAvaliativas.Where(nc => nc.DisciplinaId.Equals(request.ComponenteCurricularId) || string.IsNullOrEmpty(nc.DisciplinaId));

            if (request.AlunosId.PossuiRegistros())
                atividadeAvaliativas = atividadeAvaliativas.Where(nc => request.AlunosId.Contains(nc.AlunoId));

            return atividadeAvaliativas;
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
