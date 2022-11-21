using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake;
using SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_editar_registrar_percurso_observacoes : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_editar_registrar_percurso_observacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorCodigoEolQueryHandlerAlunoAtivoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Deve registrar observações adicionais")]
        public async Task Deve_registrar_observacoes_adicionais()
        {
            await CriarDadosBasicos(false);
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
            var observacao = "Observações adicionais";
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto
            {
                TurmaId = TURMA_ID_1,
                Semestre = 1,
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                Observacoes = observacao,
                AlunoCodigo = ALUNO_CODIGO_1
            };

            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);

            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            var acompanhamentoAlunoSemestre = acompanhamentoAlunoSemestres.FirstOrDefault();
            acompanhamentoAlunoSemestre.ShouldNotBeNull();
            acompanhamentoAlunoSemestre.Observacoes.ShouldBe(observacao);
        }

        [Fact(DisplayName = "Deve editar a observações adicionais")]
        public async Task Deve_editar_a_observacoes_adicionais()
        {
            await CriarDadosBasicos(false);
            await CriaAcomanhamentoIndividual();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
            var observacao = "Observações adicionais editada";
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto
            {
                AcompanhamentoAlunoId = 1,
                AcompanhamentoAlunoSemestreId = 1,
                TurmaId = TURMA_ID_1,
                Semestre = 1,
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                Observacoes = observacao,
                AlunoCodigo = ALUNO_CODIGO_1
            };

            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);

            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            var acompanhamentoAlunoSemestre = acompanhamentoAlunoSemestres.FirstOrDefault();
            acompanhamentoAlunoSemestre.ShouldNotBeNull();
            acompanhamentoAlunoSemestre.Observacoes.ShouldBe(observacao);
        }

        [Fact(DisplayName = "Deve limpar a observações adicionais")]
        public async Task Deve_limpar_a_observacoes_adicionais()
        {
            await CriarDadosBasicos(false);
            await CriaAcomanhamentoIndividual();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto
            {
                AcompanhamentoAlunoId = 1,
                AcompanhamentoAlunoSemestreId = 1,
                TurmaId = TURMA_ID_1,
                Semestre = 1,
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                Observacoes = String.Empty,
                AlunoCodigo = ALUNO_CODIGO_1
            };

            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);

            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            var acompanhamentoAlunoSemestre = acompanhamentoAlunoSemestres.FirstOrDefault();
            acompanhamentoAlunoSemestre.ShouldNotBeNull();
            acompanhamentoAlunoSemestre.Observacoes.ShouldBe(String.Empty);
        }


        [Fact(DisplayName = "Deve editar o percurso individual")]
        public async Task Deve_editar_o_percuso_individual()
        {
            await CriarDadosBasicos(false);
            await CriaAcomanhamentoIndividual();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var salvarAcompanhamentoAlunoUseCase = ObterServicoSalvarAcompanhamentoAlunoUseCase();
            var acompanhamentoAlunoDto = new AcompanhamentoAlunoDto
            {
                AcompanhamentoAlunoId = 1,
                AcompanhamentoAlunoSemestreId = 1,
                TurmaId = TURMA_ID_1,
                Semestre = 1,
                TextoSugerido = true,
                PercursoIndividual = TEXTO_PADRAO_PERCURSO_INDIVIDUAL,
                Observacoes = String.Empty,
                AlunoCodigo = ALUNO_CODIGO_1
            };

            var retorno = await salvarAcompanhamentoAlunoUseCase.Executar(acompanhamentoAlunoDto);
            retorno.ShouldNotBeNull();
            retorno.AcompanhamentoAlunoId.ShouldBe(1);

            var acompanhamentoAlunoSemestres = ObterTodos<AcompanhamentoAlunoSemestre>();
            acompanhamentoAlunoSemestres.ShouldNotBeNull();
            var acompanhamentoAlunoSemestre = acompanhamentoAlunoSemestres.FirstOrDefault();
            acompanhamentoAlunoSemestre.ShouldNotBeNull();
            acompanhamentoAlunoSemestre.PercursoIndividual.ShouldBe(TEXTO_PADRAO_PERCURSO_INDIVIDUAL);
        }

        [Fact(DisplayName = "Deve editar o percurso coletivo")]
        public async Task Ao_editar_percurso_coletivo()
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            await CriaAcompanhamentoColetivo();
            var acompanhamento = "Registro de percurso coletivo semestre";
            var useCase = ObterSalvarAcompanhamentoUseCase();
            var dto = new AcompanhamentoTurmaDto()
            {
                AcompanhamentoTurmaId = 1,
                TurmaId = TURMA_ID_1,
                Semestre = 1,
                ApanhadoGeral = acompanhamento
            };
            var registro = await useCase.Executar(dto);

            registro.ShouldNotBeNull();
            registro.ApanhadoGeral.ShouldBe(acompanhamento);
            registro.TurmaId.ShouldBe(TURMA_ID_1);
        }

        private async Task CriaAcomanhamentoIndividual()
        {
            await InserirNaBase(new AcompanhamentoAluno()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new AcompanhamentoAlunoSemestre()
            {
                PercursoIndividual = "teste",
                Semestre = 1,
                AcompanhamentoAlunoId = 1,
                Observacoes = "observações",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task CriaAcompanhamentoColetivo()
        {
            await InserirNaBase(new AcompanhamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                ApanhadoGeral = "Acompanhamento",
                Semestre = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }
    }
}
