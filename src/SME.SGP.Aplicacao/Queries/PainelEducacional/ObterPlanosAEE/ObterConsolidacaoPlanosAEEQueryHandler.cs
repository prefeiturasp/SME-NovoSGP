using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterPlanoAEE
{
    public class ObterConsolidacaoPlanosAEEQueryHandler : IRequestHandler<ObterConsolidacaoPlanosAEEQuery, IEnumerable<PainelEducacionalPlanoAEEDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterConsolidacaoPlanosAEEQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalPlanoAEEDto>> Handle(ObterConsolidacaoPlanosAEEQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterConsolidacaoPlanosPainelEducacional(request.Filtro);

            var retorno = new PainelEducacionalPlanoAEEDto
            {
                QuantidadePlanos = registros.Sum(r => r.QuantidadeSituacaoPlano),
                Planos = registros
                    .GroupBy(g => g.SituacaoPlano)
                    .Select(p => new IndicadorPlanoAEEDto
                    {
                        SituacaoPlano = ((SituacaoPlanoAEE)p.Key).ObterDisplayName(),
                        QuantidadeAlunos = p.Sum(x => x.QuantidadeSituacaoPlano)
                    })
                    .OrderBy(x => x.SituacaoPlano)
                    .ToList()
            };

            return new List<PainelEducacionalPlanoAEEDto> { retorno };
        }
    }
}
