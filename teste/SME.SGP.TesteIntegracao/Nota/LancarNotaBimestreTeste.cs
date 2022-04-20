using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TipoAvaliacao = SME.SGP.Dominio.TipoAvaliacao;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class LancarNotaBimestreTeste : TesteBase
    {
        public LancarNotaBimestreTeste(TestFixture testFixture) : base(testFixture)
        {
        }

        [Fact]
        public async Task Lancar_Conceito_Para_Componentrse_Regencia_de_Classe_Eja()
        {
            // Arrange
            var command = ServiceProvider.GetService<IComandosNotasConceitos>();
            await CriarUsuarioLogadoEja();
            CriarClaimEja();
            await InserirDadosBasicosNoBanco();
            await CriarAulaProfEja();
            await CriarPeriodoEscolaEja();
            await CriarParametroSistema();
            var listaDeNotas = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "7128291",
                     AtividadeAvaliativaId = 1,
                     Conceito = 2,
                     Nota=8
                 },
            };
            var dto = new NotaConceitoListaDto
            {
                DisciplinaId = "1114",
                TurmaId = "1",
                NotasConceitos = listaDeNotas
            };

            //Act
            var controller = new NotasConceitosController();
            var retorno = await controller.Post(dto, command);

            //Assert
            retorno.ShouldNotBeNull();
            Assert.IsType<OkResult>(retorno);
        }

        private async Task CriarPeriodoEscolaEja()
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

        private async Task InserirDadosBasicosNoBanco()
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
                Nome  ="Nome da UE",
            });
            await InserirNaBase(new Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = Modalidade.EJA,
                AnoLetivo = 2022,
                Semestre = 2,
                Nome = "Turma Nome 1"
                
            });
            await InserirNaBase(new Turma
            {
                Id = 2,
                UeId = 1,
                Ano = "2",
                CodigoTurma = "2",
                Historica = true,
                ModalidadeCodigo = Modalidade.Fundamental,
                 Nome = "Turma Nome 2"
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
            await InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                CriadoEm = new DateTime(2019, 12, 19),
                Situacao = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,

            });
            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = "6926886",
                TurmaId = "1",
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                CriadoEm = new DateTime(2022, 02, 10),
                DataAvaliacao = new DateTime(2022, 02, 10),
                CriadoRF = "6926886",
                CriadoPor = "ESTER CUSTODIA DOS SANTOS"
            });
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = "1114",
                CriadoPor = "ESTER CUSTODIA DOS SANTOS",
                CriadoRF = "6926886"
            });
            await InserirNaBase(new Abrangencia
            {
                Id = 1,
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d")
            });

            #region Ciclo
            await InserirNaBase(new Ciclo { Id = 1, Descricao = "Alfabetização", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 2, Descricao = "Interdisciplinar", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 3, Descricao = "Autoral", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 4, Descricao = "Médio", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 5, Descricao = "EJA - Alfabetização", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 6, Descricao = "EJA - Básica", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 7, Descricao = "EJA - Complementar", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });
            await InserirNaBase(new Ciclo { Id = 8, Descricao = "EJA - Final", CriadoEm = DateTime.Now, CriadoPor = "Sistema", CriadoRF = "1" });


            await InserirNaBase(new CicloAno { Id = 1, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "1" });
            await InserirNaBase(new CicloAno { Id = 2, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "2" });
            await InserirNaBase(new CicloAno { Id = 3, CicloId = 1, Modalidade = Modalidade.Fundamental, Ano = "3" });
            await InserirNaBase(new CicloAno { Id = 4, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "4" });
            await InserirNaBase(new CicloAno { Id = 5, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "5" });
            await InserirNaBase(new CicloAno { Id = 6, CicloId = 2, Modalidade = Modalidade.Fundamental, Ano = "6" });
            await InserirNaBase(new CicloAno { Id = 7, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "7" });
            await InserirNaBase(new CicloAno { Id = 8, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "8" });
            await InserirNaBase(new CicloAno { Id = 9, CicloId = 3, Modalidade = Modalidade.Fundamental, Ano = "9" });
            await InserirNaBase(new CicloAno { Id = 10, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "1" });
            await InserirNaBase(new CicloAno { Id = 11, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "2" });
            await InserirNaBase(new CicloAno { Id = 12, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "3" });
            await InserirNaBase(new CicloAno { Id = 13, CicloId = 5, Modalidade = Modalidade.EJA, Ano = "1" });
            await InserirNaBase(new CicloAno { Id = 14, CicloId = 6, Modalidade = Modalidade.EJA, Ano = "2" });
            await InserirNaBase(new CicloAno { Id = 15, CicloId = 7, Modalidade = Modalidade.EJA, Ano = "3" });
            await InserirNaBase(new CicloAno { Id = 16, CicloId = 8, Modalidade = Modalidade.EJA, Ano = "4" });
            await InserirNaBase(new CicloAno { Id = 17, CicloId = 4, Modalidade = Modalidade.Medio, Ano = "4" });
            #endregion Ciclo

            #region NotasTipo e CicloParametos
            await InserirNaBase(new NotaTipoValor
            {
                TipoNota = TipoNota.Nota,
                Descricao = "Nota",
                InicioVigencia = new DateTime(2014, 01, 01),
                Ativo = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaTipoValor
            {
                TipoNota = TipoNota.Conceito,
                Descricao = "Conceito",
                InicioVigencia = new DateTime(2014, 01, 01),
                Ativo = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 1,
                TipoNotaId = (int)TipoNota.Conceito,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 2,
                TipoNotaId = (int)TipoNota.Nota,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 3,
                TipoNotaId = (int)TipoNota.Nota,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 4,
                TipoNotaId = (int)TipoNota.Nota,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 5,
                TipoNotaId = (int)TipoNota.Conceito,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 6,
                TipoNotaId = (int)TipoNota.Conceito,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 7,
                TipoNotaId = (int)TipoNota.Nota,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new NotaConceitoCicloParametro
            {
                CicloId = 8,
                TipoNotaId = (int)TipoNota.Nota,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = new DateTime(2014, 01, 01),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
            #endregion NotasTipo e CicloParametos

            #region Criar Tipo Calendario
            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
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
            #endregion Criar Tipo Calendario

        }

        private async Task CriarParametroSistema()
        {
            await InserirNaBase(new ParametrosSistema {
                Nome = "MediaBimestre",
                Tipo = TipoParametroSistema.MediaBimestre,
                Descricao = "Media final para aprovacão no bimestre",
                Valor = "5",
                Ano = 2022,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF= "1",
                Ativo = true
            });
            await InserirNaBase(new ParametrosSistema
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
        private async Task CriarAulaProfEja()
        {
            await InserirNaBase(new Aula
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
        }

        private void CriarClaimEja()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "ESTER CUSTODIA DOS SANTOS");
            variaveis.Add("UsuarioLogado", "6926886");
            variaveis.Add("RF", "6926886");
            variaveis.Add("login", "6926886");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "6926886", Type = "rf" },
                new InternalClaim { Value = "40e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", "ADEILMA CRISTINA DA SILVA DE SANTANA");
            variaveis.Add("UsuarioLogado", "8012229");
            variaveis.Add("RF", "8012229");
            variaveis.Add("login", "8012229");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "8012229", Type = "rf" },
                new InternalClaim { Value = "40e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
        private async Task CriarUsuarioLogadoEja()
        {
            await InserirNaBase(new Usuario
            {
                Id = 29,
                Login = "6926886",
                CodigoRf = "6926886",
                Nome = "ESTER CUSTODIA DOS SANTOS",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }
        private async Task CriarUsuarioLogadoFundamental()
        {
            await InserirNaBase(new Usuario
            {
                Id = 21623,
                Login = "8012229",
                CodigoRf = "8012229",
                Nome = "ADEILMA CRISTINA DA SILVA DE SANTANA",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }
    }
}
