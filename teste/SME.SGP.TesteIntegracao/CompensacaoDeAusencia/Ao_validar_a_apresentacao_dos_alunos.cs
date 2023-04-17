using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Shouldly;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Infra;
using System.Collections.Generic;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_validar_a_apresentacao_dos_alunos : CompensacaoDeAusenciaTesteBase
    {
        public Ao_validar_a_apresentacao_dos_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEDataMatriculaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));            
        }

        [Fact]
        public async Task Deve_apresentar_somente_alunos_que_possuem_ausências()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            var dtoDadoBase = ObterDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var listaDeAusencia = await mediator.Send(new ObterListaAlunosComAusenciaQuery(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), BIMESTRE_3));
            listaDeAusencia.ShouldNotBeNull();
            listaDeAusencia.ToList().Exists(aluno => aluno.Id == CODIGO_ALUNO_1).ShouldBeTrue();
            listaDeAusencia.ToList().Exists(aluno => aluno.Id == CODIGO_ALUNO_2).ShouldBeTrue();
            listaDeAusencia.ToList().Exists(aluno => aluno.Id == CODIGO_ALUNO_3).ShouldBeTrue();
            listaDeAusencia.ToList().Exists(aluno => aluno.Id == CODIGO_ALUNO_4).ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_apresentar_alerta_para_alunos_com_frequencia_abaixo_de_75_porcento()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            var dtoDadoBase = ObterDtoDadoBase(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var listaDeAusencia = await mediator.Send(new ObterListaAlunosComAusenciaQuery(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), BIMESTRE_1));
            listaDeAusencia.ShouldNotBeNull();
            var listaAusenciaMenor75 = listaDeAusencia.ToList().FindAll(aluno => aluno.PercentualFrequencia <= 75);
            listaAusenciaMenor75.ShouldNotBeNull();
            listaAusenciaMenor75.ForEach(aluno => aluno.Alerta.ShouldBeTrue());
        }

        private async Task CriaFrequenciaAlunos(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
                        dtoDadoBase,
                        CODIGO_ALUNO_1, 
                        QUANTIDADE_AULA_3,
                        QUANTIDADE_AULA);

            await CriaFrequenciaAlunos(
                        dtoDadoBase,
                        CODIGO_ALUNO_2,
                        QUANTIDADE_AULA,
                        QUANTIDADE_AULA_3);

            await CriaFrequenciaAlunos(
                        dtoDadoBase,
                        CODIGO_ALUNO_3,
                        QUANTIDADE_AULA_2,
                        QUANTIDADE_AULA_2);
        }

        private async Task CriaFrequenciaAlunos(
                                CompensacaoDeAusenciaDBDto dtoDadoBase,
                                string codigoAluno,
                                int totalPresenca,
                                int totalAusencia)
        {
            await CriaFrequenciaAluno(
                        dtoDadoBase,
                        DATA_03_01_INICIO_BIMESTRE_1,
                        DATA_28_04_FIM_BIMESTRE_1,
                        codigoAluno,
                        totalPresenca,
                        totalAusencia,
                        PERIODO_ESCOLAR_ID_3);
        }
    }
}
