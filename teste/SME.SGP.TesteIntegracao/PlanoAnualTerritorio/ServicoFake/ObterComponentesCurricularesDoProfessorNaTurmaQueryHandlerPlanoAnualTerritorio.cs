using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PlanoAnualTerritorio.ServicoFake
{

    public class ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerPlanoAnualTerritorio : IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesDoProfessorNaTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol() { Codigo = 1111, Regencia = true, Professor = "7111111", CodigoComponenteTerritorioSaber = 1, TerritorioSaber = true}
            });
        }
    }
}
