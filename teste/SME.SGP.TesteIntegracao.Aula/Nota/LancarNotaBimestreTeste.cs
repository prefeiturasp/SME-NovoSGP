using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
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
    public class LancarNotaBimestreTeste : TesteBaseComuns
    {
        public LancarNotaBimestreTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //[Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Diferente_Regencia_Fundamental()
        {
            // Arrange
            var command = ServiceProvider.GetService<IComandosNotasConceitos>();
            await CriarUsuarioLogadoFundamental();
            CriarClaimFundamental();
            await InserirDadosBasicosNoBanco();
            await CriarTurmaFundamental();
            await CriarAbrangenciaFundamental();
            await CriarTipoCalendarioFundamentalMedio();
            await CriarAulaProfFundamental();
            await CriarAtividadeAvaliativaFundamental();
            await CriarPeriodoEscolar();
            await CriarParametroSistema();
            var listaDeNotas = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "123123",
                     AtividadeAvaliativaId = 1,
                     Conceito = 2,
                     Nota=null
                 },
            };
            var dto = new NotaConceitoListaDto
            {
                DisciplinaId = "9",
                TurmaId = "1",
                NotasConceitos = listaDeNotas
            };
            //Act
            var controller = new NotasConceitosController();
            // TODO: Ajustar o teste
            //var retorno = await controller.Post(dto, command);

            //Assert
            //retorno.ShouldNotBeNull();
            //Assert.IsType<OkResult>(retorno);
        }
        //[Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Regencia_Eja()
        {
            // Arrange
            var command = ServiceProvider.GetService<IComandosNotasConceitos>();
            await CriarUsuarioLogadoRegenciaEja();
            CriarClaimRegenciaEja();
            await InserirDadosBasicosNoBanco();
            await CriarTurmaEja();
            await CriarAbrangenciaEja();
            await CriarTipoCalendarioEja();
            await CriarAulaProfRegenciaEja();
            await CriarAtividadeAvaliativaEja();
            await CriarPeriodoEscolar();
            await CriarParametroSistema();
            var listaDeNotas = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "123123",
                     AtividadeAvaliativaId = 1,
                     Conceito = 2,
                     Nota=null
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
            // TODO: Ajustar o teste
            //var retorno = await controller.Post(dto, command);

            //Assert
            //retorno.ShouldNotBeNull();
            //Assert.IsType<OkResult>(retorno);
        }

        #region Massa de Dados
        private async Task CriarTurmaFundamental()
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Fundamental,
                Nome = "Turma Nome 2",
                TipoTurno = 2
            });
        }
        private async Task CriarTurmaEja()
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                ModalidadeCodigo = Modalidade.EJA,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Semestre = 2,
                Nome = "Turma Nome 1",
                TipoTurno = 2
            });
        }

        private async Task CriarPeriodoEscolar()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
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
                Nome = "Nome da UE",
            });
            await InserirNaBase(new PrioridadePerfil
            {
                Id = 1,
                CodigoPerfil = Perfis.PERFIL_PROFESSOR,
                NomePerfil = "Professor",
                Ordem = 290,
                Tipo = TipoPerfil.UE,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
            await InserirNaBase(new PrioridadePerfil
            {
                Id = 2,
                CodigoPerfil = Perfis.PERFIL_CJ,
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
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-3).Year, 12, 19),
                Situacao = true,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,

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
        }

        private async Task CriarAtividadeAvaliativaEja()
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = USUARIO_LOGADO_RF,
                TurmaId = "1",
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                DataAvaliacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoRF = USUARIO_LOGADO_RF,
                CriadoPor = USUARIO_LOGADO_NOME
            });
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = "1114",
                CriadoPor = USUARIO_LOGADO_NOME,
                CriadoRF = USUARIO_LOGADO_RF
            });
        }
        private async Task CriarAtividadeAvaliativaFundamental()
        {
            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = USUARIO_LOGADO_RF,
                TurmaId = "1",
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                DataAvaliacao = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoRF = USUARIO_LOGADO_RF,
                CriadoPor = USUARIO_LOGADO_NOME
            });
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = "9",
                CriadoPor = USUARIO_LOGADO_NOME,
                CriadoRF = USUARIO_LOGADO_RF
            });
        }

        private async Task CriarTipoCalendarioEja() 
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Ano Letivo Ano Atual",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.EJA,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false
            });
        }
        private async Task CriarTipoCalendarioFundamentalMedio() 
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Nome = "Ano Letivo Ano Atual",
                Periodo = Periodo.Semestral,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false
            });
        }
        private async Task CriarAbrangenciaEja()
        {
            await InserirNaBase(new Dominio.Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = Perfis.PERFIL_PROFESSOR
            });
        }
        private async Task CriarAbrangenciaFundamental()
        {
            await InserirNaBase(new Dominio.Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = Perfis.PERFIL_PROFESSOR
            });
        }
        private async Task CriarParametroSistema()
        {
            await InserirNaBase(new ParametrosSistema {
                Nome = "MediaBimestre",
                Tipo = TipoParametroSistema.MediaBimestre,
                Descricao = "Media final para aprovacão no bimestre",
                Valor = "5",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
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
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Ativo = true
            });
        }
        private async Task CriarAulaProfRegenciaEja()
        {
            await InserirNaBase(new Dominio.Aula
            {
                UeId = "1",
                DisciplinaId = "1114",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_LOGADO_RF,
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });
        }

        private async Task CriarAulaProfFundamental()
        {
            await InserirNaBase(new Dominio.Aula
            {
                UeId = "1",
                DisciplinaId = "9",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = USUARIO_LOGADO_RF,
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });
        }
        private void CriarClaimRegenciaEja()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", USUARIO_LOGADO_NOME);
            variaveis.Add("UsuarioLogado", USUARIO_LOGADO_RF);
            variaveis.Add("RF", USUARIO_LOGADO_RF);
            variaveis.Add("login", USUARIO_LOGADO_RF);
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = USUARIO_LOGADO_RF, Type = "rf" },
                new InternalClaim { Value = Perfis.PERFIL_PROFESSOR.ToString(), Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
        private void CriarClaimFundamental()
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>();
            variaveis.Add("NomeUsuario", USUARIO_LOGADO_NOME);
            variaveis.Add("UsuarioLogado", USUARIO_LOGADO_RF);
            variaveis.Add("RF", USUARIO_LOGADO_RF);
            variaveis.Add("login", USUARIO_LOGADO_RF);
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = USUARIO_LOGADO_RF, Type = "rf" },
                new InternalClaim { Value = Perfis.PERFIL_PROFESSOR.ToString(), Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
        private async Task CriarUsuarioLogadoRegenciaEja()
        {
            await InserirNaBase(new Usuario
            {
                Id = 29,
                Login = USUARIO_LOGADO_RF,
                CodigoRf = USUARIO_LOGADO_RF,
                Nome = USUARIO_LOGADO_NOME,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }
        private async Task CriarUsuarioLogadoFundamental()
        {
            await InserirNaBase(new Usuario
            {
                Id = 1,
                Login = USUARIO_LOGADO_RF,
                CodigoRf = USUARIO_LOGADO_RF,
                Nome = USUARIO_LOGADO_NOME,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }
        #endregion Massa de Dados
    }
}
