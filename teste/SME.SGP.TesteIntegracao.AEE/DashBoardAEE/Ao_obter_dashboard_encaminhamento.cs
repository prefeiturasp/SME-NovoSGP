using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AEE.DashBoardAEE
{
    public class Ao_obter_dashboard_encaminhamento : TesteBaseComuns
    {
        public Ao_obter_dashboard_encaminhamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_obter_dashboard_encaminhamentos()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();

            await CriarTurma(Modalidade.Fundamental, ANO_8, false);

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                Situacao = SituacaoAEE.EncerradoAutomaticamente,
                AlunoNome = "Nome do aluno 1",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                Situacao = SituacaoAEE.AtribuicaoPAAI,
                AlunoNome = "Nome do aluno 2",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                Situacao = SituacaoAEE.Encaminhado,
                AlunoNome = "Nome do aluno 3",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_4,
                Situacao = SituacaoAEE.Analise,
                AlunoNome = "Nome do aluno 4",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.EncaminhamentoAEE()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_5,
                Situacao = SituacaoAEE.Finalizado,
                AlunoNome = "Nome do aluno 5",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterEncaminhamentoAEESituacoesUseCase>();
            var filtro = new FiltroDashboardAEEDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalEncaminhamentosAnalise.ShouldBe(3);
            retorno.QtdeEncaminhamentosSituacao.ShouldBe(4);
            retorno.SituacoesEncaminhamentoAEE.Count().ShouldBe(5);
        }
    }
}
