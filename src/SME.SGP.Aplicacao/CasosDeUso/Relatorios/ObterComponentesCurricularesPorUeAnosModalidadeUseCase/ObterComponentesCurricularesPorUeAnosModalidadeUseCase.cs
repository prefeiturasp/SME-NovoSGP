using MediatR;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUeAnosModalidadeUseCase : IObterComponentesCurricularesPorUeAnosModalidadeUseCase
    {
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorUeAnosModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<ComponenteCurricularEol>> Executar(string[] anos, int anoLetivo, string ueCodigo, Modalidade modalidade)
        {
            var listaComponentes = new List<ComponenteCurricularEol>();
            if (anos.Contains("-99"))
                anos = new string[0];

            if (ueCodigo == "-99")
                ueCodigo = "";

            var turmaCodigos = await mediator.Send(new ObterTurmasPorUeAnosModalidadeQuery(ueCodigo, anoLetivo, anos, (int)modalidade));

            if (turmaCodigos == null || !turmaCodigos.Any())
                throw new NegocioException("Não foi possível obter turmas para obter os componentes curriculares.");

            if (!string.IsNullOrEmpty(ueCodigo))
                listaComponentes = (await mediator.Send(new ObterComponentesCurricularesPorUeAnosModalidadeQuery(turmaCodigos.ToArray(), anoLetivo, modalidade, anos))).ToList();
            else
            {
                ueCodigo = string.IsNullOrEmpty(ueCodigo) ? "-99" : ueCodigo;
                anos = !anos.Any() ? new string[] { "-99" } : anos; 
                listaComponentes = (await mediator.Send(new ObterComponentesCurricularesPorAnosEModalidadeQuery(ueCodigo, modalidade, anos, anoLetivo))).ToList();
            }
                
            if (listaComponentes != null && listaComponentes.Any())
                await TratarNomeComponentes(listaComponentes);

            if(listaComponentes?.Count > 1)
                listaComponentes.Insert(0, new ComponenteCurricularEol() { Codigo = -99, Descricao = "Todos" });

            return listaComponentes.GroupBy(x => x.Codigo).SelectMany(y => y.OrderBy(a => a.Descricao).Take(1));            
        }

        private async Task TratarNomeComponentes(List<ComponenteCurricularEol> listaComponentes)
        {
            var componenteIds = listaComponentes.Select(c => c.Codigo).ToArray();

            var componentesSgp = await mediator.Send(new ObterComponentesCurricularesSimplesPorIdsQuery(componenteIds));

            foreach (var componente in listaComponentes)
            {
                var componenteSgp = componentesSgp.FirstOrDefault(c => c.Id == componente.Codigo);

                if (componenteSgp != null)
                    componente.Descricao = componenteSgp.Descricao;
            }
        }
    }
}
