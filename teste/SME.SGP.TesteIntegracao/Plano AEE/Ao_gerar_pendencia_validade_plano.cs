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

namespace SME.SGP.TesteIntegracao.Plano_AEE
{
    public class Ao_gerar_pendencia_validade_plano : TesteBase
    {
        private readonly ItensBasicosBuilder _builder;

        public Ao_gerar_pendencia_validade_plano(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_gerar_pendencia_validade_para_responsavel()
        {
            await CriaBase();

            var useCase = ServiceProvider.GetService<IGerarPendenciaValidadePlanoAEEUseCase>();

            await useCase.Executar(new MensagemRabbit());

            var lista = ObterTodos<PendenciaUsuario>();

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
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new PlanoAEE()
            {
                Id = 1,
                AlunoCodigo = "7128291",
                ResponsavelId = 1,
                TurmaId = 1,
                AlunoNome = "ANA RITA",
                Questoes = new List<PlanoAEEQuestao>(),
                Situacao = SituacaoPlanoAEE.Validado,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new PlanoAEEVersao
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new Questionario
            {
                Nome = "Questionario",
                Tipo = TipoQuestionario.PlanoAEE,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new Questao
            {
                Nome = "Questao 1",
                Ordem = 1,
                QuestionarioId = 1,
                Tipo = TipoQuestao.PeriodoEscolar,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new PlanoAEEQuestao
            {
                PlanoAEEVersaoId = 1,
                QuestaoId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)

            });

            await InserirNaBase(new PlanoAEEResposta
            {
                PlanoAEEQuestaoId = 1,
                Texto = "1",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new Usuario
            {
                Id = 2,
                Login = "6118232",
                CodigoRf = "6118232",
                Nome = "MARLEI LUCIANE BERNUN",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new PendenciaPlanoAEE
            {
                PendenciaId = 1,
                PlanoAEEId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });

            await InserirNaBase(new PendenciaUsuario
            {
                PendenciaId = 1,
                UsuarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(2022, 06, 08)
            });
        }
    }
}
