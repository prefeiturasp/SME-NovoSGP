using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.FechamentoTurmaBimestreListagem.Base
{
    /// <summary>
    /// Base para os testes de caracterização dos N+1 de AEE e Conceito em
    /// ListarFechamentoTurmaBimestreUseCase.RetornaListagemAlunosFechamentoBimestreEspecifico.
    /// Os testes chamam esse método público diretamente (em vez de Executar), controlando os alunos
    /// em memória e evitando toda a máquina de EOL/tipo_calendario/período escolar não relevante
    /// para os dois N+1 investigados.
    /// </summary>
    public abstract class FechamentoTurmaBimestreListagemTesteBase : TesteBase
    {
        protected const long TURMA_ID = 1;
        protected const string CODIGO_TURMA = "1234";
        protected const long DISCIPLINA_ID = 1;
        protected const long FECHAMENTO_TURMA_ID = 1;
        protected const long FECHAMENTO_TURMA_DISCIPLINA_ID = 1;

        protected FechamentoTurmaBimestreListagemTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task<Dominio.Turma> CarregarTurma()
        {
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

            var dre = new Dre { Id = 1, Nome = "Dre Teste", CodigoDre = "11", Abreviacao = "DT" };
            var ue = new Ue { Id = 1, Nome = "Ue Teste", DreId = 1, TipoEscola = TipoEscola.EMEF, CodigoUe = "22", Dre = dre };

            await InserirNaBase(dre);
            await InserirNaBase(ue);

            var turma = new Dominio.Turma
            {
                Id = TURMA_ID,
                Nome = "1A",
                CodigoTurma = CODIGO_TURMA,
                Ano = "1",
                AnoLetivo = anoLetivo,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1,
                // TurmaEmPeriodoFechamentoQueryHandler acessa turma.Ue.Dre.CodigoDre diretamente —
                // sem essa navegação populada em memória, dispara NullReferenceException.
                Ue = ue
            };

            await InserirNaBase(turma);

            // ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery (chamada dentro do use case)
            // resolve o tipo_calendario da turma por ano_letivo+modalidade — sem essa linha,
            // ObterPorTurma retorna null e o handler quebra em tipoCalendario.Id.
            await InserirNaBase(new Dominio.TipoCalendario
            {
                Id = 1,
                AnoLetivo = anoLetivo,
                Nome = "Calendário Teste",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            return turma;
        }

        protected AlunoPorTurmaResposta CriarAlunoAtivo(string codigoAluno, int numeroChamada)
        {
            var inicioAno = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1);

            return new AlunoPorTurmaResposta
            {
                CodigoAluno = codigoAluno,
                NomeAluno = $"Aluno {codigoAluno}",
                NumeroAlunoChamada = numeroChamada,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = inicioAno,
                DataSituacao = inicioAno,
                DataNascimento = new DateTime(2015, 1, 1)
            };
        }

        /// <summary>
        /// Monta a estrutura fechamento_turma/fechamento_turma_disciplina/fechamento_aluno no banco
        /// (necessária só quando o teste precisa que ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery
        /// encontre notas reais, ex.: teste de Conceito). Retorna o FechamentoTurmaDisciplina em memória,
        /// já com FechamentoAlunos populado, pronto para compor ListagemAlunosFechamentoDto.FechamentosTurma.
        /// </summary>
        protected async Task<FechamentoTurmaDisciplina> CarregarFechamento(IEnumerable<string> codigosAlunos)
        {
            await InserirNaBase(new FechamentoTurma
            {
                Id = FECHAMENTO_TURMA_ID,
                TurmaId = TURMA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                Id = FECHAMENTO_TURMA_DISCIPLINA_ID,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID,
                DisciplinaId = DISCIPLINA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = FECHAMENTO_TURMA_DISCIPLINA_ID,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID,
                DisciplinaId = DISCIPLINA_ID
            };

            long fechamentoAlunoId = 1;
            foreach (var codigoAluno in codigosAlunos)
            {
                await InserirNaBase(new FechamentoAluno
                {
                    Id = fechamentoAlunoId,
                    FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID,
                    AlunoCodigo = codigoAluno,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = "sistema",
                    CriadoRF = "0000000"
                });

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(new FechamentoAluno
                {
                    Id = fechamentoAlunoId,
                    FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID,
                    AlunoCodigo = codigoAluno
                });

                fechamentoAlunoId++;
            }

            return fechamentoTurmaDisciplina;
        }

        protected Usuario CriarUsuarioProfessor()
        {
            var usuario = new Usuario
            {
                Id = 1,
                CodigoRf = "7111111",
                Login = "7111111",
                Nome = "Usuario Teste",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1)
            };

            usuario.DefinirPerfis(new List<PrioridadePerfil>
            {
                new PrioridadePerfil { CodigoPerfil = Perfis.PERFIL_PROFESSOR }
            });

            return usuario;
        }

        protected DisciplinaDto CriarComponenteCurricular()
            => new DisciplinaDto
            {
                Id = DISCIPLINA_ID,
                CodigoComponenteCurricular = DISCIPLINA_ID,
                Nome = "Componente Teste",
                Regencia = false,
                LancaNota = true
            };

        protected PeriodoEscolar CriarPeriodoEscolar(int bimestre = 1)
        {
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

            return new PeriodoEscolar
            {
                Id = 1,
                Bimestre = bimestre,
                PeriodoInicio = new DateTime(anoLetivo, 1, 1),
                PeriodoFim = new DateTime(anoLetivo, 3, 31)
            };
        }
    }
}
