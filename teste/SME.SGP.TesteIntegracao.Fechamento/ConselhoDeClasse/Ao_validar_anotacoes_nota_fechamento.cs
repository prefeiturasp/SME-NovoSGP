using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_validar_anotacoes_nota_fechamento : ConselhoDeClasseTesteBase
    {
        public Ao_validar_anotacoes_nota_fechamento(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact]
        public async Task Ao_consultar_valide_anotacao_fechamento()
        {
            var dtoFiltro = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Medio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };
            await CriarDadosBase(dtoFiltro);
            await InserirConselhoClassePadrao(dtoFiltro);
            await CriaFechamento();

            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();
            consulta.ShouldNotBeNull();
            
            var retorno = await consulta.Executar(new ConselhoClasseRecomendacaoDto()
                {
                    ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                    FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                    AlunoCodigo = ALUNO_CODIGO_1,
                    CodigoTurma = TURMA_CODIGO_1,
                    Bimestre = BIMESTRE_1}
            );
            
            retorno.ShouldNotBeNull();
            retorno.AnotacoesAluno.ShouldNotBeNull();
            retorno.AnotacoesAluno.Count().ShouldBeGreaterThan(0);
            foreach (var componenteId in ObterComponentesCurriculares())
            {
                retorno.AnotacoesAluno.ToList().Exists(anotacao => anotacao.DisciplinaId == componenteId.ToString()).ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Deve_verificar_se_o_estudante_possui_fechamento_do_ultimo_bimestre_ao_acessar_a_aba_final_fund_med()
        {
            var dtoFiltro = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Medio,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };
            await CriarDadosBase(dtoFiltro);
            //await InserirConselhoClassePadrao(dtoFiltro);
            await CriaFechamento();

            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => consulta.Executar(new ConselhoClasseRecomendacaoDto()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = BIMESTRE_FINAL
            }
            ));

            excecao.Message.ShouldBe("Para acessar esta aba você precisa registrar o conselho de classe do 4º bimestre");
        }

        [Fact]
        public async Task Deve_verificar_se_o_estudante_possui_fechamento_do_ultimo_bimestre_ao_acessar_a_aba_final_eja_celp()
        {
            var dtoFiltro = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.CELP,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoNota = TipoNota.Nota,
                AnoTurma = ANO_3,
                TipoCalendario = ModalidadeTipoCalendario.CELP
            };
            await CriarDadosBase(dtoFiltro);
            //await InserirConselhoClassePadrao(dtoFiltro);
            await CriaFechamento();

            var consulta = ServiceProvider.GetService<IConsultaConselhoClasseRecomendacaoUseCase>();

            var excecao = await Assert.ThrowsAsync<NegocioException>(() => consulta.Executar(new ConselhoClasseRecomendacaoDto()
            {
                ConselhoClasseId = CONSELHO_CLASSE_ID_1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                CodigoTurma = TURMA_CODIGO_1,
                Bimestre = BIMESTRE_FINAL
            }
            ));

            excecao.Message.ShouldBe("Para acessar esta aba você precisa registrar o conselho de classe do 2º bimestre");
        }

        private async Task CriaFechamento()
        {
            var fechamentoTurmaDisciplinaId = 1;

            foreach(var componente in ObterComponentesCurriculares())
            {
                await CriarFechamentoTurmaDisciplina(componente, FECHAMENTO_TURMA_ID_1);

                await CriarFechamentoTurmaAluno(fechamentoTurmaDisciplinaId);

                await CriaAnotacao(fechamentoTurmaDisciplinaId);

                fechamentoTurmaDisciplinaId++;
            }
        }

        private async Task CriaAnotacao(long fechamentoAlunoId)
        {
            await InserirNaBase(new Dominio.AnotacaoFechamentoAluno()
            {
                FechamentoAlunoId = fechamentoAlunoId,
                Anotacao = $"Anotação fechamento aluno {fechamentoAlunoId}",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFechamentoTurmaAluno(long fechamentoTurmaDisciplinaId)
        {
            await InserirNaBase(new FechamentoAluno()
            {
                FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
