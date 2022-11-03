using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base
{
    public abstract class Ao_lancar_compensacao_de_ausencia_base : CompensacaoDeAusenciaTesteBase
    {
        protected Ao_lancar_compensacao_de_ausencia_base(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ProfessorPodePersistirTurmaQuery, bool>), typeof(ProfessorPodePersistirTurmaQueryFake), ServiceLifetime.Scoped));            
        }

        protected async Task ExecuteTeste(CompensacaoDeAusenciaDBDto dtoDadoBase, List<string> listaDisciplinaRegente = null)
        {
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);
            await CriaRegistroDeFrequencia();

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObtenhaListaDeAlunos(), listaDisciplinaRegente);

            await comando.Inserir(dto);

            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();
            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull();
            TesteCompensacaoAluno(listaDaCompensacaoAluno, CODIGO_ALUNO_1, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, CODIGO_ALUNO_2, QUANTIDADE_AULA_2);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, CODIGO_ALUNO_3, QUANTIDADE_AULA);
            TesteCompensacaoAluno(listaDaCompensacaoAluno, CODIGO_ALUNO_4, QUANTIDADE_AULA);
        }

        protected void TesteDisciplinasRegentes()
        {
            var listaCompencaoRegencia = ObterTodos<CompensacaoAusenciaDisciplinaRegencia>();
            listaCompencaoRegencia.ShouldNotBeNull();
            var listaIdDisciplinas = listaCompencaoRegencia.Select(regencia => regencia.DisciplinaId).ToList();
            listaIdDisciplinas.Except(ObtenhaListaDeRegencia()).Count().ShouldBe(0);
        }

        protected List<string> ObtenhaListaDeRegencia()
        {
            return new List<string>
            {
                COMPONENTE_CIENCIAS_ID_89,
                COMPONENTE_GEOGRAFIA_ID_8,
                COMPONENTE_HISTORIA_ID_7,
                COMPONENTE_MATEMATICA_ID_2
            };
        }

        protected CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(
                        string componente,
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipo,
                        string ano)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipo,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ano,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }

        private void TesteCompensacaoAluno(List<CompensacaoAusenciaAluno> listaDaCompensacaoAluno, string codigoAluno, int quantidade)
        {
            var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == codigoAluno);
            compensacaoAluno.ShouldNotBeNull();
            compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(quantidade);
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

            await CriaFrequenciaAlunos(
            dtoDadoBase,
            CODIGO_ALUNO_4,
            QUANTIDADE_AULA_3,
            QUANTIDADE_AULA);
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
                DATA_29_04_FIM_BIMESTRE_1,
                codigoAluno,
                totalPresenca,
                totalAusencia,
                PERIODO_ESCOLAR_ID_1);
        }

        private List<CompensacaoAusenciaAlunoDto> ObtenhaListaDeAlunos()
        {
            return new List<CompensacaoAusenciaAlunoDto>()
            {
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_1,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_2,
                    QtdFaltasCompensadas = QUANTIDADE_AULA_2
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_3,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                },
                new CompensacaoAusenciaAlunoDto()
                {
                    Id = CODIGO_ALUNO_4,
                    QtdFaltasCompensadas = QUANTIDADE_AULA
                }
            };
        }

        private async Task CriaRegistroDeFrequencia()
        {
            await CrieRegistroDeFrenquencia();
            await RegistroFrequenciaAluno(CODIGO_ALUNO_1, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_2, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_3, QUANTIDADE_AULA, TipoFrequencia.F);
            await RegistroFrequenciaAluno(CODIGO_ALUNO_4, QUANTIDADE_AULA, TipoFrequencia.F);
        }
    }
}
