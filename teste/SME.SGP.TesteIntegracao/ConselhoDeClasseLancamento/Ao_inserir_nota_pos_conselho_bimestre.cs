using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento
{
    public class Ao_inserir_nota_pos_conselho_bimestre : ConselhoDeClasseLancamentoBase
    {
        private const string JUSTIFICATIVA = "Nota pós conselho";
        public Ao_inserir_nota_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_fundamental(bool anoAnterior) 
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                TipoNota.Nota, 
                ANO_7, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_medio(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                TipoNota.Nota, 
                ANO_7, 
                Modalidade.Medio, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_eja(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                TipoNota.Nota, 
                ANO_9, 
                Modalidade.EJA, 
                ModalidadeTipoCalendario.EJA,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_CURRICULAR_PORTUGUES_ID_138, anoAnterior);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_regencia_classe(bool anoAnterior)
        {
            await CrieDados(
                ObterPerfilProfessor(), 
                COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, 
                TipoNota.Nota, 
                ANO_1, 
                Modalidade.Fundamental, 
                ModalidadeTipoCalendario.FundamentalMedio,
                anoAnterior);

            await ExecuteTeste(COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, anoAnterior);
        }

        private async Task ExecuteTeste(long componente, bool anoAnterior)
        {
            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();
            var dto = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = componente,
                Nota = 7,
                Justificativa = JUSTIFICATIVA
            };
            var dtoRetorno = await comando.SalvarAsync(dto, ALUNO_CODIGO_1, CONSELHO_CLASSE_ID, FECHAMENTO_TURMA_ID, TURMA_CODIGO_1, BIMESTRE_2);

            dtoRetorno.ShouldNotBeNull();
            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();
            listaConselhoClasseNota.ShouldNotBeNull();
            var classeNota = listaConselhoClasseNota.FirstOrDefault(nota => nota.ConselhoClasseAlunoId == 1);

            if (anoAnterior)
            {
                classeNota.Nota.ShouldBe(7);
            } else
            {
                classeNota.Nota.ShouldBeNull();
                var listaAprovacaoNotaConselho = ObterTodos<WFAprovacaoNotaConselho>();
                listaAprovacaoNotaConselho.ShouldNotBeNull();
                var aprovacaoNotaConselho = listaAprovacaoNotaConselho.FirstOrDefault(aprovacao => aprovacao.ConselhoClasseNotaId == classeNota.Id);
                aprovacaoNotaConselho.ShouldNotBeNull();
                aprovacaoNotaConselho.Nota.ShouldBe(7);
            }
            
            var listaConsolidado = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            listaConsolidado.ShouldNotBeNull();
            var consolidado = listaConsolidado.FirstOrDefault(consolidadoAluno => consolidadoAluno.AlunoCodigo == ALUNO_CODIGO_1);
        }

        private async Task CrieDados(
                        string perfil, 
                        long componente,
                        TipoNota tipo, 
                        string anoTurma, 
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipoCalendario,
                        bool anoAnterior)
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade ,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = componente.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = anoAnterior
            };
            var dataAula = anoAnterior ? DATA_02_05_INICIO_BIMESTRE_2.AddYears(-1) : DATA_02_05_INICIO_BIMESTRE_2;

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
        }
    }
}
