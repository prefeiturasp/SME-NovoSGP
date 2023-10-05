using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaDevolutiva
{
    public class Ao_gerar_pendencia_devolutiva : TesteBase
    {
        public Ao_gerar_pendencia_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_ComParamentro_GerarPendenciaDevolutivaSemDiarioBordo_Desativado()
        {
            var useCase = ServiceProvider.GetService<IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase>();
            var parametroPermitiGerarPendenciaDevolutiva = new ParametrosSistema()
            {
                Nome = "GerarPendenciaDevolutivaSemDiarioBordo",
                Tipo = TipoParametroSistema.GerarPendenciaDevolutivaSemDiarioBordo,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = false,
                Descricao = "GerarPendenciaDevolutivaSemDiarioBordo",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            var parametroDataInicioGeracaoPendencias = new ParametrosSistema()
            {
                Nome = "DataInicioGeracaoPendencias",
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "DataInicioGeracaoPendencias",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            await InserirNaBase(parametroPermitiGerarPendenciaDevolutiva);
            await InserirNaBase(parametroDataInicioGeracaoPendencias);

            var msgRabbit = MontarMensagemRabbit();
            var retornoUseCase = await useCase.Executar(msgRabbit);

            Assert.False(retornoUseCase);
        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_ComParamentro_DataInicioGeracaoPendencias_Desativado()
        {
            var useCase = ServiceProvider.GetService<IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase>();
            var parametroPermitiGerarPendenciaDevolutiva = new ParametrosSistema()
            {
                Nome = "GerarPendenciaDevolutivaSemDiarioBordo",
                Tipo = TipoParametroSistema.GerarPendenciaDevolutivaSemDiarioBordo,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "GerarPendenciaDevolutivaSemDiarioBordo",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            var parametroDataInicioGeracaoPendencias = new ParametrosSistema()
            {
                Nome = "DataInicioGeracaoPendencias",
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = false,
                Descricao = "DataInicioGeracaoPendencias",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            await InserirNaBase(parametroPermitiGerarPendenciaDevolutiva);
            await InserirNaBase(parametroDataInicioGeracaoPendencias);

            var msgRabbit = MontarMensagemRabbit();
            var retornoUseCase = await useCase.Executar(msgRabbit);

            Assert.False(retornoUseCase);
        }

        [Fact]
        public async Task Deve_Gerar_Pendencia_ComParamentro_DataInicioGeracaoPendencias_GerarPendenciaDevolutivaSemDiarioBordo_Ativados()
        {
            var useCase = ServiceProvider.GetService<IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase>();
            var parametroPermitiGerarPendenciaDevolutiva = new ParametrosSistema()
            {
                Nome = "GerarPendenciaDevolutivaSemDiarioBordo",
                Tipo = TipoParametroSistema.GerarPendenciaDevolutivaSemDiarioBordo,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "GerarPendenciaDevolutivaSemDiarioBordo",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            var parametroDataInicioGeracaoPendencias = new ParametrosSistema()
            {
                Nome = "DataInicioGeracaoPendencias",
                Tipo = TipoParametroSistema.DataInicioGeracaoPendencias,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "DataInicioGeracaoPendencias",
                Valor = "",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            };
            await InserirNaBase(parametroPermitiGerarPendenciaDevolutiva);
            await InserirNaBase(parametroDataInicioGeracaoPendencias);

            var msgRabbit = MontarMensagemRabbit();
            var retornoUseCase = await useCase.Executar(msgRabbit);

            Assert.True(retornoUseCase);
        }


        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_NaoExisteDiario_Bordo_Sem_Devolutiva_ComMais25Dias()
        {
            var useCase = ServiceProvider.GetService<IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase>();
            await CriarDadosSemPendenciaComMais25Dias();
            var msg = JsonConvert.SerializeObject(new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: DateTime.Now.Year, dreId: 1, ueCodigo: "2345678", turmaId: 1, ueId: 1));
            var retornoUseCase = await useCase.Executar(new MensagemRabbit(msg));
            Assert.True(retornoUseCase);
        }


        [Fact]
        public async Task Deve_Gerar_Pendencia_ExisteDiario_Bordo_Sem_Devolutiva_ComMais25Dias()
        {
            var useCase = ServiceProvider.GetService<IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase>();
            await CriarDadosComPendenciaComMais25Dias();
            var msg = JsonConvert.SerializeObject(new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: DateTime.Now.Year, dreId: 1, ueCodigo: "2345678", turmaId: 1, ueId: 1));
            var retornoUseCase = await useCase.Executar(new MensagemRabbit(msg));
            Assert.True(retornoUseCase);
        }

        private MensagemRabbit MontarMensagemRabbit()
        {
            var msg = JsonConvert.SerializeObject(new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: DateTime.Now.Year));
            return new MensagemRabbit(msg);
        }
        private async Task CriarDadosSemPendenciaComMais25Dias()
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PeriodoDeDiasDevolutiva",
                Tipo = TipoParametroSistema.PeriodoDeDiasDevolutiva,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "PeriodoDeDiasDevolutiva",
                Valor = "25",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new Dre()
            {
                Abreviacao = "DRE Teste",
                CodigoDre = "2345678",
                DataAtualizacao = DateTime.Now,
                Nome = "DRE Teste Devolutiva"
            });
            await InserirNaBase(new Ue()
            {
                CodigoUe = "2345678",
                DataAtualizacao = DateTime.Now,
                DreId = 1,
                Nome = "UE Teste Devolutiva",
                TipoEscola = TipoEscola.EMEI
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Ano = "7",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = "2345678",
                TipoTurma = TipoTurma.Regular,
                DataAtualizacao = DateTime.Now,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Nome = "Turma xyz",
                QuantidadeDuracaoAula = 1,
                Semestre = 1,
                TipoTurno = (int)TipoTurno.Manha,
                SerieEnsino = "B",
                UeId = 1,
                NomeFiltro = "NomeFiltro",
                Historica = false,
                EtapaEJA = 0,
                EnsinoEspecial = false,
                DataInicio = DateTime.Now.AddDays(-20),
                DataFim = DateTime.Now,
                Extinta = false
            });
            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Excluido = false,
                Migrado = true,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Nome = "TipoCalendario Teste",
                Periodo = Periodo.Anual,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "2345678",
                DisciplinaId = "512",
                TurmaId = "2345678",
                TipoCalendarioId = 1,
                ProfessorRf = "8019347",
                Quantidade = 1,
                DataAula = DateTime.Now.AddDays(-28),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Excluido = false,
            });

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");
            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");
            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");
            await InserirNaBase(new Dominio.DiarioBordo()
            {
                AulaId = 1,
                ComponenteCurricularId = 512,
                TurmaId = 1,
                DevolutivaId = null,
                Planejamento = "Planejado",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });

        }

        private async Task CriarDadosComPendenciaComMais25Dias()
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PeriodoDeDiasDevolutiva",
                Tipo = TipoParametroSistema.PeriodoDeDiasDevolutiva,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = "PeriodoDeDiasDevolutiva",
                Valor = "25",
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new Dre()
            {
                Abreviacao = "DRE Teste",
                CodigoDre = "2345678",
                DataAtualizacao = DateTime.Now,
                Nome = "DRE Teste Devolutiva"
            });
            await InserirNaBase(new Ue()
            {
                CodigoUe = "2345678",
                DataAtualizacao = DateTime.Now,
                DreId = 1,
                Nome = "UE Teste Devolutiva",
                TipoEscola = TipoEscola.EMEI
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Ano = "7",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                CodigoTurma = "2345678",
                TipoTurma = TipoTurma.Regular,
                DataAtualizacao = DateTime.Now,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                Nome = "Turma xyz",
                QuantidadeDuracaoAula = 1,
                Semestre = 1,
                TipoTurno = (int)TipoTurno.Manha,
                SerieEnsino = "B",
                UeId = 1,
                NomeFiltro = "NomeFiltro",
                Historica = false,
                EtapaEJA = 0,
                EnsinoEspecial = false,
                DataInicio = DateTime.Now.AddDays(-20),
                DataFim = DateTime.Now,
                Extinta = false
            });
            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Excluido = false,
                Migrado = true,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Nome = "TipoCalendario Teste",
                Periodo = Periodo.Anual,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "2345678",
                DisciplinaId = "512",
                TurmaId = "2345678",
                TipoCalendarioId = 1,
                ProfessorRf = "8019347",
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 04),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Excluido = false,
            });
            await InserirNaBase(new Dominio.Aula()
            {
                UeId = "2345678",
                DisciplinaId = "512",
                TurmaId = "2345678",
                TipoCalendarioId = 1,
                ProfessorRf = "8019347",
                Quantidade = 1,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 04),
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0",
                Excluido = false,
            });
            await InserirNaBase(new PeriodoEscolar() 
            {
                Migrado = true,
                Bimestre = 1,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 07),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 04, 29),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase(new PeriodoEscolar()
            {
                Migrado = true,
                Bimestre = 2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 02),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 07, 22),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");
            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");
            await InserirNaBase("componente_curricular", "512", "512", "1", "1", "'ED.INF. EMEI 4 HS'", "false", "false", "true", "false", "false", "true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");
            await InserirNaBase(new Dominio.DiarioBordo()
            {
                AulaId = 1,
                ComponenteCurricularId = 512,
                TurmaId = 1,
                DevolutivaId = null,
                Planejamento = "Planejado",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "0"
            });
        }
    }
}
