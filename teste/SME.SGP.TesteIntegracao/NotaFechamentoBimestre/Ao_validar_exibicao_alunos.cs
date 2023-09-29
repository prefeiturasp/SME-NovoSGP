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
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_validar_exibicao_alunos : NotaFechamentoBimestreTesteBase
    {
        private const long FECHAMENTO_TURMA_ID_1 = 1;

        private const long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;

        private const long FECHAMENTO_ALUNO_ID_1 = 1;
        private const long FECHAMENTO_ALUNO_ID_2 = 2;
        private const long FECHAMENTO_ALUNO_ID_3 = 3;
        private const long FECHAMENTO_ALUNO_ID_4 = 4;
        private const long FECHAMENTO_ALUNO_ID_5 = 5;

        public Ao_validar_exibicao_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>),
                typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), 
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_exibir_tooltip_alunos_novos_durante_15_dias()
        {
            var filtroNotaFechamento = await ObterFiltroFechamentoNota(ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolarId = periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.Id;
            var dataInicioTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoInicio.Ticks;
            var dataFimTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoFim.Ticks;            
            
            var filtroListaNotasConceitos = await ObterFiltroListaNotasConceitos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                filtroNotaFechamento.Modalidade, dataInicioTicks, dataFimTicks, periodoEscolarId);

            var retorno = await ExecutarTeste(filtroListaNotasConceitos);

            retorno.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_1)?
                .Alunos.Where(c => c.Marcador.NaoEhNulo())
                .Count(c => c.Marcador.Tipo == TipoMarcadorFrequencia.Novo).ShouldBe(2);
        }

        [Fact]
        public async Task Deve_exibir_tooltip_alunos_inativos_ate_data_sua_inativacao()
        {
            var filtroNotaFechamento = await ObterFiltroFechamentoNota(ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolarId = periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.Id;
            var dataInicioTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoInicio.Ticks;
            var dataFimTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoFim.Ticks;            
            
            var filtroListaNotasConceitos = await ObterFiltroListaNotasConceitos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                filtroNotaFechamento.Modalidade, dataInicioTicks, dataFimTicks, periodoEscolarId);

            var retorno = await ExecutarTeste(filtroListaNotasConceitos);

            retorno.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_1)?
                .Alunos.Where(c => c.Marcador.NaoEhNulo())
                .Count(c => c.Marcador.Tipo == TipoMarcadorFrequencia.Inativo).ShouldBe(7);
        }
        
        [Fact]
        public async Task Nao_deve_exibir_alunos_inativos_antes_do_comeco_do_ano_ou_bimestre()
        {
            var filtroNotaFechamento = await ObterFiltroFechamentoNota(ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());  
            
            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolarId = periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.Id;
            var dataInicioTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoInicio.Ticks;
            var dataFimTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_1))!.PeriodoFim.Ticks;            
            
            var filtroListaNotasConceitos = await ObterFiltroListaNotasConceitos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                filtroNotaFechamento.Modalidade, dataInicioTicks, dataFimTicks, periodoEscolarId);

            var retorno = await ExecutarTeste(filtroListaNotasConceitos);

            var alunosNaoExibir = new[] { CODIGO_ALUNO_12, CODIGO_ALUNO_13 };

            retorno.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_1)?
                .Alunos.Any(c => !alunosNaoExibir.Contains(c.Id)).ShouldBeTrue();
        }
        
        private async Task<NotasConceitosRetornoDto> ExecutarTeste(ListaNotasConceitosDto filtroListaNotasConceitos)
        {
            NotasConceitosRetornoDto retorno = new();
            var useCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();
            
            if (useCase.NaoEhNulo()) 
                retorno = await useCase.Executar(filtroListaNotasConceitos);
            
            await InserirFechamentoAluno(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);            

            var notasFechamento = ObterTodos<FechamentoTurmaDisciplina>();

            notasFechamento.ShouldNotBeNull();
            notasFechamento.ShouldNotBeEmpty();
            notasFechamento.Count.ShouldBeGreaterThanOrEqualTo(1);

            return await Task.FromResult(retorno);
        }

        private async Task InserirPeriodoEscolarCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();
            
            await CriarPeriodoEscolar(dataReferencia.AddDays(-45), dataReferencia.AddDays(+30), BIMESTRE_1);
            await CriarPeriodoEscolar(dataReferencia.AddDays(40), dataReferencia.AddDays(115), BIMESTRE_2);
            await CriarPeriodoEscolar(dataReferencia.AddDays(125), dataReferencia.AddDays(200), BIMESTRE_3);
            await CriarPeriodoEscolar(dataReferencia.AddDays(210), dataReferencia.AddDays(285), BIMESTRE_4);
        }

        private async Task InserirFechamentoAluno(long disciplinaId)
        {   
            await InserirNaBase(new FechamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        
            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                DisciplinaId = disciplinaId,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var fechamentosAlunosNotas = new List<(string CodigoAluno, long IdFechamentoAluno)>
            {
                new(CODIGO_ALUNO_1, FECHAMENTO_ALUNO_ID_1),
                new(CODIGO_ALUNO_2, FECHAMENTO_ALUNO_ID_2),
                new(CODIGO_ALUNO_3, FECHAMENTO_ALUNO_ID_3),
                new(CODIGO_ALUNO_4, FECHAMENTO_ALUNO_ID_4),
                new(CODIGO_ALUNO_5, FECHAMENTO_ALUNO_ID_5)
            };

            foreach (var fechamentoAlunoNota in fechamentosAlunosNotas)
            {
                await InserirNaBase(new FechamentoAluno()
                {
                    FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                    AlunoCodigo = fechamentoAlunoNota.CodigoAluno,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
                
                Random randomNota = new();
                var nota = randomNota.Next(0, 10);
                
                await InserirNaBase(new FechamentoNota()
                {
                    DisciplinaId = disciplinaId,
                    FechamentoAlunoId = fechamentoAlunoNota.IdFechamentoAluno,
                    Nota = nota,
                    CriadoEm = DateTime.Now,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });                
            }
        }
        
        private async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota(ModalidadeTipoCalendario tipoCalendario, 
            Modalidade modalidade, string anoTurma, TipoFrequenciaAluno tipoFrequenciaAluno,
            string componenteCurricular)
        {
            return await Task.FromResult(new FiltroFechamentoNotaDto
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = tipoCalendario,
                ConsiderarAnoAnterior = false,
                Modalidade = modalidade,
                AnoTurma = anoTurma,
                TipoFrequenciaAluno = tipoFrequenciaAluno,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = componenteCurricular,
                CriarPeriodoEscolar = false,
                CriarPeriodoEscolarCustomizado = false
            });
        }        

        private static async Task<ListaNotasConceitosDto> ObterFiltroListaNotasConceitos(long disciplinaCodigo,
            Modalidade modalidade, long periodoInicioTicks, long periodoFimTicks, long periodoEscolarId)
        {
            return await Task.FromResult(new ListaNotasConceitosDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Bimestre = BIMESTRE_1,
                DisciplinaCodigo = disciplinaCodigo,
                Modalidade = modalidade,
                Semestre = 0,
                TurmaCodigo = TURMA_CODIGO_1,
                TurmaHistorico = false, 
                TurmaId = TURMA_ID_1,
                PeriodoInicioTicks = periodoInicioTicks,
                PeriodoFimTicks = periodoFimTicks,
                PeriodoEscolarId = periodoEscolarId
            });
        }
    }
}