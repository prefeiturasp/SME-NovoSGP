using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesPorTurmasCodigoLPQueryFake : IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>
    {
        private const string TURMA_CODIGO_1 = "1";
        private const int CODIGO_LINGUA_PORTUGUESA = 138;
        private const string LINGUA_PORTUGUESA = "LINGUA PORTUGUESA";
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaDto>()
            {
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = CODIGO_LINGUA_PORTUGUESA,
                    CdComponenteCurricularPai = 0,
                    Nome = LINGUA_PORTUGUESA,
                    RegistraFrequencia = true,
                    LancaNota = true,
                    TurmaCodigo = TURMA_CODIGO_1
                }
            };
        }
    }
}
