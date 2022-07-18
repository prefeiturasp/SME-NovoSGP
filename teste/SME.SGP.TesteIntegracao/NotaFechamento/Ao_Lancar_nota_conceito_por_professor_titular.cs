using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamento
{
    public class Ao_Lancar_nota_conceito_por_professor_titular : NotaFechamentoTesteBase
    {
        public Ao_Lancar_nota_conceito_por_professor_titular(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_diferente_de_regencia()
        {
            await ExecuteTeste(ObterPerfilProfessor(), COMPONENTE_CURRICULAR_ARTES_ID_139, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_Fundamental() 
        {
            await ExecuteTeste(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105, Modalidade.Fundamental, ModalidadeTipoCalendario.Infantil);
        }

        [Fact]
        public async Task Deve_Lancar_nota_conceito_por_professor_titular_para_componente_de_regencia_EJA()
        {
            await ExecuteTeste(ObterPerfilProfessor(), COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114, Modalidade.EJA, ModalidadeTipoCalendario.EJA);
        }

        private async Task ExecuteTeste(string perfil, long componenteCurricular, Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            await CriarDadosBase(ObterFiltroNotas(perfil, ANO_3, componenteCurricular.ToString(), modalidade, modalidadeTipoCalendario));

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = componenteCurricular.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.S
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = componenteCurricular,
                        ConceitoId = (int)ConceitoValores.P
                    },
                }
            };

            await ExecutarComandosFechamentoFinal(dto);

            var turmaFechamento = ObterTodos<FechamentoTurma>();
            turmaFechamento.ShouldNotBeNull();
            turmaFechamento.FirstOrDefault().TurmaId.ShouldBe(TURMA_ID_1);
            var turmaFechamentoDiciplina = ObterTodos<FechamentoTurmaDisciplina>();
            turmaFechamentoDiciplina.ShouldNotBeNull();
            turmaFechamentoDiciplina.FirstOrDefault().DisciplinaId.ShouldBe(componenteCurricular);
            var alunoFechamento = ObterTodos<FechamentoAluno>();
            alunoFechamento.ShouldNotBeNull();
            var aluno = alunoFechamento.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1);
            aluno.ShouldNotBeNull();
            var notas = ObterTodos<FechamentoNota>();
            notas.ShouldNotBeNull();
            var nota = notas.FirstOrDefault(nota => nota.FechamentoAlunoId == aluno.Id);
            nota.ShouldNotBeNull();
            nota.ConceitoId.ShouldBe((int)ConceitoValores.NS);
            var listaConsolidacaoTurma = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            listaConsolidacaoTurma.ShouldNotBeNull();
            var consolidacaoTurma = listaConsolidacaoTurma.FirstOrDefault(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1);
            consolidacaoTurma.ShouldNotBeNull();
            consolidacaoTurma.TurmaId.ShouldBe(TURMA_ID_1);
            var listaConsolidacaoNotas = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
            listaConsolidacaoNotas.ShouldNotBeNull();
            var consolidacaoNotas = listaConsolidacaoNotas.FirstOrDefault(nota => nota.ConselhoClasseConsolidadoTurmaAlunoId == consolidacaoTurma.Id);
            consolidacaoNotas.ComponenteCurricularId.ShouldBe(componenteCurricular);
            consolidacaoNotas.ConceitoId.ShouldBe((int)ConceitoValores.NS);
        }

        private FiltroNotaFechamentoDto ObterFiltroNotas(
                                        string perfil, 
                                        string anoTurma, 
                                        string componenteCurricular,
                                        Modalidade modalidade,
                                        ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = TipoNota.Conceito,
                AnoTurma = anoTurma
            };
        }
    }
}
