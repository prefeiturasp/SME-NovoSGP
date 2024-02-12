using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia.CompensacaoDeAusencia
{
    public class Ao_obter_total_compensacoes : CompensacaoDeAusenciaTesteBase
    {
        public Ao_obter_total_compensacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_total_compensacoes_componente_nao_lanca_nota()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            var dtoDadoBase = CriaCompensacoesAsusencias(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_LEITURA_OSL_ID_1061.ToString());
            var primeiraCompensacao = dtoDadoBase.First();
            await CriarDadosBase(primeiraCompensacao);
            await CriaFrequenciaAlunos(dtoDadoBase);

            var retorno = await mediator.Send(new ObterTotalCompensacoesComponenteNaoLancaNotaQuery(TURMA_CODIGO_1, BIMESTRE_FINAL));

            Assert.True(retorno.Count() > 1);
        }
        private async Task CriaFrequenciaAlunos(List<CompensacaoDeAusenciaDBDto> dtoDadoBase)
        {
            await CriaFrequenciaAlunos(
                        dtoDadoBase,
                        CODIGO_ALUNO_1,
                        QUANTIDADE_AULA_3,
                        QUANTIDADE_AULA_2);

        }

        private async Task CriaFrequenciaAlunos(
                                List<CompensacaoDeAusenciaDBDto> dtoDadoBase,
                                string codigoAluno,
                                int totalPresenca,
                                int totalAusencia)
        {
            await CriaFrequenciaAluno(
                        dtoDadoBase.First(x=>x.Bimestre == BIMESTRE_2),
                        DATA_INICIO_BIMESTRE_2,
                        DATA_FIM_BIMESTRE_2,
                        codigoAluno,
                        totalPresenca,
                        totalAusencia,
                        PERIODO_ESCOLAR_CODIGO_3,
                        QUANTIDADE_AULA_2);
            await CriaFrequenciaAluno(
                        dtoDadoBase.First(x => x.Bimestre == BIMESTRE_4),
                        DATA_INICIO_BIMESTRE_4,
                        DATA_FIM_BIMESTRE_4,
                        codigoAluno,
                        totalPresenca,
                        totalAusencia,
                        PERIODO_ESCOLAR_CODIGO_3,
                        QUANTIDADE_AULA_2);
        }
        private List<CompensacaoDeAusenciaDBDto> CriaCompensacoesAsusencias(string perfil, string componente)
        {
           return new List<CompensacaoDeAusenciaDBDto>() {
                new CompensacaoDeAusenciaDBDto()
                {
                    Perfil = perfil,
                    Modalidade = Modalidade.Fundamental,
                    TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                    Bimestre = BIMESTRE_2,
                    ComponenteCurricular = componente,
                    TipoCalendarioId = TIPO_CALENDARIO_1,
                    AnoTurma = ANO_5,
                    DataReferencia = DATA_INICIO_BIMESTRE_2,
                    QuantidadeAula = QUANTIDADE_AULA_4
                },
                new CompensacaoDeAusenciaDBDto()
                {
                    Perfil = perfil,
                    Modalidade = Modalidade.Fundamental,
                    TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                    Bimestre = BIMESTRE_4,
                    ComponenteCurricular = componente,
                    TipoCalendarioId = TIPO_CALENDARIO_1,
                    AnoTurma = ANO_5,
                    DataReferencia = DATA_INICIO_BIMESTRE_4,
                    QuantidadeAula = QUANTIDADE_AULA_4
                }
            };
        }
    }
}
