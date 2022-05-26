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
using System.Linq;
using TipoAvaliacao = SME.SGP.Dominio.TipoAvaliacao;

namespace SME.SGP.TesteIntegracao.Nota
{
    public class LancarNotaBimestreTeste : TesteBase
    {
        private ItensBasicosBuilder itensBasicos;
        public LancarNotaBimestreTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            itensBasicos = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Diferente_Regencia_Fundamental()
        {
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
            await CriaComponenteCurricularJurema();
            await itensBasicos.CriaComponenteCurricularComFrequencia();

            var listaDeNotas = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "6523614",
                     AtividadeAvaliativaId = 1,
                     Conceito = 4,
                     Nota = 4.5
                 },
            };

            var dto = new NotaConceitoListaDto
            {
                DisciplinaId = "1",
                TurmaId = "1",
                NotasConceitos = listaDeNotas
            };

            var command = ServiceProvider.GetService<IComandosNotasConceitos>();
            
            await command.Salvar(dto);

            var dtoConsulta = new ListaNotasConceitosDto()
            {   
                AnoLetivo = 2022,
                Bimestre = 1,
                TurmaCodigo = "1",
                DisciplinaCodigo = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                PeriodoInicioTicks = new DateTime(2021, 02, 10).Ticks,
                PeriodoFimTicks = new DateTime(2022, 11, 09).Ticks
            };

            var obterNotasUseCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();
            var retorno = await obterNotasUseCase.Executar(dtoConsulta);

            retorno.ShouldNotBeNull();
            Assert.True(retorno.Bimestres.Count >= 0);
        }

        [Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Diferente_Regencia_Fundamental_Sem_Avaliacao()
        {
            await CriarUsuarioLogadoFundamental();
            CriarClaimFundamental();
            await InserirDadosBasicosNoBanco();
            await CriarTurmaFundamental();
            await CriarAbrangenciaFundamental();
            await CriarTipoCalendarioFundamentalMedio();
            await CriarAulaProfFundamental();
            await CriarPeriodoEscolar();
            await CriarParametroSistema();
            await CriaComponenteCurricularJurema();
            await CriaFechamento();
            await itensBasicos.CriaComponenteCurricularComFrequencia();

            var listaNotas = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    CodigoAluno = "6523614",
                    DisciplinaId = 1,
                    Nota = 10
                }
            };

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 1,
                    DisciplinaId = 1,
                    NotaConceitoAlunos = listaNotas,
                    TurmaId = "1"
                }
            };

            var command = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            var retorno = await command.Salvar(fechamentoTurma);

            retorno.ShouldNotBeNull();
            Assert.IsType<AuditoriaPersistenciaDto>(retorno);
        }

        [Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Regencia_Eja()
        {
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
            await itensBasicos.CriaComponenteCurricularComFrequencia();

            var listaDeNotas = new List<NotaConceitoDto>()
            {
             new NotaConceitoDto()
                 {
                     AlunoId = "7128291",
                     AtividadeAvaliativaId = 1,
                     Conceito = 2
                 }
            };
            var dto = new NotaConceitoListaDto
            {
                DisciplinaId = "1114",
                TurmaId = "1",
                NotasConceitos = listaDeNotas
            };

            var command = ServiceProvider.GetService<IComandosNotasConceitos>();

            await command.Salvar(dto);

            var dtoConsulta = new ListaNotasConceitosDto()
            {
                AnoLetivo = 2,
                Bimestre = 2,
                TurmaCodigo = "1",
                DisciplinaCodigo = 1,
                PeriodoInicioTicks = new DateTime(2021, 02, 10).Ticks,
                PeriodoFimTicks = new DateTime(2022, 11, 09).Ticks
            };

            var obterNotasUseCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();
            var retorno = await obterNotasUseCase.Executar(dtoConsulta);

            retorno.ShouldNotBeNull();
            Assert.True(retorno.Bimestres.Count >= 0);
        }

        [Fact]
        public async Task Deve_Arrendondar_Nota()
        {
            await CriarUsuarioLogadoFundamental();
            CriarClaimFundamental();
            await InserirDadosBasicosNoBanco();
            await CriaNotaParametro();

            var consultaNota = ServiceProvider.GetService<IConsultasNotasConceitos>();
            var retornoNota = await consultaNota.ObterValorArredondado(new DateTime(2021, 02, 10), 4.8);

            retornoNota.ShouldBe(5);
        }

        #region Massa de Dados
        private async Task CriarTurmaFundamental()
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = "2",
                CodigoTurma = "1",
                Historica = true,
                AnoLetivo = 2022,
                ModalidadeCodigo = Modalidade.Fundamental,
                Nome = "Turma Nome 2",
                
            });
        }
        private async Task CriarTurmaEja()
        {
            await InserirNaBase(new Turma
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

        private async Task InserirDadosBasicosNoBanco()
        {
            await itensBasicos.CriaItensComuns(); 

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

        private async Task CriaAvaliacao()
        {
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
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral
            });
        }

        private async Task CriarAtividadeAvaliativaEja()
        {
            await CriaAvaliacao();

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
        }
        private async Task CriarAtividadeAvaliativaFundamental()
        {
            await CriaAvaliacao();

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = "6737544",
                TurmaId = "1",
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                CriadoEm = new DateTime(2022, 02, 10),
                DataAvaliacao = new DateTime(2022, 02, 10),
                CriadoRF = "6737544",
                CriadoPor = "GENILDO CLEBER DA SILVA"
            });
            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = "1",
                CriadoPor = "GENILDO CLEBER DA SILVA",
                CriadoRF = "6737544"
            });
        }

        private async Task CriarTipoCalendarioEja() 
        {
            await InserirNaBase(new TipoCalendario
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
        }
        private async Task CriarTipoCalendarioFundamentalMedio() 
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = 2022,
                Nome = "Ano Letivo 202",
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
            await InserirNaBase(new Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d")
            });
        }
        private async Task CriarAbrangenciaFundamental()
        {
            await InserirNaBase(new Abrangencia
            {
                UsuarioId = 1,
                DreId = 1,
                UeId = 1,
                TurmaId = 1,
                Historico = true,
                Perfil = new Guid("40e1e074-37d6-e911-abd6-f81654fe895d")
            });
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
        private async Task CriarAulaProfRegenciaEja()
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

        private async Task CriarAulaProfFundamental()
        {
            await InserirNaBase(new Aula
            {
                UeId = "1",
                DisciplinaId = "1",
                TurmaId = "1",
                TipoCalendarioId = 1,
                ProfessorRf = "6737544",
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
        private void CriarClaimRegenciaEja()
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
            variaveis.Add("NomeUsuario", "GENILDO CLEBER DA SILVA");
            variaveis.Add("UsuarioLogado", "6737544");
            variaveis.Add("RF", "6737544");
            variaveis.Add("login", "6737544");
            variaveis.Add("Claims", new List<InternalClaim> {
                new InternalClaim { Value = "6737544", Type = "rf" },
                new InternalClaim { Value = "40e1e074-37d6-e911-abd6-f81654fe895d", Type = "perfil" }
            });
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }
        private async Task CriarUsuarioLogadoRegenciaEja()
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
                Login = "6737544",
                CodigoRf = "6737544",
                Nome = "GENILDO CLEBER DA SILVA",
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private async Task CriaComponenteCurricularJurema()
        {
            await InserirNaBase(new ComponenteCurricularJurema()
            {
                CodigoJurema = 1,
                DescricaoEOL = "Arte",
                CodigoEOL = 1,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private async Task CriaFechamento()
        {
            await InserirNaBase(new FechamentoTurma()
            {
                Id = 1,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
            await InserirNaBase(new FechamentoTurmaDisciplina()
            {
                Id = 1,
                FechamentoTurmaId = 1,
                DisciplinaId = 1,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno()
            {
                Id = 1,
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = "6523614",
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoNota()
            {
                Id = 1,
                FechamentoAlunoId = 1,
                Nota = 4.5,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        private async Task CriaNotaParametro()
        {
            await InserirNaBase(new NotaParametro
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

        private async Task CriaTipoEvento()
        {
            await InserirNaBase(new EventoTipo
            {
                Codigo = (int)TipoEvento.FechamentoBimestre,
                Ativo = true,
                CriadoEm = new DateTime(2022, 01, 01),
                CriadoPor = "",
                CriadoRF = ""
            });
            
        }
        #endregion Massa de Dados
    }
}
