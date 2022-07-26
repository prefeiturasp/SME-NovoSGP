using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_cp_e_cj : CompensacaoDeAusenciaTesteBase
    {
        
        public Ao_lancar_compensacao_de_ausencia_cp_e_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //bulk insert
        //[Fact]
        public async Task Deve_lancar_ausencia_para_cp()
        {
            await ExecuteTeste(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_diretor()
        {
            await ExecuteTeste(ObterPerfilDiretor(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_cj()
        {
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecuteTeste(ObterPerfilCJ(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_diretor_regencia_de_classe()
        {
            await ExecuteTeste(ObterPerfilDiretor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();
        }

        //[Fact]
        public async Task Deve_lancar_ausencia_para_cp_regencia_de_classe()
        {
            await ExecuteTeste(ObterPerfilCP(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), ObtenhaListaDeRegencia());
            TesteDisciplinasRegentes();
        }

        private void TesteDisciplinasRegentes()
        {
            var listaCompencaoRegencia = ObterTodos<CompensacaoAusenciaDisciplinaRegencia>();
            listaCompencaoRegencia.ShouldNotBeNull();
            var listaIdDisciplinas = listaCompencaoRegencia.Select(regencia => regencia.DisciplinaId).ToList();
            listaIdDisciplinas.Except(ObtenhaListaDeRegencia()).Count().ShouldBe(0);
        }

        private async Task ExecuteTeste(string perfil, string componente, List<string> listaDisciplinaRegente = null)
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(perfil, componente);
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

        private CompensacaoDeAusenciaDBDto ObtenhaDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_03_01_INICIO_BIMESTRE_1,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
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

        private List<string> ObtenhaListaDeRegencia()
        {
            return new List<string>
            {
                COMPONENTE_CIENCIAS_ID_89,
                COMPONENTE_GEOGRAFIA_ID_8,
                COMPONENTE_HISTORIA_ID_7,
                COMPONENTE_MATEMATICA_ID_2
            };
        }
    }
}
