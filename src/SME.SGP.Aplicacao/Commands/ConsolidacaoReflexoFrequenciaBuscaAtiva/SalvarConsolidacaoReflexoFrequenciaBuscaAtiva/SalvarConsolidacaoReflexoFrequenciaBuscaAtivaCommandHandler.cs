using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommandHandler : IRequestHandler<SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand, long>
    {
        private readonly IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva repositorioConsolidacaoReflexoFrequencia;

        public SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommandHandler(IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva repositorioConsolidacaoReflexoFrequencia)
        {
            this.repositorioConsolidacaoReflexoFrequencia = repositorioConsolidacaoReflexoFrequencia ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoReflexoFrequencia));
        }

        public async Task<long> Handle(SalvarConsolidacaoReflexoFrequenciaBuscaAtivaCommand request, CancellationToken cancellationToken)
        {
            var consolidacaoExistente = await repositorioConsolidacaoReflexoFrequencia.ObterIdConsolidacao(request.ConsolidacaoReflexoFrequencia.TurmaCodigo,
                                                                                                 request.ConsolidacaoReflexoFrequencia.AlunoCodigo,
                                                                                                 request.ConsolidacaoReflexoFrequencia.Mes,
                                                                                                 request.ConsolidacaoReflexoFrequencia.AnoLetivo);
            if (consolidacaoExistente.NaoEhNulo())
            {
                request.ConsolidacaoReflexoFrequencia.Id = consolidacaoExistente.Id;
                request.ConsolidacaoReflexoFrequencia.CriadoEm = consolidacaoExistente.CriadoEm;
                request.ConsolidacaoReflexoFrequencia.CriadoPor = consolidacaoExistente.CriadoPor;
                request.ConsolidacaoReflexoFrequencia.CriadoRF = consolidacaoExistente.CriadoRF;
            }
                
            return await repositorioConsolidacaoReflexoFrequencia.SalvarAsync(request.ConsolidacaoReflexoFrequencia);           
        }
    }
}
