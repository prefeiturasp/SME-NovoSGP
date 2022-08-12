using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes
{
    public class ProfessoresTurmaDisciplinaQueryHandlerFakeProfessorPortugues : IRequestHandler<ProfessoresTurmaDisciplinaQuery, IEnumerable<ProfessorAtribuidoTurmaDisciplinaDTO>>
    {
        private const string TURMA_CODIGO_1 = "1";
        private const string TURMA_NOME_1 = "TURMA 1";
        private const string CODIGO_RF_11 = "11";
        private const string DISCIPLINA_PORTUGUES_139 = "139";
        private const string NOME_PROFESSOR_1 = "NOME_PROFESSOR_1";
        public async Task<IEnumerable<ProfessorAtribuidoTurmaDisciplinaDTO>> Handle(ProfessoresTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            return new List<ProfessorAtribuidoTurmaDisciplinaDTO>()
            {
                new ()
                {
                    CodigoTurma = TURMA_CODIGO_1,
                    NomeTurma = TURMA_NOME_1,
                    DataInicioAtribuicao = DateTimeExtension.HorarioBrasilia().AddYears(-1),
                    DataFimAtribuicao = DateTimeExtension.HorarioBrasilia().AddYears(1),
                    DataFimTurma = DateTimeExtension.HorarioBrasilia().AddYears(1),
                    AnoAtribuicao =  DateTimeExtension.HorarioBrasilia().Year,
                    CodigoRf = CODIGO_RF_11,
                    DisciplinaId = DISCIPLINA_PORTUGUES_139,
                    NomeProfessor = NOME_PROFESSOR_1
                }
            };
        }
    }

}

