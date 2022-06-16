using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public abstract class AulaTeste : TesteBase
    {
        protected AulaTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task ExecuteTesteRegistre(bool ehRegente = false)
        {
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = ObtenhaDtoAula();
            if (ehRegente) dto.EhRegencia = true;

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        protected async Task CarregueBase(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, bool criaPeriodo = true)
        {
            await CriaItensComuns(criaPeriodo);
            CriarClaimUsuario(perfil);
            await CriaUsuario();
            await CriaTipoCalendario(tipoCalendario);
            await CriaTurma(modalidade);
        }

        protected void CriarClaimUsuario(string perfil)
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "João Usuário");
            variaveis.Add("UsuarioLogado", "2222222");
            variaveis.Add("RF", "2222222");
            variaveis.Add("login", "2222222");

            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "2222222", Type = "rf" },
                new InternalClaim { Value = perfil, Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        protected string ObtenhaPerfilEspecialista()
        {
            return "40e1e074-37d6-e911-abd6-f81654fe895d";
        }

        protected string ObtenhaPerfilCJ()
        {
            return "41e1e074-37d6-e911-abd6-f81654fe895d";
        }

        protected PersistirAulaDto ObtenhaDtoAula()
        {
            return new PersistirAulaDto()
            {
                CodigoTurma = "1",
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                DataAula = new DateTime(2022, 02, 10),
                DisciplinaCompartilhadaId = 1106,
                CodigoUe = "1",
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                CodigoComponenteCurricular = 1106,
                NomeComponenteCurricular = "português",
                TipoCalendarioId = 1
            };
        }

        protected async Task CriarPeriodoEscolarEncerrado()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(2022, 01, 10),
                PeriodoFim = new DateTime(2022, 02, 5),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CrieEvento(EventoLetivo letivo)
        {
            await InserirNaBase(new EventoTipo
            {
                Descricao = "festa",
                Ativo = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Evento
            {
                Nome = "festa",
                TipoCalendarioId = 1,
                TipoEventoId = 1,
                UeId = "1",
                Letivo = letivo,
                DreId = "1",
                DataInicio = new DateTime(2022, 02, 10),
                DataFim = new DateTime(2022, 02, 10),
                Status = EntidadeStatus.Aprovado,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CrieAtribuicaoEsporadica()
        {
            await InserirNaBase(new AtribuicaoEsporadica
            {
                UeId = "1",
                ProfessorRf = "2222222",
                AnoLetivo = 2022,
                DreId = "1",
                DataInicio = new DateTime(2022, 01, 10),
                DataFim = new DateTime(2022, 01, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriaAtribuicaoCJ()
        {
            await InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = "1",
                DreId = "1",
                UeId = "1",
                ProfessorRf = "2222222",
                DisciplinaId = 1106,
                Modalidade = Modalidade.EducacaoInfantil,
                Substituir = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriaAula(string rf = "2222222")
        {
            await InserirNaBase(new Aula
            {
                UeId = "1",
                DisciplinaId = "1106",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = 3,
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

        protected async Task CriaUsuario()
        {
            await InserirNaBase(new Usuario
            {
                Id = 29,
                Login = "2222222",
                CodigoRf = "2222222",
                Nome = "João Usuário",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new Usuario
            {
                Login = "1111111",
                CodigoRf = "1111111",
                Nome = "Maria Usuário",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private async Task CriaTurma(Modalidade modalidade)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = 2022,
                Semestre = 2,
                Nome = "Turma Nome 1"
            });
        }

        private async Task CriaTipoCalendario(ModalidadeTipoCalendario tipoCalendario)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = 2022,
                Nome = "Ano Letivo 202",
                Periodo = Periodo.Semestral,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false
            });
        }

        private async Task CriaItensComuns(bool criaPeriodo)
        {
            await CriaPadrao();
            if (criaPeriodo) await CriarPeriodoEscolar();
            await CriaComponenteCurricular();
        }

        public async Task CriaPadrao()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE AB",
                Nome = "DRE AB"
            });
            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "Nome da UE",
            });

            await InserirNaBase(new PrioridadePerfil
            {
                Id = 1,
                CodigoPerfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                NomePerfil = "Professor",
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });

            await InserirNaBase(new PrioridadePerfil
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

        private async Task CriarPeriodoEscolar()
        {
            await InserirNaBase(new PeriodoEscolar
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


        private async Task CriaComponenteCurricular()
        {
            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "1106", "1106", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");
        }
    }
}
