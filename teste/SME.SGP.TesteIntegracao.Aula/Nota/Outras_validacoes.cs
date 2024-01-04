using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
using Xunit;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class Outras_validacoes : NotaTesteBase
    {
        public Outras_validacoes(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ServicosFakes.ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDisciplinasEolQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDisciplinasEolQueryHandlerFakePortugues), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
        }

        //[Fact]
        public async Task Deve_apresentar_registro_de_auditoria()
        {
            var filtroNota = ObterFiltroNotas(ObterPerfilProfessor(), TipoNota.Nota, ANO_7);

            await CriarEstruturaBaseDeNota(filtroNota);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_1, NOTA_6);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_1, NOTA_7);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_1, NOTA_9);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_1, NOTA_8);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_1, NOTA_1);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_2, NOTA_5);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_2, NOTA_4);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_2, NOTA_3);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_2, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_2, NOTA_10);

            var notaconceito = ObterNotaNumericaPersistencia(filtroNota);

            var listaNotaConceito = ObterNotaConceitoListar(filtroNota);

            await ExecutarNotasConceito(notaconceito, listaNotaConceito, false);
        }

        //[Fact]
        public async Task Deve_notificar_notas_com_maioria_alunos_abaixo_minimo_nota_conceito()
        {
            var filtroNota = ObterFiltroNotas(ObterPerfilProfessor(), TipoNota.Conceito, ANO_1);

            await CriarEstruturaBaseDeNota(filtroNota);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_1, null, (int)ConceitoValores.NS);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_1, null, (int)ConceitoValores.S);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_1, null, (int)ConceitoValores.P);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_1, null, (int)ConceitoValores.NS);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_1, null, (int)ConceitoValores.S);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_2, null, (int)ConceitoValores.NS);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_2, null, (int)ConceitoValores.S);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_2, null, (int)ConceitoValores.P);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_2, null, (int)ConceitoValores.NS);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_2, null, (int)ConceitoValores.S);

            var notaconceito = ObterNotaConceitoPersistencia(filtroNota);

            var listaNotaConceito = ObterNotaConceitoListar(filtroNota);

            await ExecutarNotasConceito(notaconceito, listaNotaConceito, false);
            var notificacoes = ObterTodos<Notificacao>();

            notificacoes.ShouldNotBeNull();
            notificacoes.Any(a => a.Tipo == NotificacaoTipo.Notas).ShouldBeTrue();
        }

        //[Fact]
        public async Task Deve_notificar_notas_com_maioria_alunos_abaixo_minimo_nota_numerica()
        {
            var filtroNota = ObterFiltroNotas(ObterPerfilProfessor(), TipoNota.Nota, ANO_7);

            await CriarEstruturaBaseDeNota(filtroNota);


            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_1, NOTA_4);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_1, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_1, NOTA_3);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_1, NOTA_1);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_1, NOTA_1);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_2, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_2, NOTA_4);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_2, NOTA_3);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_2, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_2, NOTA_1);

            var notaconceito = ObterNotaNumericaAbaixoDaMediaPersistencia(filtroNota);
            var listaNotaConceito = ObterNotaConceitoListar(filtroNota);

            await ExecutarNotasConceito(notaconceito, listaNotaConceito, false);
            var notificacoes = ObterTodos<Notificacao>();

            notificacoes.ShouldNotBeNull();
            notificacoes.Any(a => a.Tipo == NotificacaoTipo.Notas).ShouldBeTrue();
        }

        //[Fact]
        public async Task Deve_notificar_notas_exteporanea_nota_numerica()
        {
            var filtroNota = ObterFiltroNotas(ObterPerfilProfessor(), TipoNota.Nota, ANO_7);

            filtroNota.CriarPeriodoAbertura = false;
            filtroNota.CriarPeriodoEscolar = false;

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            filtroNota.DataReferencia = dataAtual.AddDays(-15);

            await CriarEstruturaBaseDeNota(filtroNota);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataAtual.AddDays(-20), dataAtual.AddDays(-10), BIMESTRE_1);
            await CriarPeriodoEscolar(dataAtual.AddDays(10), dataAtual.AddDays(20), BIMESTRE_2);
            await CriarPeriodoEscolar(dataAtual.AddDays(25), dataAtual.AddDays(35), BIMESTRE_3);
            await CriarPeriodoEscolar(dataAtual.AddDays(45), dataAtual.AddDays(70), BIMESTRE_4);



            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_1, NOTA_4);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_1, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_1, NOTA_3);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_1, NOTA_1);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_1, NOTA_1);

            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_1, ATIVIDADE_AVALIATIVA_2, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_2, ATIVIDADE_AVALIATIVA_2, NOTA_4);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_3, ATIVIDADE_AVALIATIVA_2, NOTA_3);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_4, ATIVIDADE_AVALIATIVA_2, NOTA_2);
            await CriarNotaConceitoNaBase(filtroNota, ALUNO_CODIGO_5, ATIVIDADE_AVALIATIVA_2, NOTA_1);

            var notaconceito = ObterNotaNumericaPersistencia(filtroNota);
            var listaNotaConceito = ObterNotaConceitoListar(filtroNota, dataAtual.AddDays(-20), dataAtual.AddDays(-10));

            await ExecutarNotasConceito(notaconceito, listaNotaConceito, false);
            var notificacoes = ObterTodos<Notificacao>();

            notificacoes.ShouldNotBeNull();
            notificacoes.Any(a => a.Tipo == NotificacaoTipo.Notas).ShouldBeTrue();
        }


        private async Task CriarNotaConceitoNaBase(FiltroNotasDto filtroNota, string alunoCodigo, long atividadeAvaliativaId, double? nota = null, long? conceitoId = null)
        {
            await InserirNaBase(new NotaConceito()
            {
                AlunoId = alunoCodigo,
                AtividadeAvaliativaID = atividadeAvaliativaId,
                Nota = nota,
                ConceitoId = conceitoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                DisciplinaId = filtroNota.ComponenteCurricular,
                TipoNota = filtroNota.TipoNota
            });
        }

        private ListaNotasConceitosDto ObterNotaConceitoListar(FiltroNotasDto filtroNota)
        {
            return new ListaNotasConceitosDto()
            {
                TurmaId = long.Parse(TURMA_CODIGO_1),
                AnoLetivo = filtroNota.ConsiderarAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : DateTimeExtension.HorarioBrasilia().Year,
                Bimestre = BIMESTRE_1,
                DisciplinaCodigo = long.Parse(filtroNota.ComponenteCurricular),
                Modalidade = filtroNota.Modalidade,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                Semestre = SEMESTRE_1,
                TurmaCodigo = TURMA_CODIGO_1,
                TurmaHistorico = false,
                PeriodoInicioTicks = filtroNota.ConsiderarAnoAnterior ? DATA_01_01_INICIO_BIMESTRE_1.AddYears(-1).Ticks : DATA_01_01_INICIO_BIMESTRE_1.Ticks,
                PeriodoFimTicks = filtroNota.ConsiderarAnoAnterior ? DATA_01_05_FIM_BIMESTRE_1.AddYears(-1).Ticks : DATA_01_05_FIM_BIMESTRE_1.Ticks,
            };
        }
        private ListaNotasConceitosDto ObterNotaConceitoListar(FiltroNotasDto filtroNota, DateTime periodoInicio, DateTime periodoFim)
        {
            return new ListaNotasConceitosDto()
            {
                TurmaId = long.Parse(TURMA_CODIGO_1),
                AnoLetivo = filtroNota.ConsiderarAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : DateTimeExtension.HorarioBrasilia().Year,
                Bimestre = BIMESTRE_1,
                DisciplinaCodigo = long.Parse(filtroNota.ComponenteCurricular),
                Modalidade = filtroNota.Modalidade,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                Semestre = SEMESTRE_1,
                TurmaCodigo = TURMA_CODIGO_1,
                TurmaHistorico = false,
                PeriodoInicioTicks = periodoInicio.Ticks,
                PeriodoFimTicks = periodoFim.Ticks,
            };
        }

        private NotaConceitoListaDto ObterNotaNumericaPersistencia(FiltroNotasDto filtroNota)
        {
            return new NotaConceitoListaDto()
            {
                DisciplinaId = filtroNota.ComponenteCurricular,
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_10},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_9},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_8},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_7},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_6},

                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_9},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_10},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_7},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_8},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_5},
                }
            };
        }

        private NotaConceitoListaDto ObterNotaNumericaAbaixoDaMediaPersistencia(FiltroNotasDto filtroNota)
        {
            return new NotaConceitoListaDto()
            {
                DisciplinaId = filtroNota.ComponenteCurricular,
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_1},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_2},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_3},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_4},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Nota = NOTA_1},

                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_1},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_1},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_2},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_3},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Nota = NOTA_4},
                }
            };
        }

        private NotaConceitoListaDto ObterNotaConceitoPersistencia(FiltroNotasDto filtroNota)
        {
            return new NotaConceitoListaDto()
            {
                DisciplinaId = filtroNota.ComponenteCurricular,
                TurmaId = TURMA_CODIGO_1,
                NotasConceitos = new List<NotaConceitoDto>()
                {
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Conceito = (int)ConceitoValores.P},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Conceito = (int)ConceitoValores.NS},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Conceito = (int)ConceitoValores.S},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Conceito = (int)ConceitoValores.P},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_1, Conceito = (int)ConceitoValores.NS},

                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_1, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Conceito = (int)ConceitoValores.S},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_2, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Conceito = (int)ConceitoValores.P},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_3, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Conceito = (int)ConceitoValores.NS},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_4, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Conceito = (int)ConceitoValores.S},
                    new NotaConceitoDto() { AlunoId = ALUNO_CODIGO_5, AtividadeAvaliativaId = ATIVIDADE_AVALIATIVA_2, Conceito = (int)ConceitoValores.P},
                }
            };
        }

        private async Task CriarEstruturaBaseDeNota(FiltroNotasDto filtroNota)
        {
            await CriarDadosBase(filtroNota);

            var dataAula = filtroNota.DataReferencia.HasValue ? filtroNota.DataReferencia.Value : filtroNota.ConsiderarAnoAnterior ? DATA_03_01.AddYears(-1) : DATA_03_01;

            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1, filtroNota.ProfessorRf);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            await CriarAtividadeAvaliativa(dataAula, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, false, false, filtroNota.ProfessorRf);

            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, filtroNota.ComponenteCurricular);

            dataAula = filtroNota.DataReferencia.HasValue ? filtroNota.DataReferencia.Value.AddDays(1) : filtroNota.ConsiderarAnoAnterior ? DATA_04_01.AddYears(-1) : DATA_04_01;

            await CriarAula(filtroNota.ComponenteCurricular, dataAula, RecorrenciaAula.AulaUnica, NUMERO_AULA_1, filtroNota.ProfessorRf);

            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_2);

            await CriarAtividadeAvaliativa(dataAula, TIPO_AVALIACAO_CODIGO_2, AVALIACAO_NOME_2, false, false, filtroNota.ProfessorRf);

            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_2, filtroNota.ComponenteCurricular);
        }

        private FiltroNotasDto ObterFiltroNotas(string perfil, TipoNota tipoNota, string anoTurma, bool considerarAnoAnterior = false)
        {
            return new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoCalendarioId = TIPO_CALENDARIO_1,
                CriarPeriodoEscolar = true,
                CriarPeriodoAbertura = true,
                TipoNota = tipoNota,
                AnoTurma = anoTurma,
                ConsiderarAnoAnterior = considerarAnoAnterior,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            };
        }
    }
}