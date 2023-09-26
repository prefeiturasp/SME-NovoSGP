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

        
        
        // public Task<IEnumerable<AlunoPorTurmaResposta>> ObterDadosAluno(string codidoAluno, int anoLetivo, bool consideraHistorico, bool filtrarSituacao = true, bool tipoTurma = true)
        // {
        //     if (codidoAluno.Equals("77777"))
        //         return ObterAlunosPorTurma("1", true);
        //     if (codidoAluno.Equals("666666"))
        //         return ObterAlunosPorTurma("1", codidoAluno, true);
        //     return ObterAlunosPorTurma("1");
        // }

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
    }
}