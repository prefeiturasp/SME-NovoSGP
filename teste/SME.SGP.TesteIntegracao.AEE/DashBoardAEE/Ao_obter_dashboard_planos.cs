using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Infra.Dtos;
using Shouldly;
using System.Linq;

namespace SME.SGP.TesteIntegracao.AEE.DashBoardAEE
{
    public class Ao_obter_dashboard_planos : TesteBaseComuns
    {
        public Ao_obter_dashboard_planos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Dashboard AEE - Planos situações")]
        public async Task Ao_obter_dashboard_planos_situacoes()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();

            await CriarTurma(Modalidade.Fundamental, ANO_8, false);

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.AtribuicaoPAAI,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNumero = 2,
                AlunoNome = "Nome do aluno 2",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Encerrado,
                AlunoCodigo = CODIGO_ALUNO_3,
                AlunoNumero = 3,
                AlunoNome = "Nome do aluno 3",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente,
                AlunoCodigo = CODIGO_ALUNO_4,
                AlunoNumero = 4,
                AlunoNome = "Nome do aluno 4",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var useCase = ServiceProvider.GetService<IObterPlanoAEESituacoesUseCase>();
            var filtro = new FiltroDashboardAEEDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalPlanosVigentes.ShouldBe(2);
            retorno.SituacoesPlanos.Count().ShouldBe(4);
        }

        [Fact(DisplayName = "Dashboard AEE - Planos vigentes")]
        public async Task Ao_obter_dashboard_planos_vigentes()
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(ObterPerfilCP());

            await CriarUsuarios();

            await CriarTurma(Modalidade.Fundamental, ANO_8, false);

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Expirado,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Validado,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNumero = 2,
                AlunoNome = "Nome do aluno 2",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Encerrado,
                AlunoCodigo = CODIGO_ALUNO_3,
                AlunoNumero = 3,
                AlunoNome = "Nome do aluno 3",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente,
                AlunoCodigo = CODIGO_ALUNO_4,
                AlunoNumero = 4,
                AlunoNome = "Nome do aluno 4",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = ALUNO_CODIGO_5,
                AlunoNumero = 5,
                AlunoNome = "Nome do aluno 5",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var useCase = ServiceProvider.GetService<IObterPlanosAEEVigentesUseCase>();
            var filtro = new FiltroDashboardAEEDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1
            };

            var retorno = await useCase.Executar(filtro);
            retorno.ShouldNotBeNull();
            retorno.TotalPlanosVigentes.ShouldBe(3);
            retorno.PlanosVigentes.Count().ShouldBe(1);
            retorno.PlanosVigentes.First().Quantidade.ShouldBe(2);
        }
    }
}
