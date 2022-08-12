using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class ProfessoresTurmaDisciplinaQueryHandlerFakeProfessorLeituraOSL: IRequestHandler<ProfessoresTurmaDisciplinaQuery, IEnumerable<ProfessorAtribuidoTurmaDisciplinaDTO>>
    {
        private const string TURMA_CODIGO_1 = "1";
        private const string TURMA_NOME_1 = "TURMA 1";
        private const string CODIGO_RF_11 = "11";
        private const string DISCIPLINA_LEITURA_OSL_1061 = "1061";
        private const string NOME_PROFESSOR_1 = "NOME_PROFESSOR_1";
        
        public ProfessoresTurmaDisciplinaQueryHandlerFakeProfessorLeituraOSL()
        {}
        
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
                    DisciplinaId = DISCIPLINA_LEITURA_OSL_1061,
                    NomeProfessor = NOME_PROFESSOR_1
                }
            };
        }
    }
}