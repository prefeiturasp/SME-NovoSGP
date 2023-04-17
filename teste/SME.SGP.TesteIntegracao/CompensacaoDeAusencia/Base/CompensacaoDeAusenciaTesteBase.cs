using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base
{
    public abstract class CompensacaoDeAusenciaTesteBase : TesteBaseComuns
    {
        protected const int COMPENSACAO_AUSENCIA_ID_1 = 1;
        protected const int PERIODO_ESCOLAR_ID_3 = 3;
        protected const int REGISTRO_FREQUENCIA_ID_1 = 1;

        protected const string DESCRICAO_COMPENSACAO = "Compensação de ausência teste";
        protected const string ATIVIDADE_COMPENSACAO = "Atividade teste";

        protected CompensacaoDeAusenciaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IServicoAuditoria), typeof(ServicoAuditoriaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>), typeof(IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaConsolidarDashBoardFrequenciaCommand, bool>), typeof(IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBase(CompensacaoDeAusenciaDBDto dtoDB)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(dtoDB.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(dtoDB);

            await CriarAula(dtoDB);

            if (dtoDB.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(dtoDB.ConsiderarAnoAnterior);

            if (dtoDB.CriarPeriodoAbertura)
                await CriarPeriodoReabertura(dtoDB.TipoCalendarioId);

            await CriarAbrangencia(dtoDB.Perfil);

            await CriarParametrosSistema(dtoDB.ConsiderarAnoAnterior, dtoDB.PermiteCompensacaoForaPeriodoAtivo);
        }

        protected async Task CriarAula(CompensacaoDeAusenciaDBDto dtoDB)
        {
            await InserirNaBase(
                            new Dominio.Aula
                            {
                                UeId = UE_CODIGO_1,
                                DisciplinaId = dtoDB.ComponenteCurricular,
                                TurmaId = TURMA_CODIGO_1,
                                TipoCalendarioId = dtoDB.TipoCalendarioId,
                                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                                Quantidade = dtoDB.QuantidadeAula,
                                DataAula = dtoDB.DataReferencia.GetValueOrDefault(),
                                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                                TipoAula = TipoAula.Normal,
                                CriadoEm = DateTime.Now,
                                CriadoPor = SISTEMA_NOME,
                                CriadoRF = SISTEMA_CODIGO_RF,
                                AulaCJ = dtoDB.AulaCj
                            });
        }

        protected CompensacaoAusenciaDto ObtenhaCompensacaoAusenciaDto(CompensacaoDeAusenciaDBDto dtoDadoBase, List<CompensacaoAusenciaAlunoDto> listaAlunos, List<string> listaDisciplinaRegente = null)
        {
            return new CompensacaoAusenciaDto()
            {
                TurmaId = TURMA_CODIGO_1,
                Alunos = listaAlunos,
                Bimestre = dtoDadoBase.Bimestre,
                Atividade = ATIVIDADE_COMPENSACAO,
                Descricao = DESCRICAO_COMPENSACAO,
                DisciplinaId = dtoDadoBase.ComponenteCurricular,
                DisciplinasRegenciaIds = listaDisciplinaRegente
            };
        }

        protected async Task CriaCompensacaoAusencia(CompensacaoDeAusenciaDBDto dtoDadoBase)
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = dtoDadoBase.ComponenteCurricular,
                Bimestre = dtoDadoBase.Bimestre,
                TurmaId = TURMA_ID_1,
                Nome = ATIVIDADE_COMPENSACAO,
                Descricao = DESCRICAO_COMPENSACAO,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriaFrequenciaAluno(
                                CompensacaoDeAusenciaDBDto dbDto,
                                DateTime periodoInicio,
                                DateTime periodoFim,
                                string codigoAluno,
                                int totalPresenca,
                                int totalAusencia,
                                long PeriodoEscolarId,
                                int totalCompensacoes = 0)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                PeriodoInicio = periodoInicio,
                PeriodoFim = periodoFim,
                Bimestre = dbDto.Bimestre,
                TotalAulas = dbDto.QuantidadeAula,
                TotalAusencias = totalAusencia,
                TotalCompensacoes = totalCompensacoes,
                PeriodoEscolarId = PeriodoEscolarId,
                TotalPresencas = totalPresenca,
                TotalRemotos = 0,
                DisciplinaId = dbDto.ComponenteCurricular,
                CodigoAluno = codigoAluno,
                TurmaId = TURMA_CODIGO_1,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected async Task CrieRegistroDeFrenquencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task RegistroFrequenciaAluno(string codigoAluno, int numeroAula, TipoFrequencia valorFrequencia)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID_1,
                Valor = (int)valorFrequencia,
                NumeroAula = numeroAula,
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriaCompensacaoAusenciaAluno(string codigoAluno, int quantidadeCompensada)
        {
            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = codigoAluno,
                CompensacaoAusenciaId = COMPENSACAO_AUSENCIA_ID_1,
                QuantidadeFaltasCompensadas = quantidadeCompensada,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarCompensacaoAusenciaAlunoAula(List<CompensacaoAusenciaAluno> compensacaoAusenciaAlunos, List<RegistroFrequenciaAluno> registroFrequenciaAlunos)
        {
            registroFrequenciaAlunos = registroFrequenciaAlunos.Where(t => t.Valor == (int)TipoFrequencia.F).ToList();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciaAlunos.Where(t => t.CompensacaoAusenciaId == COMPENSACAO_AUSENCIA_ID_1))
            {
                foreach(var registroFrequencia in registroFrequenciaAlunos.Where(t => t.CodigoAluno == compensacaoAusenciaAluno.CodigoAluno).Take(compensacaoAusenciaAluno.QuantidadeFaltasCompensadas))
                {
                    await InserirNaBase(new CompensacaoAusenciaAlunoAula
                    {
                        CompensacaoAusenciaAlunoId = compensacaoAusenciaAluno.Id,
                        RegistroFrequenciaAlunoId = registroFrequencia.Id,
                        NumeroAula = registroFrequencia.NumeroAula,
                        DataAula = DateTimeExtension.HorarioBrasilia(),
                        CriadoEm = DateTimeExtension.HorarioBrasilia(),
                        CriadoPor = SISTEMA_NOME,
                        CriadoRF = SISTEMA_CODIGO_RF
                    });
                }
            }
        }

        protected class CompensacaoDeAusenciaDBDto
        {
            public CompensacaoDeAusenciaDBDto()
            {
                CriarPeriodoEscolar = true;
                TipoCalendarioId = TIPO_CALENDARIO_1;
                CriarPeriodoAbertura = true;
                PermiteCompensacaoForaPeriodoAtivo = true;
            }

            public DateTime? DataReferencia { get; set; }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public int Bimestre { get; set; }
            public string ComponenteCurricular { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public bool CriarPeriodoAbertura { get; set; }
            public string AnoTurma { get; set; }
            public int QuantidadeAula { get; set; }
            public bool AulaCj { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }

            public bool PermiteCompensacaoForaPeriodoAtivo { get; set; }
        }

        private async Task CriarTurmaTipoCalendario(CompensacaoDeAusenciaDBDto dtoDB)
        {
            await CriarTipoCalendario(dtoDB.TipoCalendario, dtoDB.ConsiderarAnoAnterior);
            await CriarTurma(dtoDB.Modalidade, dtoDB.AnoTurma, dtoDB.ConsiderarAnoAnterior);
        }

        private async Task CriarPeriodoEscolar(bool considerarAnoAnterior)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_28_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        private async Task CriarAbrangencia(string perfil)
        {
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = new Guid(perfil),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });
        }

        private async Task CriarParametrosSistema(bool considerarAnoAnterior, bool ativo)
        {
            await InserirNaBase(new ParametrosSistema
            {
                Nome = "PermiteCompensacaoForaPeriodo",
                Tipo = TipoParametroSistema.PermiteCompensacaoForaPeriodo,
                Descricao = "Permite compensação fora do periodo",
                Valor = string.Empty,
                Ano = considerarAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Ativo = ativo
            });
        }

        protected async Task CriarPeriodoReaberturaAnoAnterior(long tipoCalendarioId)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DATA_01_01_ANO_ANTERIOR,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        protected CompensacaoDeAusenciaDBDto ObterDtoDadoBase(string perfil, string componente)
        {
            return new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_3,
                ComponenteCurricular = componente,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = ANO_5,
                DataReferencia = DATA_25_07_INICIO_BIMESTRE_3,
                QuantidadeAula = QUANTIDADE_AULA_4
            };
        }
    }
}