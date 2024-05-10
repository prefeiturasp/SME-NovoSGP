using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorCodigoQueryHandler : IRequestHandler<ObterTurmaPorCodigoQuery, Turma>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioCache repositorioCache;

        public ObterTurmaPorCodigoQueryHandler(IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioCache repositorioCache) 
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        
        public async Task<Turma> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return request.UsarRepositorio 
                    ? await repositorioTurmaConsulta.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo)
                    : await repositorioCache.ObterAsync(ObterChave(request.TurmaCodigo), async () => await repositorioTurmaConsulta.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo));
        }
        
        private string ObterChave(string turmaCodigo)
        {
            return string.Format(NomeChaveCache.TURMA_CODIGO, turmaCodigo); 
        }
    }
}
