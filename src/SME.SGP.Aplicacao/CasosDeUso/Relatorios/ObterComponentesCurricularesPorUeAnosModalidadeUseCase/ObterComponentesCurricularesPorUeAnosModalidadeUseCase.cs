using MediatR;
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
            var turmaCodigos = await  mediator.Send(new ObterTurmasPorUeAnosModalidadeQuery(ueCodigo, anoLetivo, anos, (int)modalidade));

            if (turmaCodigos == null || !turmaCodigos.Any())
                throw new NegocioException("Não foi possível obter turmas para obter os componentes curriculares.");

            var listaComponentes = (await mediator.Send(new ObterComponentesCurricularesPorUeAnosModalidadeQuery(turmaCodigos.ToArray(), anoLetivo, modalidade, anos))).ToList();

            listaComponentes.Insert(0, new ComponenteCurricularEol() { Codigo = -99, Descricao = "Todos" });

            return listaComponentes;
        }
    }
}
