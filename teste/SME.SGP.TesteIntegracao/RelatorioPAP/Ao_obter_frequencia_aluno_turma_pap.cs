using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.RelatorioPAP.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioPAP
{
    public class Ao_obter_frequencia_aluno_turma_pap : RelatorioPAPTesteBase
    {
        public Ao_obter_frequencia_aluno_turma_pap(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Obter a frequencia da turma para um periodo")]
        public async Task Ao_obter_frequencia_turma_pap_um_periodo()
        {
            await CriarDadosBase();

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 30,
                DisciplinaId = COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(),
                Bimestre = BIMESTRE_1,
                TotalPresencas = 28,
                TotalAusencias = 1,
                TotalCompensacoes = 1,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var configuracao = new ConfiguracaoRelatorioPAP()
            {
                Id = 1,
                InicioVigencia = DATA_03_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_01_05_FIM_BIMESTRE_1,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL
            };

            var periodoRelatorioPap = new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = 1,
                Configuracao = configuracao,
                PeriodosEscolaresRelatorio = new List<PeriodoEscolarRelatorioPAP>()
                {
                    new PeriodoEscolarRelatorioPAP()
                    {
                        PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                        PeriodoRelatorioId = 1
                    }
                }
            };

            var mediator = ServiceProvider.GetService<IMediator>();
            var frequencia = await mediator.Send(new ObterFrequenciaTurmaPAPQuery(TURMA_CODIGO_1, CODIGO_ALUNO_1, periodoRelatorioPap));

            frequencia.ShouldNotBeNull();
            frequencia.AlunoCodigo.ShouldBe(CODIGO_ALUNO_1);
            frequencia.PercentualFrequencia.ShouldBe(100);
            frequencia.TotalPresencas.ShouldBe(28);
            frequencia.TotalAulas.ShouldBe(30);
            frequencia.TotalAusencias.ShouldBe(1);
            frequencia.TotalCompensacoes.ShouldBe(1);
        }

        [Fact(DisplayName = "Obter a frequencia da turma para todos periodo")]
        public async Task Ao_obter_frequencia_turma_pap_todos_periodo()
        {
            await CriarDadosBase();

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 1,
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 30,
                DisciplinaId = COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(),
                PeriodoInicio = DATA_03_01_INICIO_BIMESTRE_1,
                PeriodoFim = DATA_01_05_FIM_BIMESTRE_1,
                Bimestre = BIMESTRE_1,
                TotalPresencas = 28,
                TotalAusencias = 1,
                TotalCompensacoes = 1,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id= 2,
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 5,
                DisciplinaId = COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(),
                PeriodoInicio = DATA_02_05_INICIO_BIMESTRE_2,
                PeriodoFim = DATA_24_07_FIM_BIMESTRE_2,
                Bimestre = BIMESTRE_2,
                TotalPresencas = 5,
                TotalAusencias = 0,
                TotalCompensacoes = 0,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 3,
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 15,
                DisciplinaId = COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(),
                PeriodoInicio = DATA_25_07_INICIO_BIMESTRE_3,
                PeriodoFim = DATA_02_10_FIM_BIMESTRE_3,
                Bimestre = BIMESTRE_3,
                TotalPresencas = 15,
                TotalAusencias = 0,
                TotalCompensacoes = 0,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Dominio.FrequenciaAluno()
            {
                Id = 4,
                CodigoAluno = CODIGO_ALUNO_1,
                TurmaId = TURMA_CODIGO_1,
                TotalAulas = 25,
                DisciplinaId = COMPONENTE_CURRICULAR_PAP_PROJETO_COLABORATIVO.ToString(),
                PeriodoInicio = DATA_03_10_INICIO_BIMESTRE_4,
                PeriodoFim = DATA_22_12_FIM_BIMESTRE_4,
                Bimestre = BIMESTRE_4,
                TotalPresencas = 15,
                TotalAusencias = 5,
                TotalCompensacoes = 0,
                Tipo = TipoFrequenciaAluno.Geral,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var configuracao = new ConfiguracaoRelatorioPAP()
            {
                Id = 1,
                InicioVigencia = DATA_03_01_INICIO_BIMESTRE_1,
                FimVigencia = DATA_22_12_FIM_BIMESTRE_4,
                TipoPeriocidade = ConstantesTestePAP.TIPO_PERIODICIDADE_BIMESTRAL
            };

            var periodoRelatorioPap = new PeriodoRelatorioPAP()
            {
                ConfiguracaoId = 1,
                Configuracao = configuracao,
                PeriodosEscolaresRelatorio = new List<PeriodoEscolarRelatorioPAP>()
                {
                    new PeriodoEscolarRelatorioPAP()
                    {
                        PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                        PeriodoRelatorioId = 1
                    },
                    new PeriodoEscolarRelatorioPAP()
                    {
                        PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                        PeriodoRelatorioId = 1
                    },
                    new PeriodoEscolarRelatorioPAP()
                    {
                        PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                        PeriodoRelatorioId = 1
                    },
                    new PeriodoEscolarRelatorioPAP()
                    {
                        PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                        PeriodoRelatorioId = 1
                    }
                }
            };

            var mediator = ServiceProvider.GetService<IMediator>();
            var frequencia = await mediator.Send(new ObterFrequenciaTurmaPAPQuery(TURMA_CODIGO_1, CODIGO_ALUNO_1, periodoRelatorioPap));

            frequencia.ShouldNotBeNull();
            frequencia.AlunoCodigo.ShouldBe(CODIGO_ALUNO_1);
            frequencia.PercentualFrequencia.ShouldBe(93.33);
            frequencia.TotalPresencas.ShouldBe(63);
            frequencia.TotalAulas.ShouldBe(75);
            frequencia.TotalAusencias.ShouldBe(6);
            frequencia.TotalCompensacoes.ShouldBe(1);
        }
    }
}
