using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class ObterNotasConceitosFechamentoPorTurmaIdEBimestreQueryHandler : IRequestHandler<ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery, IEnumerable<NotaConceitoComponenteBimestreAlunoDto>>
{
    private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
    private readonly IRepositorioCache repositorioCache;

    public ObterNotasConceitosFechamentoPorTurmaIdEBimestreQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota, IRepositorioCache repositorioCache)
    {
        this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));        
        this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
    }

    public async Task<IEnumerable<NotaConceitoComponenteBimestreAlunoDto>> Handle(ObterNotasConceitosFechamentoPorTurmaIdEBimestreQuery request, CancellationToken cancellationToken)
    {
        var retorno = new List<NotaConceitoComponenteBimestreAlunoDto>();

        foreach (var turmaId in request.TurmasIds)
        {
            var notasConceitosFechamento = (await repositorioCache.ObterAsync(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_FECHAMENTO_TURMA_BIMESTRE, turmaId, request.Bimestre),
                async () => await repositorioConselhoClasseNota.ObterNotasConceitosFechamentoPorTurmaIdEBimestreAsync(turmaId, request.Bimestre))).ToList();
        
            if (notasConceitosFechamento.Any())
                retorno.AddRange(notasConceitosFechamento);
        }

        return await Task.FromResult(retorno);
    }
}