using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterAlunosAtivosTurmaProgramaPapEolQueryHandlerFake: IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery,IEnumerable<AlunosTurmaProgramaPapDto>>
    {
        private const int ALUNO_CODIGO_1 = 1;
        private const int ALUNO_CODIGO_2 = 2;
        private const int ALUNO_CODIGO_3 = 3;
        private const int ALUNO_CODIGO_4 = 4;
        
        private const int TURMA_CODIGO_1 = 1;
        private const int COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        private const string COMPONENTE_CURRICULAR_PORTUGUES_NOME_138 = "Língua Portuguesa";
        
        public ObterAlunosAtivosTurmaProgramaPapEolQueryHandlerFake()
        {}

        public async Task<IEnumerable<AlunosTurmaProgramaPapDto>> Handle(ObterAlunosAtivosTurmaProgramaPapEolQuery request, CancellationToken cancellationToken)
        {
            return new List<AlunosTurmaProgramaPapDto> {
                new AlunosTurmaProgramaPapDto
                {
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoTurma  = TURMA_CODIGO_1,
                      CodigoComponente = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                      Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME_138
                },
                new AlunosTurmaProgramaPapDto
                {
                    CodigoAluno = ALUNO_CODIGO_2,
                    CodigoTurma  = TURMA_CODIGO_1,
                    CodigoComponente = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME_138
                },
                new AlunosTurmaProgramaPapDto
                {
                    CodigoAluno = ALUNO_CODIGO_3,
                    CodigoTurma  = TURMA_CODIGO_1,
                    CodigoComponente = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME_138
                },
                new AlunosTurmaProgramaPapDto
                {
                    CodigoAluno = ALUNO_CODIGO_4,
                    CodigoTurma  = TURMA_CODIGO_1,
                    CodigoComponente = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME_138
                }
            };
        }
    }
}