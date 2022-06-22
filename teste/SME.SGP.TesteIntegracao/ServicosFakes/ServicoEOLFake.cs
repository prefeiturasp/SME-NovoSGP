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

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoEOLFake : IServicoEol
    {
        public Task AlterarEmail(string login, string email)
        {
            throw new NotImplementedException();
        }

        public Task<AlterarSenhaRespostaDto> AlterarSenha(string login, string novaSenha)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirCJSeNecessario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirCJSeNecessario(string codigoRf)
        {
            throw new NotImplementedException();
        }

        public Task AtribuirPerfil(string codigoRf, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<AutenticacaoApiEolDto> Autenticar(string login, string senha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CicloRetornoDto> BuscarCiclos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoEscolaRetornoDto> BuscarTiposEscola()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> DefinirTurmasRegulares(string[] codigosTurmas)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteUsuarioComMesmoEmail(string login, string email)
        {
            throw new NotImplementedException();
        }

        public Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> ListagemTurmasComComponente(string codigoUe, string modalidade, int semestre, string codigoTurma, int anoLetivo, int qtdeRegistos, int qtdeRegistrosIgnorados)
        {
            throw new NotImplementedException();
        }

        public async Task<AutenticacaoApiEolDto> ObtenhaAutenticacaoSemSenha(string login)
        {
            return new AutenticacaoApiEolDto()
            {
                CodigoRf = "7924488"
            };
        }

        public Task<AbrangenciaRetornoEolDto> ObterAbrangencia(string login, Guid perfil)
        {
            throw new NotImplementedException();
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

        public Task<AbrangenciaRetornoEolDto> ObterAbrangenciaParaSupervisor(string[] uesIds)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ObterAdministradoresSGP(string codigoDreOuUe)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ObterAdministradoresSGPParaNotificar(string codigoDreOuUe)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosAtivosPorTurma(string codigoTurma, DateTime dataAula)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string turmaId, bool consideraInativos = false)
        {
            var alunos = new List<AlunoPorTurmaResposta> {
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

            return alunos.Where(x => x.CodigoTurma.ToString() == turmaId);
        }

        public Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesCurriculares()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorAnosEModalidade(string codigoUe, Modalidade modalidade, string[] anosEscolares, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil, bool realizarAgrupamentoComponente = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamento(string codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesPorLoginEIdPerfil(string login, Guid idPerfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesTurmasProgramaPorAnoEModalidade(string codigoUe, Modalidade modalidade, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesRegenciaPorAno(int anoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true)
        {
            return ObterAlunosPorTurma("1");
        }

        public async Task<DadosTurmaEolDto> ObterDadosTurmaPorCodigo(string codigoTurma)
        {
            return new DadosTurmaEolDto
            {
                Ano = '\u0000',
                AnoLetivo = 2022,
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

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasParaPlanejamento(long codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurma(string codigoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorCodigoTurmaLoginEPerfil(string codigoTurma, string login, Guid perfil)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsAgrupadas(long[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIdsSemAgrupamento(long[] ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DreRespostaEolDto> ObterDres()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId)
        {
            throw new NotImplementedException();
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorDre()
        {
            throw new NotImplementedException();
        }

        public EstruturaInstitucionalRetornoEolDTO ObterEstruturaInstuticionalVigentePorTurma(string[] codigosTurma = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UsuarioEolRetornoDto> ObterFuncionariosPorCargoUe(string ueId, long cargoId)
        {
            return new List<UsuarioEolRetornoDto>{
                new UsuarioEolRetornoDto
                {
                    CodigoRf="9988776",
                    NomeServidor = "UsuarioTeste1",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
                new UsuarioEolRetornoDto
                {
                    CodigoRf="7788990",
                    NomeServidor = "UsuarioTeste2",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
            };
        }

        public async Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorCargoUeAsync(string ueId, long cargoId)
        {
            return new List<UsuarioEolRetornoDto>{
                new UsuarioEolRetornoDto
                {
                    CodigoRf="9988776",
                    NomeServidor = "UsuarioTeste1",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
                new UsuarioEolRetornoDto
                {
                    CodigoRf="7788990",
                    NomeServidor = "UsuarioTeste2",
                    CodigoFuncaoAtividade = 0,
                    EstaAfastado = false
                },
            };
        }

        public Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorDre(Guid perfil, FiltroFuncionarioDto filtroFuncionariosDto)
        {
            throw new NotImplementedException();
        }

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

        public Task<IEnumerable<UsuarioEolRetornoDto>> ObterFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FuncionarioUnidadeDto>> ObterListaNomePorListaLogin(IEnumerable<string> logins)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<ProfessorResumoDto>> ObterListaResumosPorListaRF(IEnumerable<string> codigosRF, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf)
        {
            throw new NotImplementedException();
        }

        public async Task<MeusDadosDto> ObterMeusDados(string login)
        {
            return new MeusDadosDto()
            {
                Nome = "João Usuário",
                Email = String.Empty
            };
        }

        public Task<InformacoesEscolaresAlunoDto> ObterNecessidadesEspeciaisAluno(string codigoAluno)
        {
            throw new NotImplementedException();
        }

        public Task<string> ObterNomeProfessorPeloRF(string rfProfessor)
        {
            throw new NotImplementedException();
        }

        public async Task<PerfisApiEolDto> ObterPerfisPorLogin(string login)
        {
            var listaUsuarios = new List<PerfisApiEolDto>
            {
                new PerfisApiEolDto
                {
                    CodigoRf = "2222222",
                    Perfis = new List<Guid>
                    {
                        new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                        new Guid("41e1e074-37d6-e911-abd6-f81654fe895d"),
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "6737544",
                    Perfis = new List<Guid>
                    {
                        new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                        new Guid("41e1e074-37d6-e911-abd6-f81654fe895d"),
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "1111111",
                    Perfis = new List<Guid>
                    {
                        new Guid("44E1E074-37D6-E911-ABD6-F81654FE895D"),
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "8888888",
                    Perfis = new List<Guid>
                    {
                        new Guid("44E1E074-37D6-E911-ABD6-F81654FE895D"),
                    }
                },
            };
            return listaUsuarios.Where(x => x.CodigoRf == login).FirstOrDefault();
        }

        public Task<int[]> ObterPermissoesPorPerfil(Guid perfilGuid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorResumoDto>> ObterProfessoresAutoComplete(int anoLetivo, string dreId, string nomeProfessor, bool incluirEmei)
        {
            throw new NotImplementedException();
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
                    DisciplinaId = 1060
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf ="6118232",
                    ProfessorNome ="MARLEI LUCIANE BERNUN",
                    DisciplinaNome = "LEITURA - OSL",
                    DisciplinaId = 1061
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "2222222",
                    ProfessorNome = "João Usuário",
                    DisciplinaNome = "REG CLASSE EJA ETAPA BASICA",
                    DisciplinaId = 1114
                },
                new ProfessorTitularDisciplinaEol
                {
                    ProfessorRf = "6737544",
                    ProfessorNome = "GENILDO CLEBER DA SILVA",
                    DisciplinaNome = "Disciplina Fundamental",
                    DisciplinaId = 1114
                },
            };
        }

        public Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorTurmas(IEnumerable<string> codigosTurmas)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProfessorTitularDisciplinaEol>> ObterProfessoresTitularesPorUe(string ueCodigo, DateTime dataReferencia)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessorResumoDto> ObterProfessorPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioResumoCoreDto> ObterResumoCore(string login)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessorResumoDto> ObterResumoProfessorPorRFAnoLetivo(string codigoRF, int anoLetivo, bool buscarOutrosCargos = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoSupervisores)
        {
            return new List<SupervisoresRetornoDto>()
            {
                new SupervisoresRetornoDto()
                {
                    CodigoRf = "1",
                    NomeServidor = "Teste da silva"
                }
            };
        }

        public Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanoAnual(string codigoRf, long componenteCurricularId, int codigoTurma)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TurmaPorUEResposta>> ObterTurmasPorUE(string ueId, string anoLetivo)
        {
            throw new NotImplementedException();
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

        public Task<bool> PodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string professorRf, string codigoTurma, string disciplinaId, DateTime data)
        {

            return true;
        }

        public Task<bool> PodePersistirTurmaNoPeriodo(string professorRf, string codigoTurma, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProfessorPodePersistirTurma(string professorRf, string codigoTurma, DateTime data)
        {
            throw new NotImplementedException();
        }

        public Task ReiniciarSenha(string login)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioEolAutenticacaoRetornoDto> RelecionarUsuarioPerfis(string login)
        {
            throw new NotImplementedException();
        }

        public Task RemoverCJSeNecessario(Guid usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TurmaPossuiComponenteCurricularPAP(string codigoTurma, string login, Guid idPerfil)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidarProfessor(string professorRf)
        {
            throw new NotImplementedException();
        }

        public Task<AtribuicaoProfessorTurmaEOLDto> VerificaAtribuicaoProfessorTurma(string professorRf, string codigoTurma)
        {
            throw new NotImplementedException();
        }
    }
}