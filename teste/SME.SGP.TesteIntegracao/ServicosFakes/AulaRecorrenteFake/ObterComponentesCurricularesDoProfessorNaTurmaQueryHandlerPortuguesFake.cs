using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
	public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerPortuguesFake : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
	{
		public ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerPortuguesFake()
		{}

		public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
		{
			return new List<ComponenteCurricularEol>() 
			{ 
				new ComponenteCurricularEol(){ Codigo = 138,TerritorioSaber = false}
			};
		}
	}
}
