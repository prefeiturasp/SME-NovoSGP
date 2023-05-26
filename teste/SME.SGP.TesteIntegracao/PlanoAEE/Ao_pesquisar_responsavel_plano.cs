using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_pesquisar_responsavel_plano : PlanoAEETesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_pesquisar_responsavel_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve retornar o responsável pelo plano aee por ue")]
        public async Task Deve_retornar_responsavel_pelo_planoaee_por_ue()
        {
            await _builder.CriaItensComunsEja(); 

            var useCase = ServiceProvider.GetService<IPesquisaResponsavelPlanoPorDreUEUseCase>();
            var filtro = new FiltroPesquisaFuncionarioDto()
            {
                CodigoTurma = "1"
            };

            var pagina = await useCase.Executar(filtro);

            pagina.ShouldNotBeNull();
            pagina.Items.ShouldNotBeNull();
            pagina.Items.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact(DisplayName = "Plano AEE - Deve retornar todos responsáveis dos planos aee por dre")]
        public async Task Deve_retornar_todos_responsaveis_planoaee_por_dre()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Validado,
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
                AlunoCodigo = CODIGO_ALUNO_2,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 1
            };
            var useCase = ObterServicoObterResponsaveisPlanosAEEUseCase();
            var retorno = await useCase.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            retorno.ToList().Exists(plano => plano.CodigoRf == USUARIO_PROFESSOR_CODIGO_RF_2222222).ShouldBeTrue();
            retorno.ToList().Exists(plano => plano.CodigoRf == USUARIO_PROFESSOR_CODIGO_RF_1111111).ShouldBeTrue();
        }

        [Fact(DisplayName = "Plano AEE - Deve retornar todos responsáveis dos planos aee por todos filtros")]
        public async Task Deve_retornar_todos_responsaveis_planoaee_por_todos_filtros()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Validado,
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
                Situacao = SituacaoPlanoAEE.Encerrado,
                AlunoCodigo = CODIGO_ALUNO_2,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 1,
                UeId = 1,
                AlunoCodigo= CODIGO_ALUNO_2,
                ExibirEncerrados = false,
                TurmaId= TURMA_ID_1
            };
            var useCase = ObterServicoObterResponsaveisPlanosAEEUseCase();
            var retorno = await useCase.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            retorno.ToList().Exists(plano => plano.CodigoRf == USUARIO_PROFESSOR_CODIGO_RF_1111111).ShouldBeTrue();
        }
    }
}
