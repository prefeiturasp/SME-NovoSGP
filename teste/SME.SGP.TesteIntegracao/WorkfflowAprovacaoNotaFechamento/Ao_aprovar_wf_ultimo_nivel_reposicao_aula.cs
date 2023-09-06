using Elastic.Apm.Api;
using EmptyFiles;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Servicos;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.WorkfflowAprovacaoNotaFechamento
{
    public class Ao_aprovar_wf_ultimo_nivel_reposicao_aula : TesteBaseComuns
    {


        public Ao_aprovar_wf_ultimo_nivel_reposicao_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_aprovar_reposicao_aula_cj()
        {
            var comandosWorkflow = ServiceProvider.GetService<IRepositorioAulaConsulta>();

            await CriarNovaAulaReposicao(ObterPerfilCJ());

            await CriarAtribuicaoCJ(Modalidade.Medio, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            var aula = ObterAula(Convert.ToString(COMPONENTE_CURRICULAR_PORTUGUES_ID_138), DateTime.Now, RecorrenciaAula.AulaUnica);

            long workflowAprovacaoId =  aula != null && aula?.WorkflowAprovacaoId != null ? (long)aula.WorkflowAprovacaoId : 0;

            var aulaRetornoWorkflow = await comandosWorkflow.ObterPorWorkflowId(workflowAprovacaoId);

            aulaRetornoWorkflow.AulaCJ.ShouldBeEquivalentTo(true);
        }

        private async Task CriarNovaAulaReposicao(string perfil)
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(true, DATA_02_05, DATA_08_07, BIMESTRE_2, 1);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(Modalidade.Fundamental);
            await CriarParametrosSistema(DATA_02_05.Year);

            await InserirNaBase(new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = Convert.ToString(COMPONENTE_CURRICULAR_PORTUGUES_ID_138),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                Quantidade = 2,
                DataAula = DateTime.Now,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Reposicao,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                WorkflowAprovacaoId = 1,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = true
            });
        }
        private Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = 3,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                WorkflowAprovacaoId = 1,
                Excluido = false,
                Migrado = false,
                AulaCJ = true
            };
        }
    }
}
