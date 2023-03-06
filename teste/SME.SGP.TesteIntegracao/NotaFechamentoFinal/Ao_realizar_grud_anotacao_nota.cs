using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    public class Ao_realizar_grud_anotacao_nota : NotaFechamentoTesteBase
    {
        private const string ANOTACAO = "Aluno em recuperação";
        private const string ANOTACAO_ALTERACAO = "Alteração da anotação";
        public Ao_realizar_grud_anotacao_nota(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerSemPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverArquivosExcluidosCommand, bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_inserir_anotacao_do_aluno()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            await CriaFechamento();

            var auditoria = await ExecuteTeste(ANOTACAO);

            auditoria.ShouldNotBeNull();
            var listaAnotacao = ObterTodos<Dominio.AnotacaoFechamentoAluno>();
            listaAnotacao.ShouldNotBeNull();
            var anotacao = listaAnotacao.FirstOrDefault();
            anotacao.ShouldNotBeNull();
            anotacao.Anotacao.ShouldBe(ANOTACAO);
        }

        [Fact]
        public async Task Ao_alterar_anotacao_do_aluno()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            await CriaFechamento();
            await CriaAnotacao();

            var auditoria = await ExecuteTeste(ANOTACAO_ALTERACAO);

            auditoria.ShouldNotBeNull();
            var listaAnotacao = ObterTodos<Dominio.AnotacaoFechamentoAluno>();
            listaAnotacao.ShouldNotBeNull();
            var anotacao = listaAnotacao.FirstOrDefault();
            anotacao.ShouldNotBeNull();
            anotacao.Anotacao.ShouldBe(ANOTACAO_ALTERACAO);
        }

        [Fact]
        public async Task Ao_excluir_anotacao_do_aluno()
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), TipoNota.Conceito, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            await CriaFechamento();
            await CriaAnotacao();

            var auditoria = await ExecuteTeste(string.Empty);

            auditoria.ShouldNotBeNull();
            var listaAnotacao = ObterTodos<Dominio.AnotacaoFechamentoAluno>();
            listaAnotacao.ShouldNotBeNull();
            listaAnotacao.Count.ShouldBe(0);
        }

        private async Task<AuditoriaPersistenciaDto> ExecuteTeste(string anotacao)
        {
            var useCase = ServiceProvider.GetService<ISalvarAnotacaoFechamentoAlunoUseCase>();
            var dto = new AnotacaoAlunoDto()
            {
                Anotacao = anotacao,
                CodigoAluno = CODIGO_ALUNO_1,
                FechamentoId = FECHAMENTO_ALUNO_ID_1
            };

            return await useCase.Executar(dto);
        }

        private async Task CriaAnotacao()
        {
            await InserirNaBase(new Dominio.AnotacaoFechamentoAluno()
            {
                FechamentoAlunoId = FECHAMENTO_ALUNO_ID_1,
                Anotacao = ANOTACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriaFechamento()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoAluno()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
