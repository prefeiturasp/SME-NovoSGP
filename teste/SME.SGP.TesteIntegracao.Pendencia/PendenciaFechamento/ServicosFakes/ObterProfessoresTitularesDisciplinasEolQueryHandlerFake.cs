using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.PendenciaFechamento.ServicosFakes
{
    public class ObterProfessoresTitularesDisciplinasEolQueryHandlerFake: IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        private const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        private const string DISCIPLINA_PORTUGUES_138 = "138";

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> Handle(ObterProfessoresTitularesDisciplinasEolQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorTitularDisciplinaEol>()
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                    ProfessorNome ="PROFESSOR DE PORTUGUES",
                    DisciplinaNome = "LÍNGUA PORTUGUESA",
                    CodigosDisciplinas = DISCIPLINA_PORTUGUES_138 
                },
            };
        }
    }
}
