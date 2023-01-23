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
    public class ObterTurmasPorCodigoQueryHandler : IRequestHandler<ObterTurmasPorCodigoQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioCache repositorioCache;

        public ObterTurmasPorCodigoQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }       

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterAsync(ObterChave(request.TurmaCodigo), async () => await repositorioTurmaConsulta.ObterTurmasComUeEDrePorCodigo(request.TurmaCodigo));
        }

        private string ObterChave(string turmaCodigo)
        {
            return string.Format(NomeChaveCache.CHAVE_TURMA_ID, turmaCodigo);
        }
    }
}
