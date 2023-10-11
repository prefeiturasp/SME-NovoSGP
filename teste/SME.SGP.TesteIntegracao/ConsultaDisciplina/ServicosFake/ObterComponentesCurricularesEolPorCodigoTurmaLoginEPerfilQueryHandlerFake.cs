using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake
{
    public class ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFake : IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFake() { }
        public async Task<IEnumerable<ComponenteCurricularEol>> Handle(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery request, CancellationToken cancellationToken)
        {
            var listaComponenteCurricularEOL = new List<ComponenteCurricularEol>();

            var componentesCurriculares1 = new ComponenteCurricularEol
            {
                CodigoComponenteCurricularPai = 1,
                Codigo = 1,
                TurmaCodigo = "1"
            };
            var componentesCurriculares2 = new ComponenteCurricularEol
            {
                CodigoComponenteCurricularPai = 1,
                Codigo = 2,
                TurmaCodigo = "1"
            };

            listaComponenteCurricularEOL.Add(componentesCurriculares1);
            listaComponenteCurricularEOL.Add(componentesCurriculares2);

            return await Task.FromResult(listaComponenteCurricularEOL);
        }
    }
}
