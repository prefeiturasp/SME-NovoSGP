using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            await CriarDadosBase(ObterFiltroNotas(ANO_3));

            var comando = ServiceProvider.GetService<IComandosFechamentoFinal>();

            var dto = new FechamentoFinalSalvarDto()
            {
                DisciplinaId = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TurmaCodigo = TURMA_CODIGO_1,
                Itens = new List<FechamentoFinalSalvarItemDto>()
                {
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_1,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.NS
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_2,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.S
                    },
                    new FechamentoFinalSalvarItemDto()
                    {
                        AlunoRf = ALUNO_CODIGO_3,
                        ComponenteCurricularCodigo = COMPONENTE_CURRICULAR_ARTES_ID_139,
                        ConceitoId = (int)ConceitoValores.P
                    },
                }
            };

            await comando.SalvarAsync(dto);
            var turmaFechamento = ObterTodos<FechamentoTurma>();
            var turmaFechamentoDiciplina = ObterTodos<FechamentoTurmaDisciplina>();
            var alunoFechamento = ObterTodos<FechamentoAluno>();
            var notas = ObterTodos<FechamentoNota>();
            var consolidacaoTurma = ObterTodos<ConselhoClasseConsolidadoTurmaAluno>();
            var consolidacaoNotas = ObterTodos<ConselhoClasseConsolidadoTurmaAlunoNota>();
        }

        private FiltroNotaFechamentoDto ObterFiltroNotas(string anoTurma)
        {
            return new FiltroNotaFechamentoDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                AnoTurma = anoTurma
            };
        }
    }
}
