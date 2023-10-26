using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes.Respostas;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterTurmasDoProfessorQueryHandlerFake : IRequestHandler<ObterTurmasDoProfessorQuery, IEnumerable<ProfessorTurmaReposta>>
    {
        private const string ESCOLA_CODIGO_1 = "1";
        private const string ANO_7 = "7";
        private const int CODIGO_TURMA_1 = 1;
        private const int CODIGO_TURMA_2 = 2;
        private const string NOME_TURMA_1 = "1A";
        private const string NOME_TURMA_2 = "2A";
        private const int CODIGO_MODALIDADE = 5;
        private const string DRE_CODIGO_1 = "1";
        private const string DRE_NOME_1 = "NOME DRE 1";
        private const string DRE_ABREVIACAO_1 = "DRE-1";
        private const string MODALIDADE_FUNDAMENTAL = "FUNDAMENTAL";
        private const int SEMESTRE_0 = 0;
        private const string UNIDADE_ADMINISTRATIVA = "UNIDADE ADMINISTRATIVA";
        private const int UE_CODIGO_TIPO_3 = 3;
        private const string UE_NOME_1 = "NOME UE 1";
        private const string UE_ABREVIACAO_1 = "UE-1";
        private const string TIPO_ESCOLA_CEU_EMEF = "CEU EMEF";
        private const string TIPO_ESCOLA_CODIGO_16 = "16";
        
        public async Task<IEnumerable<ProfessorTurmaReposta>> Handle(ObterTurmasDoProfessorQuery request, CancellationToken cancellationToken)
        {
            return  new List<ProfessorTurmaReposta>()
            {
                new ProfessorTurmaReposta(){
                    CodEscola = ESCOLA_CODIGO_1,
                    Ano = ANO_7,
                    CodTurma = CODIGO_TURMA_1,
                    NomeTurma = NOME_TURMA_1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodModalidade = CODIGO_MODALIDADE,
                    CodDre = DRE_CODIGO_1,
                    Dre = DRE_NOME_1,
                    DreAbrev = DRE_ABREVIACAO_1,
                    Modalidade = MODALIDADE_FUNDAMENTAL,
                    Semestre = SEMESTRE_0,
                    TipoUE = UNIDADE_ADMINISTRATIVA,
                    CodTipoUE = UE_CODIGO_TIPO_3,
                    Ue = UE_NOME_1,
                    UeAbrev = UE_ABREVIACAO_1,
                    TipoEscola = TIPO_ESCOLA_CEU_EMEF,
                    CodTipoEscola = TIPO_ESCOLA_CODIGO_16,
                },
                new ProfessorTurmaReposta(){
                    CodEscola = ESCOLA_CODIGO_1,
                    Ano = ANO_7,
                    CodTurma = CODIGO_TURMA_2,
                    NomeTurma = NOME_TURMA_2,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodModalidade = CODIGO_MODALIDADE,
                    CodDre = DRE_CODIGO_1,
                    Dre = DRE_NOME_1,
                    DreAbrev = DRE_ABREVIACAO_1,
                    Modalidade = MODALIDADE_FUNDAMENTAL,
                    Semestre = SEMESTRE_0,
                    TipoUE = UNIDADE_ADMINISTRATIVA,
                    CodTipoUE = UE_CODIGO_TIPO_3,
                    Ue = UE_NOME_1,
                    UeAbrev = UE_ABREVIACAO_1,
                    TipoEscola = TIPO_ESCOLA_CEU_EMEF,
                    CodTipoEscola = TIPO_ESCOLA_CODIGO_16,
                }
            };
        }
    }
}
