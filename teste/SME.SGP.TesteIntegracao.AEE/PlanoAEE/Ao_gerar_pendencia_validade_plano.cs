using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_gerar_pendencia_validade_plano : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_gerar_pendencia_validade_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact(DisplayName = "Plano AEE - Deve gerar uma pendencia com validade para o responsavel")]
        public async Task Deve_gerar_pendencia_validade_para_responsavel()
        {
            await CriaBase();

            var useCase = ServiceProvider.GetService<IGerarPendenciaValidadePlanoAEEUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var lista = ObterTodos<Dominio.PendenciaUsuario>();

            lista.ShouldNotBeEmpty();
            lista.FirstOrDefault().UsuarioId.ShouldBe(1);
        }

        private async Task CriaBase()
        {
            await _builder.CriaItensComunsEja();

            await InserirNaBase(new ParametrosSistema
            {
                Nome = "GerarPendencia",
                Tipo = TipoParametroSistema.GerarPendenciasPlanoAEE,
                Descricao = "Gerar Pendência planoAEE",
                Valor = "",
                Ano = DateTime.Today.Year,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Ativo = true
            });

            await InserirNaBase(new Pendencia()
            {
                Id = 1,
                Tipo = TipoPendencia.AEE,
                Descricao = "Pendência AEE",
                Titulo = "Pendência plano AEE",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                Id = 1,
                AlunoCodigo = "11223344",
                ResponsavelId = 1,
                TurmaId = 1,
                AlunoNome = "Maria Aluno teste",
                Questoes = new List<PlanoAEEQuestao>(),
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new PlanoAEEVersao
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Questionario
            {
                Nome = "Questionario",
                Tipo = TipoQuestionario.PlanoAEE,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Questao
            {
                Nome = "Questao 1",
                Ordem = 1,
                QuestionarioId = 1,
                Tipo = TipoQuestao.PeriodoEscolar,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new PlanoAEEQuestao
            {
                PlanoAEEVersaoId = 1,
                QuestaoId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)

            });

            await InserirNaBase(new PlanoAEEResposta
            {
                PlanoAEEQuestaoId = 1,
                Texto = "1",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Usuario
            {
                Id = 2,
                Login = "1234567",
                CodigoRf = "1234567",
                Nome = "Maria dos testes",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new PendenciaPlanoAEE
            {
                PendenciaId = 1,
                PlanoAEEId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new Dominio.PendenciaUsuario
            {
                PendenciaId = 1,
                UsuarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });
        }
    }
}
