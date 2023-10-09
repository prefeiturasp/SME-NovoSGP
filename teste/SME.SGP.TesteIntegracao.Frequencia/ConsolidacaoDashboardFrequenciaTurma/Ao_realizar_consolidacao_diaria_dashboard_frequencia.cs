using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsolidacaoDashboardFrequenciaTurma
{
    public class Ao_realizar_consolidacao_diaria_dashboard_frequencia : ConsolidacaoDashBoardBase
    {

        public Ao_realizar_consolidacao_diaria_dashboard_frequencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        [Fact(DisplayName = "Consolidação Dashboard - Deve gerar a consolidação diaria com data")]
        public async Task Deve_gerar_consolidacao_diaria_com_data()
        {
            await CriarItensBasicos();

            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date.AddDays(-1);

            await CriarAulas(dataReferencia);

            await CriarRegistroFrequencia();

            await CriarRegistroFrequenciaAluno();
            
            var useCase = ServiceProvider.GetService<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase>();
            var mensagem = new ConsolidacaoPorTurmaDashBoardFrequencia()
            {
                AnoLetivo = dataReferencia.Year, 
                TurmaId = ConstantesTeste.TURMA_ID_1,
                Mes = dataReferencia.Month, 
                DataAula = dataReferencia
            };
            
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            var consolidacoes = ObterTodos<ConsolidacaoDashBoardFrequencia>();
            consolidacoes.ShouldNotBeEmpty();
            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().DataAula.ShouldBe(dataReferencia);
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeAusentes.ShouldBe(1);
            consolidacoes.FirstOrDefault(c=> c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadePresencas.ShouldBe(2);
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeRemotos.ShouldBe(1);
        }
       
        [Fact(DisplayName = "Consolidação Dashboard - Deve atualizar a consolidação diaria com data")]
        public async Task Deve_atualizar_consolidacao_diaria_com_data()
        {
            await CriarItensBasicos();

            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date.AddDays(-1);

            await CriarAulas(dataReferencia);

            await CriarRegistroFrequencia();

            await CriarRegistroFrequenciaAluno();
            
            await InserirNaBase(new ConsolidacaoDashBoardFrequencia
            {
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                UeId = ConstantesTeste.UE_1_ID,
                DataAula = dataReferencia,
                AnoLetivo = dataReferencia.Year,
                Mes = dataReferencia.Month,
                TurmaId = ConstantesTeste.TURMA_ID_1,
                TurmaNome = ConstantesTeste.TURMA_NOME_1A,
                TurmaAno = ConstantesTeste.TURMA_ANO_1,
                DreId = ConstantesTeste.DRE_1_ID,
                DreAbreviacao = ConstantesTeste.DRE_1_NOME,
                DreCodigo = ConstantesTeste.DRE_1_CODIGO,
                ModalidadeCodigo = (int)Modalidade.Fundamental,
                QuantidadeAusentes = 2,
                QuantidadePresencas = 1,
                QuantidadeRemotos = 2,
                Tipo = (int)TipoPeriodoDashboardFrequencia.Diario,
                semestre = 0
            });
            
            var consolidacoes = ObterTodos<ConsolidacaoDashBoardFrequencia>();
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeAusentes.ShouldBe(2);
            consolidacoes.FirstOrDefault(c=> c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadePresencas.ShouldBe(1);
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeRemotos.ShouldBe(2);
            
            var useCase = ServiceProvider.GetService<IExecutaConsolidacaoDiariaDashBoardFrequenciaPorTurmaUseCase>();
            var mensagem = new ConsolidacaoPorTurmaDashBoardFrequencia()
            {
                AnoLetivo = dataReferencia.Year, 
                Mes = dataReferencia.Month, 
                TurmaId = ConstantesTeste.TURMA_ID_1,
                DataAula = dataReferencia
            };
            
            var jsonMensagem = JsonSerializer.Serialize(mensagem);

            await useCase.Executar(new MensagemRabbit(jsonMensagem));

            consolidacoes = ObterTodos<ConsolidacaoDashBoardFrequencia>();
            consolidacoes.ShouldNotBeEmpty();
            consolidacoes.Count.ShouldBe(1);
            consolidacoes.FirstOrDefault().DataAula.ShouldBe(dataReferencia);
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeAusentes.ShouldBe(1);
            consolidacoes.FirstOrDefault(c=> c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadePresencas.ShouldBe(2);
            consolidacoes.FirstOrDefault(c => c.TurmaId == ConstantesTeste.TURMA_ID_1).QuantidadeRemotos.ShouldBe(1);
        }
        
        private async Task CriarRegistroFrequenciaAluno()
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_1,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_1,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.COMPARECEU_ID_1,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_2,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_1,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_3,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_1,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.COMPARECEU_ID_1,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_4,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_1,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_1
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_1,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_2,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_2,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_2,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.REMOTO_ID_3,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_3,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_2,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_4,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_2,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_2
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_1,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_3,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.REMOTO_ID_3,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_3
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_2,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_3,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.REMOTO_ID_3,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_3
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_3,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_3,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.COMPARECEU_ID_1,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_3
            });

            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = ConstantesTeste.ALUNO_CODIGO_4,
                RegistroFrequenciaId = ConstantesTeste.REGISTRO_FREQUENCIA_ID_3,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                Valor = ConstantesTeste.FALTOU_ID_2,
                NumeroAula = ConstantesTeste.QUANTIDADE_AULAS_1,
                AulaId = ConstantesTeste.AULA_ID_3
            });
        }

        private async Task CriarRegistroFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = ConstantesTeste.AULA_ID_1,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = ConstantesTeste.AULA_ID_2,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });

            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = ConstantesTeste.AULA_ID_3,
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date
            });
        }

        private async Task CriarAulas(DateTime dataReferencia)
        {
            await InserirNaBase(new Dominio.Aula
            {
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                UeId = ConstantesTeste.UE_1_CODIGO,
                DisciplinaId = ConstantesTeste.COMPONENTE_CURRICULAR_ARTE_139,
                TurmaId = ConstantesTeste.TURMA_CODIGO_1,
                TipoCalendarioId = ConstantesTeste.TIPO_CALENDARIO_ID_1,
                DataAula = dataReferencia,
                Quantidade = ConstantesTeste.QUANTIDADE_AULAS_1,
                ProfessorRf = ConstantesTeste.SISTEMA_RF
            });

            await InserirNaBase(new Dominio.Aula
            {
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                UeId = ConstantesTeste.UE_1_CODIGO,
                DisciplinaId = ConstantesTeste.COMPONENTE_CURRICULAR_MATEMATICA_2,
                TurmaId = ConstantesTeste.TURMA_CODIGO_1,
                TipoCalendarioId = ConstantesTeste.TIPO_CALENDARIO_ID_1,
                DataAula = dataReferencia,
                Quantidade = ConstantesTeste.QUANTIDADE_AULAS_1,
                ProfessorRf = ConstantesTeste.SISTEMA_RF
            });

            await InserirNaBase(new Dominio.Aula
            {
                CriadoPor = ConstantesTeste.SISTEMA_NOME, CriadoRF = ConstantesTeste.SISTEMA_RF,
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date,
                UeId = ConstantesTeste.UE_1_CODIGO,
                DisciplinaId = ConstantesTeste.COMPONENTE_CURRICULAR_ARTE_139,
                TurmaId = ConstantesTeste.TURMA_CODIGO_1,
                TipoCalendarioId = ConstantesTeste.TIPO_CALENDARIO_ID_1,
                DataAula = dataReferencia,
                Quantidade = ConstantesTeste.QUANTIDADE_AULAS_1,
                ProfessorRf = ConstantesTeste.SISTEMA_RF
            });
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = ConstantesTeste.DRE_1_CODIGO,
                Abreviacao = ConstantesTeste.DRE_1_NOME,
                Nome = ConstantesTeste.DRE_1_NOME
            });

            await InserirNaBase(new Ue
            {
                CodigoUe = ConstantesTeste.UE_1_CODIGO,
                DreId = ConstantesTeste.DRE_1_ID,
                Nome = ConstantesTeste.UE_1_NOME,
            });

            await InserirNaBase(new Dominio.Turma
            {
                UeId = ConstantesTeste.UE_1_ID,
                Ano = ConstantesTeste.TURMA_ANO_1,
                CodigoTurma = ConstantesTeste.TURMA_CODIGO_1,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Fundamental,
                Nome = ConstantesTeste.TURMA_NOME_1A
            });

            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = ConstantesTeste.TIPO_CALENDARIO_NOME_1,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = ConstantesTeste.SISTEMA_NOME,
                CriadoRF = ConstantesTeste.SISTEMA_RF
            });

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
        }
    }
}
