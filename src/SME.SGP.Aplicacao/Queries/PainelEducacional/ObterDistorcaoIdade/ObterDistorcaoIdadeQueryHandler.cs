using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDistorcaoIdade
{
    public class ObterDistorcaoIdadeQueryHandler : IRequestHandler<ObterDistorcaoIdadeQuery, IEnumerable<PainelEducacionalDistorcaoIdadeDto>>
    {
        private readonly IRepositorioDistorcaoIdade repositorio;

        public ObterDistorcaoIdadeQueryHandler(IRepositorioDistorcaoIdade repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalDistorcaoIdadeDto>> Handle(ObterDistorcaoIdadeQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterDistorcaoIdade(request.Filtro);
            return registros
                .GroupBy(r => r.Modalidade)
                .Select(g => new PainelEducacionalDistorcaoIdadeDto
                {
                    Modalidade = g.Key,
                    SerieAno = g
                        .GroupBy(x => x.Ano)
                        .Select(a => new SerieAnoDistorcaoIdadeDto
                        {
                            Ano = $"{a.Key}º",
                            QuantidadeAlunos = a.Sum(z => z.QuantidadeAlunos)
                        })
                        .OrderBy(x => x.Ano)
                        .ToList()
                })
                .ToList();
        }
    }
}
