using MediatR;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroEstudantesPap;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapQueryHandler : IRequestHandler<ObterIndicadoresPapQuery, IEnumerable<ConsolidacaoInformacoesPap>>
    {
        private readonly IMediator mediator;

        public ObterIndicadoresPapQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ConsolidacaoInformacoesPap>> Handle(ObterIndicadoresPapQuery request, CancellationToken cancellationToken)
        {
            var indicadoresCombinados = new List<ConsolidacaoInformacoesPap>();

            var numerosSgp = await mediator.Send(new ObterIndicadoresPapSgpQuery(), cancellationToken);
            var dificuldadesEol = await mediator.Send(new PainelEducacionalIndicadoresPapEolQuery(), cancellationToken);

            var dificuldadesPorTipo = numerosSgp != null
                ? numerosSgp.GroupBy(n => n.TipoPap)
                           .ToDictionary(g => g.Key, g => g.First())  
                : new Dictionary<TipoPap, ContagemDificuldadePorTipoDto>();

            var numerosPorTipo = dificuldadesEol != null
                ? dificuldadesEol.GroupBy(d => d.TipoPap)
                                 .ToDictionary(g => g.Key, g => g.First())  
                : new Dictionary<TipoPap, ContagemNumeroAlunosPapDto>();

            var todosTiposPap = Enum.GetValues<TipoPap>().AsEnumerable();

            foreach (var tipoPap in todosTiposPap)
            {
                var numeros = numerosPorTipo.ContainsKey(tipoPap) ? numerosPorTipo[tipoPap] : null;
                var dificuldades = dificuldadesPorTipo.ContainsKey(tipoPap) ? dificuldadesPorTipo[tipoPap] : null;

                indicadoresCombinados.Add(new ConsolidacaoInformacoesPap(
                    id: 0,
                    tipoPap: tipoPap,
                    quantidadeTurmas: numeros?.QuantidadeTurmas ?? 0,
                    quantidadeEstudantes: numeros?.QuantidadeEstudantes ?? 0,
                    quantidadeEstudantesComMenosDe75PorcentoFrequencia: numeros?.QuantidadeEstudantesComMenosDe75PorcentoFrequencia ?? 0,
                    dificuldadeAprendizagem1: dificuldades?.DificuldadeAprendizagem1 ?? 0,
                    dificuldadeAprendizagem2: dificuldades?.DificuldadeAprendizagem2 ?? 0,
                    outrasDificuldadesAprendizagem: dificuldades?.OutrasDificuldadesAprendizagem ?? 0
                ));
            }

            return indicadoresCombinados.OrderBy(i => i.TipoPap);
        }   
    }
}