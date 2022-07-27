using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_ausencia_bimestre_encerrado : CompensacaoDeAusenciaTesteBase
    {
        private const int QUANTIDADE_AULAS_22 = 22;
        public Ao_lancar_compensacao_ausencia_bimestre_encerrado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        // TODO: Comentado devido ao problema de inclusão (Bulk Insert)
        //[Fact]
        public async Task Deve_lancar_compensacao_ausencia_bimestre_encerrado_sem_reabertura()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                true,
                true);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            await comando.Inserir(compensacaoAusenciaDosAlunos);
            
            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();
            
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull(); 
            
            var compensacaoAusenciasAlunos = await ObterCompensacaoAusenciasAlunos();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == compensacaoAusenciaAluno.Item1);
                compensacaoAluno.ShouldNotBeNull();

                compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(compensacaoAusenciaAluno.Item2);                
            }            
        }

        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_ano_anterior_sem_reabertura_periodo()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                false,
                true);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            async Task DoExecutarInserir()
            {
                await comando.Inserir(compensacaoAusenciaDosAlunos);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarInserir);
        }
        
        // TODO: Comentado devido ao problema de inclusão (Bulk Insert)
        //[Fact]
        public async Task Deve_lancar_compensacao_ausencia_ano_anterior_com_reabertura_periodo()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                true,
                true,
                true);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            await comando.Inserir(compensacaoAusenciaDosAlunos);
            
            var listaDeCompensacaoAusencia = ObterTodos<CompensacaoAusencia>();
            listaDeCompensacaoAusencia.ShouldNotBeNull();

            var listaDeCompensacaoAusenciaAluno = ObterTodos<CompensacaoAusenciaAluno>();
            listaDeCompensacaoAusenciaAluno.ShouldNotBeNull();
            
            var compensacao = listaDeCompensacaoAusencia.FirstOrDefault();
            compensacao.ShouldNotBeNull();
            
            var listaDaCompensacaoAluno = listaDeCompensacaoAusenciaAluno.FindAll(aluno => aluno.CompensacaoAusenciaId == compensacao.Id);
            listaDaCompensacaoAluno.ShouldNotBeNull(); 
            
            var compensacaoAusenciasAlunos = await ObterCompensacaoAusenciasAlunos();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                var compensacaoAluno = listaDaCompensacaoAluno.Find(aluno => aluno.CodigoAluno == compensacaoAusenciaAluno.Item1);
                compensacaoAluno.ShouldNotBeNull();

                compensacaoAluno.QuantidadeFaltasCompensadas.ShouldBe(compensacaoAusenciaAluno.Item2);                
            }
        }
        
        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_componente_que_nao_lanca_frequencia()
        {
            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                BIMESTRE_1,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(),
                ANO_7,
                QUANTIDADE_AULAS_22,
                true,
                false,
                true,
                false);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            comando.ShouldNotBeNull();            
            
            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(compensacaoDeAusencia);

            async Task DoExecutarInserir()
            {
                await comando.Inserir(compensacaoAusenciaDosAlunos);
            }

            await Should.ThrowAsync<NegocioException>(DoExecutarInserir);
        }

        private static async Task<List<Tuple<string, int>>> ObterCompensacaoAusenciasAlunos()
        {
            //-> Item1 = Código do aluno
            //   Item2 = Quantidade de aulas
            return await Task.FromResult(new List<Tuple<string, int>>
            {
                new(CODIGO_ALUNO_1, QUANTIDADE_AULA),
                new(CODIGO_ALUNO_2, QUANTIDADE_AULA_2),
                new(CODIGO_ALUNO_3, QUANTIDADE_AULA_3),
                new(CODIGO_ALUNO_4, QUANTIDADE_AULA_4)
            });
        }

        private async Task<CompensacaoAusenciaDto> LancarCompensacaoAusenciasAlunos(CompensacaoDeAusenciaDBDto compensacaoDeAusencia)
        {
            await CriarDadosBase(compensacaoDeAusencia);
            await CriarFrequenciasAlunos(compensacaoDeAusencia.Bimestre, compensacaoDeAusencia.ComponenteCurricular);
            await CriarRegistroFrequencia();

            var compensacaoAusenciasAlunos = await ObterCompensacaoAusenciasAlunos();

            List<CompensacaoAusenciaAlunoDto> alunos = new();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                alunos.Add(new CompensacaoAusenciaAlunoDto
                {
                    Id = compensacaoAusenciaAluno.Item1,
                    QtdFaltasCompensadas = compensacaoAusenciaAluno.Item2
                });
            };
            
            return await Task.FromResult(new CompensacaoAusenciaDto
            {
                TurmaId = TURMA_CODIGO_1,
                Alunos = alunos,
                Bimestre = compensacaoDeAusencia.Bimestre,
                Atividade = ATIVIDADE_COMPENSACAO,
                Descricao = DESCRICAO_COMPENSACAO,
                DisciplinaId = compensacaoDeAusencia.ComponenteCurricular,
                DisciplinasRegenciaIds = null
            });
        }

        private async Task CriarRegistroFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            //-> Item1 = Código do aluno
            //   Item2 = Quantidade de aulas
            //   Item3 = Tipo da frequência
            var registrosFrequenciasAlunos = new List<Tuple<string, int, TipoFrequencia>>
            {
                new(CODIGO_ALUNO_1, QUANTIDADE_AULA_4, TipoFrequencia.F),
                new(CODIGO_ALUNO_2, QUANTIDADE_AULA_4, TipoFrequencia.C),
                new(CODIGO_ALUNO_3, QUANTIDADE_AULA_4, TipoFrequencia.R),
                new(CODIGO_ALUNO_4, QUANTIDADE_AULA_4, TipoFrequencia.F)
            };

            foreach (var registroFrequenciaAluno in registrosFrequenciasAlunos)
            {
                await CriarRegistrosFrequenciasAlunos(registroFrequenciaAluno.Item1, registroFrequenciaAluno.Item2,
                    registroFrequenciaAluno.Item3);
            }
        }

        private async Task CriarRegistrosFrequenciasAlunos(string codigoAluno, int numeroAula,
            TipoFrequencia tipoFrequencia)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID_1,
                Valor = (int)tipoFrequencia,
                NumeroAula = numeroAula,
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });            
        }

        private async Task CriarFrequenciasAlunos(int bimestre, string disciplinaId)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolar = periodosEscolares.FirstOrDefault(c => c.Bimestre == bimestre);
            
            if (periodoEscolar == null)
                return;

            const int totalAulas = 22;

            // -> Item1 = Código aluno
            //    Item2 = Total de ausências
            //    Item3 = Total de compensações
            //    Item4 = Total de presenças
            //    Item5 = Total remotos
            var frequenciasAlunos = new List<Tuple<string, int, int, int, int>>
            {
                new(CODIGO_ALUNO_1, TOTAL_AUSENCIAS_1, TOTAL_COMPENSACOES_1, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_2, TOTAL_AUSENCIAS_3, TOTAL_COMPENSACOES_3, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_3, TOTAL_AUSENCIAS_7, TOTAL_COMPENSACOES_7, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_4, TOTAL_AUSENCIAS_8, TOTAL_COMPENSACOES_8, totalAulas, TOTAL_REMOTOS_0)
            };

            foreach (var frequenciaAluno in frequenciasAlunos)
            {
                await InserirNaBase(new Dominio.FrequenciaAluno
                {
                    PeriodoInicio = periodoEscolar.PeriodoInicio,
                    PeriodoFim = periodoEscolar.PeriodoFim,
                    Bimestre = bimestre,
                    TotalAulas = totalAulas,
                    TotalAusencias = frequenciaAluno.Item2,
                    TotalCompensacoes = frequenciaAluno.Item3,
                    PeriodoEscolarId = periodoEscolar.Id,
                    TotalPresencas = frequenciaAluno.Item4,
                    TotalRemotos = frequenciaAluno.Item5,
                    DisciplinaId = disciplinaId,
                    CodigoAluno = frequenciaAluno.Item1,
                    TurmaId = TURMA_CODIGO_1,
                    Tipo = TipoFrequenciaAluno.PorDisciplina,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }
        
        private static async Task<CompensacaoDeAusenciaDBDto> ObterCompensacaoDeAusencia(string perfil, int bimestre,
            Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular,
            string anoTurma, int quantidadeAulas, bool criarPeriodoEscolar, bool criarPeriodoAbertura, 
            bool permiteCompensacaoForaPeriodoAtivo, bool considerarAnoAnterior)
        {
            return await Task.FromResult(new CompensacaoDeAusenciaDBDto
            {
                Perfil = perfil,
                Bimestre = bimestre,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = anoTurma,
                QuantidadeAula = quantidadeAulas,
                CriarPeriodoEscolar = criarPeriodoEscolar,
                CriarPeriodoAbertura = criarPeriodoAbertura,
                PermiteCompensacaoForaPeriodoAtivo = permiteCompensacaoForaPeriodoAtivo,
                ConsiderarAnoAnterior = considerarAnoAnterior
            });
        }        
    }
}