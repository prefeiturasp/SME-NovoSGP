using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_obter_ausencia_motivo_por_Aluno : FrequenciaTesteBase
    {
        private const int MOTIVO_ID = 1;
        private const string DESCRICAO_JUSTIFICATIVA = "Covid";
        private const string DESCRICAO_MOTIVO = "Doença";
        public Ao_obter_ausencia_motivo_por_Aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Frequência - Deve obter ausencia motivo por Aluno")]
        public async Task Deve_obter_ausencia_motivo_por_Aluno()
        {
            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), true, NUMERO_AULAS_1);
            await CrieAnotacaoFrequencia();

            var mediator = ServiceProvider.GetService<IMediator>();
            var lista = await mediator.Send(new ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery(CODIGO_ALUNO_1, TURMA_CODIGO_1, BIMESTRE_2, (short)DateTimeExtension.HorarioBrasilia().Year));

            lista.ShouldNotBeNull();
            lista.ToList().Exists(anotacao => anotacao.JustificativaAusencia == DESCRICAO_JUSTIFICATIVA).ShouldBeTrue();

        }

        private async Task CrieAnotacaoFrequencia()
        {
            await InserirNaBase(new MotivoAusencia
            {
                Descricao = DESCRICAO_MOTIVO
            });

            await InserirNaBase(new AnotacaoFrequenciaAluno
            {
                AulaId = AULA_ID,
                CodigoAluno = CODIGO_ALUNO_1,
                MotivoAusenciaId = MOTIVO_ID,
                Anotacao = DESCRICAO_JUSTIFICATIVA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }

}
