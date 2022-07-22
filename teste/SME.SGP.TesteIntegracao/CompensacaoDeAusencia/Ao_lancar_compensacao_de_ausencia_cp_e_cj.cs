using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_de_ausencia_cp_e_cj : CompensacaoDeAusenciaTesteBase
    {
        private const int PERIODO_ESCOLAR_ID_1 = 1;
        public Ao_lancar_compensacao_de_ausencia_cp_e_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //bulk insert
        //[Fact]
        public async Task Deve_lancar_ausencia_para_cp()
        {
            var dtoDadoBase = ObtenhaDtoDadoBase(ObterPerfilCP(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());
            await CriarDadosBase(dtoDadoBase);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            var dto = ObtenhaCompensacaoAusenciaDto(dtoDadoBase, ObtenhaListaDeAlunos());

            await comando.Inserir(dto); 
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
    }
}
