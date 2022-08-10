using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterNotasFechamentosPorTurmasIdsQueryHandler : IRequestHandler<ObterNotasFechamentosPorTurmasIdsQuery, IEnumerable<NotaConceitoComponenteBimestreAlunoDto>>
{
    private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
    private readonly IRepositorioCache repositorioCache;

    public ObterNotasFechamentosPorTurmasIdsQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota, IRepositorioCache repositorioCache)
    {
        this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
    }

    public async Task<IEnumerable<NotaConceitoComponenteBimestreAlunoDto>> Handle(ObterNotasFechamentosPorTurmasIdsQuery request, CancellationToken cancellationToken)
    {
        var retorno = new List<NotaConceitoComponenteBimestreAlunoDto>();
        
        foreach (var turmaId in request.TurmasIds)
        {
            var notasConceitos = (await repositorioCache.ObterAsync($"NotaFechamento-{turmaId}",
                async () => await repositorioFechamentoNota.ObterNotasPorTurmaIdAsync(turmaId))).ToList();
            
            if (notasConceitos.Any())
                retorno.AddRange(notasConceitos);
        }
        
        return await Task.FromResult(retorno);        
    }
}