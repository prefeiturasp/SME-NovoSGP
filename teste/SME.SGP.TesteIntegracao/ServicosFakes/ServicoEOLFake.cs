using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoMatriz = SME.SGP.Dominio.GrupoMatriz;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoEOLFake 
    {
        private readonly string ALUNO_CODIGO_1 = "1";
        private readonly string ALUNO_CODIGO_2 = "2";
        private readonly string ALUNO_CODIGO_3 = "3";
        private readonly string ALUNO_CODIGO_4 = "4";
        private readonly string ALUNO_CODIGO_5 = "5";
        private readonly string ALUNO_CODIGO_6 = "6";
        private readonly string ALUNO_CODIGO_7 = "7";
        private readonly string ALUNO_CODIGO_8 = "8";
        private readonly string ALUNO_CODIGO_9 = "9";
        private readonly string ALUNO_CODIGO_10 = "10";

        private readonly string ATIVO = "Ativo";
        private readonly string RESPONSAVEL = "RESPONSAVEL";
        private readonly string TIPO_RESPONSAVEL_4 = "4";
        private readonly string CELULAR_RESPONSAVEL = "11111111111";

        private const string ESCOLA_CODIGO_1 = "1";
        private const string ESCOLA_CODIGO_2 = "2";
        private const string ANO_7 = "7";
        private const int CODIGO_TURMA_1 = 1;
        private const int CODIGO_TURMA_2 = 2;
        private const string NOME_TURMA_1 = "1A";
        private const string NOME_TURMA_2 = "2A";
        private const int CODIGO_MODALIDADE = 5;
        private const string DRE_CODIGO_1 = "1";
        private const string DRE_NOME_1 = "NOME DRE 1";
        private const string DRE_ABREVIACAO_1 = "DRE-1";
        private const string MODALIDADE_FUNDAMENTAL = "FUNDAMENTAL";
        private const int SEMESTRE_0 = 0;
        private const string UNIDADE_ADMINISTRATIVA = "UNIDADE ADMINISTRATIVA";
        private const int UE_CODIGO_TIPO_3 = 3;
        private const string UE_NOME_1 = "NOME UE 1";
        private const string UE_ABREVIACAO_1 = "UE-1";
        private const string TIPO_ESCOLA_CEU_EMEF = "CEU EMEF";
        private const string TIPO_ESCOLA_CODIGO_16 = "16";

       public async Task<AutenticacaoApiEolDto> ObtenhaAutenticacaoSemSenha(string login)
        {
            return new AutenticacaoApiEolDto()
            {
                CodigoRf = login
            };
        }

        public async Task<AbrangenciaCompactaVigenteRetornoEOLDTO> ObterAbrangenciaCompactaVigente(string login, Guid perfil)
        {
            return new AbrangenciaCompactaVigenteRetornoEOLDTO
            {
                Login = login,
                Abrangencia = new AbrangenciaCargoRetornoEolDTO
                {
                    GrupoID = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                    CargosId = new List<int>
                    {
                        3239, 3255, 3263, 3271, 3280, 3298, 3301, 3336, 3344, 3840, 3859, 3867, 3874, 3883, 3884, 3310, 3131, 3212, 3213, 3220, 3247, 3395, 3425, 3433, 3450, 3816, 3875, 3877, 3880
                    },
                    Grupo = GruposSGP.Professor,
                    Abrangencia = Infra.Enumerados.Abrangencia.Professor,
                },
                IdTurmas = new List<string> { "2366531" }.ToArray(),
            };
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, bool consideraInativos = false)
        {
            var alunos = new List<AlunoPorTurmaResposta>();
            if (!consideraInativos)
            {
                alunos = new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "11223344",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                          CodigoTurma=int.Parse(turmaId),
                          DataNascimento=new DateTime(1959,01,16,00,00,00),
                          DataSituacao= new DateTime(2021,11,09,17,25,31),
                          DataMatricula= new DateTime(2021,11,09,17,25,31),
                          EscolaTransferencia=null,
                          NomeAluno="Maria Aluno teste",
                          NomeSocialAluno=null,
                          NumeroAlunoChamada=1,
                          ParecerConclusivo=null,
                          PossuiDeficiencia=false,
                          SituacaoMatricula="Ativo",
                          Transferencia_Interna=false,
                          TurmaEscola=null,
                          TurmaRemanejamento=null,
                          TurmaTransferencia=null,
                          NomeResponsavel="João teste",
                          TipoResponsavel="4",
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    },
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "6523614",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                          CodigoTurma=int.Parse(turmaId),
                          DataNascimento=new DateTime(1959,01,16,00,00,00),
                          DataSituacao= new DateTime(2021,11,09,17,25,31),
                          DataMatricula= new DateTime(2021,11,09,17,25,31),
                          EscolaTransferencia=null,
                          NomeAluno="ANA RITA ANDRADE FERREIRA DOS SANTOS",
                          NomeSocialAluno=null,
                          NumeroAlunoChamada=1,
                          ParecerConclusivo=null,
                          PossuiDeficiencia=false,
                          SituacaoMatricula="Ativo",
                          Transferencia_Interna=false,
                          TurmaEscola=null,
                          TurmaRemanejamento=null,
                          TurmaTransferencia=null,
                          NomeResponsavel="ANA RITA ANDRADE FERREIRA DOS SANTOS,",
                          TipoResponsavel="4",
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    }
                };
            }
            else
            {
                alunos = new List<AlunoPorTurmaResposta>
                {
                    new AlunoPorTurmaResposta
                    {
                          Ano=0,
                          CodigoAluno = "6523616",
                          CodigoComponenteCurricular=0,
                          CodigoSituacaoMatricula= SituacaoMatriculaAluno.Desistente,
                          CodigoTurma=int.Parse(turmaId),
                          DataNascimento=new DateTime(1959,01,16,00,00,00),
                          DataSituacao= new DateTime(2021,11,09,17,25,31),
                          DataMatricula= new DateTime(2021,11,09,17,25,31),
                          EscolaTransferencia=null,
                          NomeAluno="ANA RITA ANDRADE FERREIRA DOS SANTOS",
                          NomeSocialAluno=null,
                          NumeroAlunoChamada=1,
                          ParecerConclusivo=null,
                          PossuiDeficiencia=false,
                          SituacaoMatricula="Desistente",
                          Transferencia_Interna=false,
                          TurmaEscola=null,
                          TurmaRemanejamento=null,
                          TurmaTransferencia=null,
                          NomeResponsavel="ANA RITA ANDRADE FERREIRA DOS SANTOS,",
                          TipoResponsavel="4",
                          CelularResponsavel="11961861993",
                          DataAtualizacaoContato= new DateTime(2018,06,22,19,02,35),
                    }
                };
            }
            return alunos.Where(x => x.CodigoTurma.ToString() == turmaId);
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, string codigoAluno, bool consideraInativos = false)
        {
            var alunos = new List<AlunoPorTurmaResposta>
            {
                   new AlunoPorTurmaResposta
                    {
                      Ano = 0,
                      CodigoAluno = "11223344",
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    },
               new AlunoPorTurmaResposta
                    {
                      Ano = 0,
                      CodigoAluno = "6523614",
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    },
               new AlunoPorTurmaResposta
                    {
                      Ano = 0,
                      CodigoAluno = "666666",
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                    },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_1,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_1,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_2,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_2,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_3,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_3,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_4,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_4,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_5,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_5,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_6,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_6,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_7,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_7,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_8,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_8,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_9,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_9,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                },
                new AlunoPorTurmaResposta
                {
                      Ano = 0,
                      CodigoAluno = ALUNO_CODIGO_10,
                      CodigoComponenteCurricular = 0,
                      CodigoSituacaoMatricula= SituacaoMatriculaAluno.Ativo,
                      CodigoTurma=int.Parse(turmaId),
                      DataNascimento=new DateTime(1959,01,16,00,00,00),
                      DataSituacao= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      DataMatricula= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                      NomeAluno= ALUNO_CODIGO_10,
                      NumeroAlunoChamada=1,
                      SituacaoMatricula= ATIVO,
                      NomeResponsavel= RESPONSAVEL,
                      TipoResponsavel= TIPO_RESPONSAVEL_4,
                      CelularResponsavel=CELULAR_RESPONSAVEL,
                      DataAtualizacaoContato= new DateTime(DateTimeExtension.HorarioBrasilia().Year,01,01),
                }
            };
            return alunos.Where(x => x.CodigoTurma.ToString() == turmaId && x.CodigoAluno == codigoAluno);
        }

        public async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil, bool realizarAgrupamentoComponente = false, bool checaMotivoDisponibilizacao = true)
        {
            return await Task.FromResult(new List<ComponenteCurricularEol>()
            {
                new ComponenteCurricularEol()
                {
                    Codigo = 1106,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 2, Nome = "Diversificada" },
                    LancaNota = false            
                },
                new ComponenteCurricularEol()
                {
                    Codigo = 138,
                    TerritorioSaber = false,
                    GrupoMatriz = new GrupoMatriz() { Id = 1, Nome = "Base Nacional Comum" },
                    LancaNota = true
                }
            });
        }
        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true, bool tipoTurma = true)
        {
            if (codidoAluno.Equals("77777"))
                return ObterAlunosPorTurma("1", true);
            if (codidoAluno.Equals("666666"))
                return ObterAlunosPorTurma("1", codidoAluno, true);
            return ObterAlunosPorTurma("1");
        }

        public async Task<DadosTurmaEolDto> ObterDadosTurmaPorCodigo(string codigoTurma)
        {
            return new DadosTurmaEolDto
            {
                Ano = '\u0000',
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Codigo = 2366531,
                CodigoModalidade = 0,
                DataFim = null,
                DataInicioTurma = DateTime.Now,
                DuracaoTurno = 7,
                Ehistorico = false,
                EnsinoEspecial = false,
                EtapaEJA = 0,
                Extinta = false,
                Modalidade = null,
                NomeTurma = "2A",
                Semestre = 0,
                SerieEnsino = null,
                TipoTurma = TipoTurma.Regular,
                TipoTurno = 5,
            };
        }

        public Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAgrupadas(long[] ids, string codigoTurma = null)
        {
            return Task.FromResult(new List<DisciplinaDto>
            {
                new DisciplinaDto
                {
                    Id = 1217,
                    CodigoComponenteCurricular = 1217,
                    GrupoMatrizId = 4,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 1",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 1",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 138,
                    CodigoComponenteCurricular = 138,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 2",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 6,
                    CodigoComponenteCurricular = 6,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 3",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 2,
                    CodigoComponenteCurricular = 2,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 4",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 7,
                    CodigoComponenteCurricular = 7,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 5",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"

                },
                new DisciplinaDto
                {
                    Id = 139,
                    CodigoComponenteCurricular = 139,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 6",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1105,
                    CodigoComponenteCurricular = 1105,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 7",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1114,
                    CodigoComponenteCurricular = 1114,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 8",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1061,
                    CodigoComponenteCurricular = 1061,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 9",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = true,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = true,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 2",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 512,
                    CodigoComponenteCurricular = 512,
                    GrupoMatrizId = 1,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Regência de Classe Infantil",
                    NomeComponenteInfantil = "REGÊNCIA INFANTIL EMEI 4H",
                    Regencia = true,
                    RegistraFrequencia = true,
                    GrupoMatrizNome = "Base Nacional Comum",
                    TurmaCodigo = "1"
                },
                new DisciplinaDto
                {
                    Id = 1214,
                    CodigoComponenteCurricular = 1214,
                    GrupoMatrizId = 4,
                    CdComponenteCurricularPai = null,
                    Compartilhada = false,
                    Nome = "Teste 11",
                    NomeComponenteInfantil = null,
                    PossuiObjetivos = false,
                    Regencia = false,
                    RegistraFrequencia = false,
                    TerritorioSaber = false,
                    LancaNota = false,
                    ObjetivosAprendizagemOpcionais = false,
                    GrupoMatrizNome = "Teste 11",
                    TurmaCodigo = "1"
                },
            }.Where(x => ids.Contains(x.Id)));
        }

        // public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId)
        // {
        //     return new List<UsuarioEolRetornoDto>{
        //         new UsuarioEolRetornoDto
        //         {
        //             CodigoRf="9988776",
        //             NomeServidor = "UsuarioTeste1",
        //             CodigoFuncaoAtividade = 0,
        //             EstaAfastado = false
        //         },
        //         new UsuarioEolRetornoDto
        //         {
        //             CodigoRf="7788990",
        //             NomeServidor = "UsuarioTeste2",
        //             CodigoFuncaoAtividade = 0,
        //             EstaAfastado = false
        //         },
        //     };
        // }
        
        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorPerfilDre(Guid perfil, string codigoDre)
        {
            return new List<UsuarioEolRetornoDto>{
                new UsuarioEolRetornoDto
                {
                    CodigoRf="1",
                    NomeServidor = "ALEXANDRE AFRANIO HOKAMA SILVA",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false,
                    UsuarioId = 1
                },
                new UsuarioEolRetornoDto
                {
                    CodigoRf="2",
                    NomeServidor = "FILIPE EMMANUEL ADOLPHO ECARD",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false,
                    UsuarioId = 2
                },
            };
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterListaNomePorListaRF(IEnumerable<string> codigosRF)
        {
            return new List<ProfessorResumoDto>()
            {
                new ProfessorResumoDto(){
                    CodigoRF = "11223344",
                    Nome = "Maria Aluno teste"
                }
            };
        }

        public Task<IEnumerable<ProfessorTurmaReposta>> ObterListaTurmasPorProfessor(string codigoRf)
        {
            return  Task.FromResult<IEnumerable<ProfessorTurmaReposta>>(new List<ProfessorTurmaReposta>()
            {
                new ProfessorTurmaReposta(){
                    CodEscola = ESCOLA_CODIGO_1,
                    Ano = ANO_7,
                    CodTurma = CODIGO_TURMA_1,
                    NomeTurma = NOME_TURMA_1,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodModalidade = CODIGO_MODALIDADE,
                    CodDre = DRE_CODIGO_1,
                    Dre = DRE_NOME_1,
                    DreAbrev = DRE_ABREVIACAO_1,
                    Modalidade = MODALIDADE_FUNDAMENTAL,
                    Semestre = SEMESTRE_0,
                    TipoUE = UNIDADE_ADMINISTRATIVA,
                    CodTipoUE = UE_CODIGO_TIPO_3,
                    Ue = UE_NOME_1,
                    UeAbrev = UE_ABREVIACAO_1,
                    TipoEscola = TIPO_ESCOLA_CEU_EMEF,
                    CodTipoEscola = TIPO_ESCOLA_CODIGO_16,
                },
                new ProfessorTurmaReposta(){
                    CodEscola = ESCOLA_CODIGO_1,
                    Ano = ANO_7,
                    CodTurma = CODIGO_TURMA_2,
                    NomeTurma = NOME_TURMA_2,
                    AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                    CodModalidade = CODIGO_MODALIDADE,
                    CodDre = DRE_CODIGO_1,
                    Dre = DRE_NOME_1,
                    DreAbrev = DRE_ABREVIACAO_1,
                    Modalidade = MODALIDADE_FUNDAMENTAL,
                    Semestre = SEMESTRE_0,
                    TipoUE = UNIDADE_ADMINISTRATIVA,
                    CodTipoUE = UE_CODIGO_TIPO_3,
                    Ue = UE_NOME_1,
                    UeAbrev = UE_ABREVIACAO_1,
                    TipoEscola = TIPO_ESCOLA_CEU_EMEF,
                    CodTipoEscola = TIPO_ESCOLA_CODIGO_16,
                }
            });
        }

        public async Task<MeusDadosDto> ObterMeusDados(string login)
        {
            return new MeusDadosDto()
            {
                Nome = "João Usuário",
                Email = String.Empty
            };
        }

        public async Task<RetornoDadosAcessoUsuarioSgpDto> CarregarDadosAcessoPorLoginPerfil(string login, Guid perfilGuid, AdministradorSuporteDto administradorSuporte = null)
        {
            return new RetornoDadosAcessoUsuarioSgpDto()
            {
                Permissoes = new List<int>() { 1 },
                Token = "",
                DataExpiracaoToken = DateTime.Now
            };
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesDisciplinas(string turmaCodigo, DateTime? dataReferencia = null, string professorRf = null, bool realizaAgrupamento = true)
        {
            return new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="",
                    ProfessorNome ="Não há professor titular.",
                    DisciplinaNome = "INFORMATICA - OIE",
                    DisciplinasId = new long[] { 1060 }
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="6118232",
                    ProfessorNome ="MARLEI LUCIANE BERNUN",
                    DisciplinaNome = "LEITURA - OSL",
                    DisciplinasId =new long[] { 1061 }
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "2222222",
                    ProfessorNome = "João Usuário",
                    DisciplinaNome = "REG CLASSE EJA ETAPA BASICA",
                    DisciplinasId = new long[] { 1114 }
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "6737544",
                    ProfessorNome = "GENILDO CLEBER DA SILVA",
                    DisciplinaNome = "Disciplina Fundamental",
                    DisciplinasId = new long[] { 1114 }
                },
            };
        }

        public Task<IEnumerable<SupervisoresRetornoDto>> ObterSupervisoresPorCodigo(string[] codigoSupervisores)
        {
            return Task.FromResult<IEnumerable<SupervisoresRetornoDto>>( new List<SupervisoresRetornoDto>()
            {
                new SupervisoresRetornoDto()
                {
                    CodigoRf = "1",
                    NomeServidor = "Teste da silva"
                }
            });
        }
        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterUsuarioFuncionario(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto)
        {
            return new List<UsuarioEolRetornoDto>()
            {
                new UsuarioEolRetornoDto()
                {
                    CodigoRf = "2222222"
                }
            };
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data)
        {
            if (disciplinaId == "139")
                return false;
            return true;
        }
    }
}