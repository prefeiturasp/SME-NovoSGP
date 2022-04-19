using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
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
            await CriarItensBasicos();
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
                TurmaId = "2366531",
                NotasConceitos = listaDeNotas
            };

            //Act
            var controller = new NotasConceitosController();
            var retorno = await controller.Post(dto, command);

            //Assert
            retorno.ShouldNotBeNull();
            Assert.IsType<OkResult>(retorno);
        }


        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1
            });

            await InserirNaBase(new Turma
            {
                Id = 2366531,
                UeId = 1,
                Ano = "2",
                CodigoTurma = "2366531"
            });

            await InserirNaBase(new PrioridadePerfil
            {
                Id = 29,
                CodigoPerfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d"),
                NomePerfil = "Professor",
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
            await InserirNaBase(new PrioridadePerfil
            {
                Id = 32,
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
                TurmaId = "2366531",
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
                TurmaId = 2366531,
                Historico = false
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
