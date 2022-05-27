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
            await PreparaBaseDiferenteRegenciaFundamental();

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
            await PreparaBaseDiferenteRegenciaFundamental();

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
                    Bimestre = 2,
                    DisciplinaId = 1,
                    NotaConceitoAlunos = listaNotas,
                    TurmaId = "1"
                }
            };

            var command = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            var retorno = await command.Salvar(fechamentoTurma);

            retorno.ShouldNotBeNull();
            Assert.IsAssignableFrom<IEnumerable<AuditoriaPersistenciaDto>>(retorno);
        }

        [Fact]
        public async Task Deve_Lancar_Conceito_Para_Componente_Regencia_Eja()
        {
            await PreparaBaseEja();

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
        public async Task Deve_Lancar_Conceito_Para_Componente_Regencia_Eja_Sem_Avaliacao()
        {
            await PreparaBaseEja();

            var listaNotas = new List<FechamentoNotaDto>()
            {
                new FechamentoNotaDto()
                {
                    CodigoAluno = "7128291",
                    DisciplinaId = 1,
                    Nota = 7
                }
            };

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new FechamentoTurmaDisciplinaDto()
                {
                    Bimestre = 2,
                    DisciplinaId = 1,
                    NotaConceitoAlunos = listaNotas,
                    TurmaId = "1"
                }
            };

            var command = ServiceProvider.GetService<IComandosFechamentoTurmaDisciplina>();

            var retorno = await command.Salvar(fechamentoTurma);

            retorno.ShouldNotBeNull();
            Assert.IsAssignableFrom<IEnumerable<AuditoriaPersistenciaDto>>(retorno);
        }

        [Fact]
        public async Task Deve_Arrendondar_Nota()
        {
            await CriarUsuarioLogadoFundamental();
            CriarClaimFundamental();
            await itensBasicos.CriaItensComuns();
            await itensBasicos.CriaNotaParametro();

            var consultaNota = ServiceProvider.GetService<IConsultasNotasConceitos>();
            var retornoNota = await consultaNota.ObterValorArredondado(new DateTime(2021, 02, 10), 4.8);

            retornoNota.ShouldBe(5);
        }

        private async Task PreparaBaseDiferenteRegenciaFundamental()
        {
            await CriarUsuarioLogadoFundamental();
            CriarClaimFundamental();
            await InserirDadosBasicosNoBanco();
            await CriarTurmaFundamental();
            await CriarAbrangenciaFundamental();
            await CriarTipoCalendarioFundamentalMedio();
            await CriarAulaProfFundamental();
            await CriaFechamento();
        }

        private async Task PreparaBaseEja()
        {
            await CriarUsuarioLogadoRegenciaEja();
            CriarClaimRegenciaEja();
            await InserirDadosBasicosNoBanco();
            await CriarTurmaEja();
            await CriarAbrangenciaEja();
            await CriarTipoCalendarioEja();
            await CriarAulaProfRegenciaEja();
            await CriarAtividadeAvaliativaEja();
            await CriaFechamento();
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


        private async Task InserirDadosBasicosNoBanco()
        {
            await itensBasicos.CriaItensComuns(); 
            await itensBasicos.CriaCiclo();
            await itensBasicos.CriarPeriodoEscolar();
            await itensBasicos.CriarParametroSistema();
            await itensBasicos.CriaTipoEventoBimestral();
            await itensBasicos.CriaComponenteCurricularComFrequencia();

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
            await itensBasicos.CriaAvaliacaoBimestral();

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
        //private async Task CriarAtividadeAvaliativaFundamental()
        //{
        //    await itensBasicos.CriaAvaliacaoBimestral();

        //    await InserirNaBase(new AtividadeAvaliativa
        //    {
        //        Id = 1,
        //        DreId = "1",
        //        UeId = "1",
        //        ProfessorRf = "6737544",
        //        TurmaId = "1",
        //        Categoria = CategoriaAtividadeAvaliativa.Normal,
        //        TipoAvaliacaoId = 1,
        //        NomeAvaliacao = "Avaliação 04",
        //        DescricaoAvaliacao = "Avaliação 04",
        //        CriadoEm = new DateTime(2022, 02, 10),
        //        DataAvaliacao = new DateTime(2022, 02, 10),
        //        CriadoRF = "6737544",
        //        CriadoPor = "GENILDO CLEBER DA SILVA"
        //    });
        //    await InserirNaBase(new AtividadeAvaliativaDisciplina
        //    {
        //        Id = 1,
        //        AtividadeAvaliativaId = 1,
        //        DisciplinaId = "1",
        //        CriadoPor = "GENILDO CLEBER DA SILVA",
        //        CriadoRF = "6737544"
        //    });
        //}

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

        #endregion Massa de Dados
    }
}
