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

public class ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQueryHandler : IRequestHandler<ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQuery, IEnumerable<NotaConceitoComponenteBimestreAlunoDto>>
{
    private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
    private readonly IRepositorioCache repositorioCache;

    public ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota, IRepositorioCache repositorioCache)
    {
        this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));        
        this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
    }

    public async Task<IEnumerable<NotaConceitoComponenteBimestreAlunoDto>> Handle(ObterNotasConceitosConselhoClassePorTurmaIdEBimestreQuery request, CancellationToken cancellationToken)
    {
        var retorno = new List<NotaConceitoComponenteBimestreAlunoDto>();

        foreach (var turmaId in request.TurmasIds)
        {
            var notasConceitosConselhoClasse = (await repositorioCache.ObterAsync($"NotaConceitoConselhoClasse-{turmaId}-{request.Bimestre}",
                async () => await repositorioConselhoClasseNota.ObterNotasConceitosConselhoClassePorTurmaIdEBimestreAsync(turmaId, request.Bimestre))).ToList();
        
            if (!notasConceitosConselhoClasse.Any())
                throw new NegocioException("Não foi possível recuperar a lista das notas/conceitos do conselho de classe da turma.");

            retorno.AddRange(notasConceitosConselhoClasse);            
        }

        return await Task.FromResult(retorno);
    }
}