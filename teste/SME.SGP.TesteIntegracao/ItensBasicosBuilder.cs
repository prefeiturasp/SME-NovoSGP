using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Setup
{
    public class ItensBasicosBuilder
    {
        private readonly TesteBase _teste;
        public ItensBasicosBuilder(TesteBase teste)
        {
            _teste = teste;
        }

        public async Task CriaItensComuns()
        {
            await _teste.InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE AB",
                Nome = "DRE AB"
            });
            await _teste.InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "Nome da UE",
            });

            await _teste.InserirNaBase(new PrioridadePerfil
            {
                Id = 1,
                CodigoPerfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                NomePerfil = "Professor",
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
            await _teste.InserirNaBase(new PrioridadePerfil
            {
                Id = 2,
                CodigoPerfil = new Guid("41e1e074-37d6-e911-abd6-f81654fe895d"),
                NomePerfil = "Professor CJ",
                Ordem = 320,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        public async Task CriaItensComunsEja()
        {
            CriarClaimRegenciaEja();

            await CriaItensComuns();
            await CriarPeriodoEscolar();

            await _teste.InserirNaBase(new Usuario
            {
                Id = 29,
                Login = "6926886",
                CodigoRf = "6926886",
                Nome = "ESTER CUSTODIA DOS SANTOS",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await _teste.InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = Modalidade.EJA,
                AnoLetivo = 2022,
                Semestre = 2,
                Nome = "Turma Nome 1"
            });

            await _teste.InserirNaBase(new TipoCalendario
            {
                AnoLetivo = 2022,
                Nome = "Ano Letivo 202",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.EJA,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false
            });

            await _teste.InserirNaBase(new Aula
            {
                UeId = "1",
                DisciplinaId = "1114",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "6926886",
                Quantidade = 1,
                DataAula = new DateTime(2022, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(2022, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });

            await _teste.InserirNaBase(new Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = new Guid("41e1e074-37d6-e911-abd6-f81654fe895d")
            });
        }

        public async Task CriaAulaSemFrequencia()
        {
            await _teste.InserirNaBase(new Aula
            {
                UeId = "1",
                DisciplinaId = "1106",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "6926886",
                Quantidade = 1,
                DataAula = new DateTime(2022, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(2022, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });


        }

        public async Task CriaComponenteCurricularSemFrequencia()
        {
            await _teste.InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await _teste.InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await _teste.InserirNaBase("componente_curricular", "1106", "1106", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");
        }

        public async Task CriaComponenteCurricularComFrequencia()
        {
            await _teste.InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await _teste.InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await _teste.InserirNaBase("componente_curricular", "1", "0", "1", "1", "'INGLES'", "false", "false", "false", "false", "true", "true", "'Inglês'", "''");
        }

        public async Task CriaNotaParametro()
        {
            await _teste.InserirNaBase(new NotaParametro
            {
                Ativo = true,
                FimVigencia = DateTime.Today.AddDays(2),
                Incremento = 0.5,
                InicioVigencia = new DateTime(2021, 02, 10),
                Maxima = 10,
                Media = 5,
                Minima = 0,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        public async Task CriaTipoEventoBimestral()
        {
            await _teste.InserirNaBase(new EventoTipo
            {
                Id = 1,
                Codigo = (int)TipoEvento.FechamentoBimestre,
                Ativo = true,
                Descricao = "Tipo Evento",
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        public async Task CriaCiclo()
        {
            await _teste.InserirNaBase(new Ciclo { Id = 1, Descricao = "Alfabetização", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 2, Descricao = "Interdisciplinar", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 3, Descricao = "Autoral", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 4, Descricao = "Médio", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 5, Descricao = "EJA - Alfabetização", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 6, Descricao = "EJA - Básica", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 7, Descricao = "EJA - Complementar", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await _teste.InserirNaBase(new Ciclo { Id = 8, Descricao = "EJA - Final", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });


            await _teste.InserirNaBase(new CicloAno { Id = 1, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "1" });
            await _teste.InserirNaBase(new CicloAno { Id = 2, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "2" });
            await _teste.InserirNaBase(new CicloAno { Id = 3, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "3" });
            await _teste.InserirNaBase(new CicloAno { Id = 4, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "4" });
            await _teste.InserirNaBase(new CicloAno { Id = 5, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "5" });
            await _teste.InserirNaBase(new CicloAno { Id = 6, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "6" });
            await _teste.InserirNaBase(new CicloAno { Id = 7, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "7" });
            await _teste.InserirNaBase(new CicloAno { Id = 8, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "8" });
            await _teste.InserirNaBase(new CicloAno { Id = 9, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "9" });
            await _teste.InserirNaBase(new CicloAno { Id = 10, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "1" });
            await _teste.InserirNaBase(new CicloAno { Id = 11, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "2" });
            await _teste.InserirNaBase(new CicloAno { Id = 12, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "3" });
            await _teste.InserirNaBase(new CicloAno { Id = 13, CicloId = 5, Modalidade = Modalidade.EJA, Ano = "1" });
            await _teste.InserirNaBase(new CicloAno { Id = 14, CicloId = 6, Modalidade = Modalidade.EJA, Ano = "2" });
            await _teste.InserirNaBase(new CicloAno { Id = 15, CicloId = 7, Modalidade = Modalidade.EJA, Ano = "3" });
            await _teste.InserirNaBase(new CicloAno { Id = 16, CicloId = 8, Modalidade = Modalidade.EJA, Ano = "4" });
            await _teste.InserirNaBase(new CicloAno { Id = 17, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "4" });
        }

        public async Task CriaAvaliacaoBimestral()
        {
            await _teste.InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                CriadoEm = new DateTime(2019, 12, 19),
                Situacao = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral
            });
        }

        public async Task CriarParametroSistema()
        {
            await _teste.InserirNaBase(new ParametrosSistema
            {
                Nome = "MediaBimestre",
                Tipo = TipoParametroSistema.MediaBimestre,
                Descricao = "Media final para aprovacão no bimestre",
                Valor = "5",
                Ano = 2022,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Ativo = true
            });
            await _teste.InserirNaBase(new ParametrosSistema
            {
                Nome = "PercentualAlunosInsuficientes",
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Descricao = "Media final para aprovacão no bimestre",
                Valor = "50",
                Ano = 2022,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Ativo = true
            });
        }

        public async Task CriaComponenteCurricularJurema()
        {
            await _teste.InserirNaBase(new ComponenteCurricularJurema()
            {
                CodigoJurema = 1,
                DescricaoEOL = "Arte",
                CodigoEOL = 1,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        public async Task CriarPeriodoEscolar()
        {
            await _teste.InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(2022, 01, 10),
                PeriodoFim = DateTime.Now.AddYears(1),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        private void CriarClaimRegenciaEja()
        {
            var contextoAplicacao = _teste.ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "ESTER CUSTODIA DOS SANTOS");
            variaveis.Add("UsuarioLogado", "6926886");
            variaveis.Add("RF", "6926886");
            variaveis.Add("login", "6926886");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "6926886", Type = "rf" },
                new InternalClaim { Value = "41e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
    }
}
