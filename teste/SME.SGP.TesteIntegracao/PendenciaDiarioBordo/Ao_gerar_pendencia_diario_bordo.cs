using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaDiarioBordo
{
    public class Ao_gerar_pendencia_diario_bordo : TesteBase
    {
        private const string Professor_512_Rf_1001 = "1001";
        private const string Professor_512_Rf_1002 = "1002";
        private const string Professor_512_Rf_1003 = "1003";
        private const string Professor_513_Rf_1004 = "1004";
        private const string Professor_513_Rf_1005 = "1005";
        private const string Professor_513_Rf_1006 = "1006";
        private const string Professor_534_Rf_1007 = "1007";
        private const string Professor_534_Rf_1008 = "1008";
        private const string Professor_534_Rf_1009 = "1009";

        private const int Codigo_Regencia_Infantil_Emei_Manha_534 = 534;
        private const int Codigo_Regencia_Infantil_Emei_4h_512 = 512;
        private const int Codigo_Regencia_Infantil_Emei_2h_513 = 513;

        private const string CODIGO_TURMA_7A = "1";
        private const string CODIGO_TURMA_7B = "2";
        private const string CODIGO_TURMA_7P = "3";

        private const string Modalidade_EI_7P = "EI-7P";
        private const string Modalidade_EI_7A = "EI-7A";
        private const string Modalidade_EI_7B = "EI-7B";

        private const string Descricao_Regencia_Infantil_Emei_Manha_534 = "REGÊNCIA INFANTIL EMEI MANHÃ";
        private const string Descricao_Regencia_Infantil_Emei_4h_512 = "REGÊNCIA INFANTIL EMEI 4H";
        private const string Descricao_Regencia_Infantil_Emei_2h_513 = "REGÊNCIA INFANTIL EMEI 2H";

        private const string Nome_Escola_Emei_Paraisopolis = "CEU EMEI PARAISOPOLIS (DRE  CL)";
        private const string Nome_Escola_Emei_Sao_Paulo = "EMEI SAO PAULO";

        public Ao_gerar_pendencia_diario_bordo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }
        [Fact]
        public async Task Deve_gerar_pendencias_de_acordo_componente_do_professor()
        {
            var useCase = ServiceProvider.GetService<ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase>();

            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarCadastrosBasicos();

            var filtroUseCase = ObterFiltroUseCase();

            foreach (var item in filtroUseCase)
            {
                var jsonMensagem = JsonSerializer.Serialize(item);

                var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));

                retorno.ShouldBeTrue();
            }           

            foreach (var item in filtroUseCase)
            {
                foreach (var aulaProfessorComponente in item.AulasProfessoresComponentesCurriculares)
                {
                    var pendenciaRetorno = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(aulaProfessorComponente.ComponenteCurricularId, aulaProfessorComponente.ProfessorRf,1));

                    pendenciaRetorno.ShouldBeGreaterThan(0);

                    if (aulaProfessorComponente.ProfessorRf.Equals(Professor_512_Rf_1001))
                        pendenciaRetorno.ShouldBe(1);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_512_Rf_1002))
                        pendenciaRetorno.ShouldBe(2);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_512_Rf_1003))
                        pendenciaRetorno.ShouldBe(3);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_513_Rf_1004))
                        pendenciaRetorno.ShouldBe(4);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_513_Rf_1005))
                        pendenciaRetorno.ShouldBe(5);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_513_Rf_1006))
                        pendenciaRetorno.ShouldBe(6);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_534_Rf_1007))
                        pendenciaRetorno.ShouldBe(7);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_534_Rf_1008))
                        pendenciaRetorno.ShouldBe(8);
                    else if (aulaProfessorComponente.ProfessorRf.Equals(Professor_534_Rf_1009))
                        pendenciaRetorno.ShouldBe(9);
                }
            }
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencias_componente_do_professor_tipo_escola_ignorada_param_sistema()
        {
            var useCase = ServiceProvider.GetService<ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase>();

            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarParametroSistema_TiposUEIgnorarGeracaoPendencia(((int) TipoEscola.CEIDIRET).ToString());
            await CriarCadastrosBasicos(TipoEscola.CEIDIRET);

            var filtroUseCase = ObterFiltroUseCase();

            foreach (var item in filtroUseCase)
            {
                var jsonMensagem = JsonSerializer.Serialize(item);

                var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));

                retorno.ShouldBeFalse();
            }

            foreach (var item in filtroUseCase)
            {
                foreach (var aulaProfessorComponente in item.AulasProfessoresComponentesCurriculares)
                {
                    var pendenciaRetorno = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(aulaProfessorComponente.ComponenteCurricularId, aulaProfessorComponente.ProfessorRf, 1));
                    pendenciaRetorno.ShouldBe(0);
                }
            }
        }

        [Fact]
        public async Task Deve_gerar_pendencias_componente_do_professor_tipo_escola_ignorada_param_sistema_inativo()
        {
            var useCase = ServiceProvider.GetService<ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase>();

            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarParametroSistema_TiposUEIgnorarGeracaoPendencia(((int)TipoEscola.CEIDIRET).ToString(), false);
            await CriarCadastrosBasicos(TipoEscola.CEIDIRET);

            var filtroUseCase = ObterFiltroUseCase();

            foreach (var item in filtroUseCase)
            {
                var jsonMensagem = JsonSerializer.Serialize(item);

                var retorno = await useCase.Executar(new MensagemRabbit(jsonMensagem));

                retorno.ShouldBeTrue();
            }

            foreach (var item in filtroUseCase)
            {
                foreach (var aulaProfessorComponente in item.AulasProfessoresComponentesCurriculares)
                {
                    var pendenciaRetorno = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(aulaProfessorComponente.ComponenteCurricularId, aulaProfessorComponente.ProfessorRf, 1));
                    pendenciaRetorno.ShouldBeGreaterThan(0);
                }
            }
        }

        private async Task CriarParametroSistema_TiposUEIgnorarGeracaoPendencia(string valor, bool ativo = true)
        {
            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.TiposUEIgnorarGeracaoPendencia,
                Ativo = ativo,
                CriadoPor = "",
                CriadoRF = "",
                Valor = valor,
                Nome = "TiposUEIgnorarGeracaoPendencia",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Descricao = "Tipos de UE ignoradas na geração de pendências no ano"
            });
        }

        private async Task CriarCadastrosBasicos(TipoEscola tipoEscola = TipoEscola.EMEF)
        {
            await InserirNaBase(new TipoCalendario()
            {
                Situacao = true,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
                Nome = "Ano Atual - Calendário Infantil ",
                Periodo = Periodo.Anual,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Excluido = false
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 08, 20),
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 01),
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_4h_512.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7P,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 10),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_4h_512.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7P,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 11),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_4h_512.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7P,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_2h_513.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7A,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 10),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_2h_513.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7A,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 11),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_2h_513.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7A,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_Manha_534.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7B,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 10),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_Manha_534.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7B,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 11),
                ProfessorRf = "Sistema",
                DisciplinaId = Codigo_Regencia_Infantil_Emei_Manha_534.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = CODIGO_TURMA_7B,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 09),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1001,
                Login = Professor_512_Rf_1001,
                Nome = Professor_512_Rf_1001,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1002,
                Login = Professor_512_Rf_1002,
                Nome = Professor_512_Rf_1002,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1003,
                Login = Professor_512_Rf_1003,
                Nome = Professor_512_Rf_1003,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_513_Rf_1004,
                Login = Professor_513_Rf_1004,
                Nome = Professor_513_Rf_1004,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_513_Rf_1005,
                Login = Professor_513_Rf_1005,
                Nome = Professor_513_Rf_1005,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_513_Rf_1006,
                Login = Professor_513_Rf_1006,
                Nome = Professor_513_Rf_1006,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_534_Rf_1007,
                Login = Professor_534_Rf_1007,
                Nome = Professor_534_Rf_1007,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_534_Rf_1008,
                Login = Professor_534_Rf_1008,
                Nome = Professor_534_Rf_1008,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_534_Rf_1009,
                Login = Professor_534_Rf_1009,
                Nome = Professor_534_Rf_1009,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase("componente_curricular_area_conhecimento", "1", "'Área de conhecimento 1'");

            await InserirNaBase("componente_curricular_grupo_matriz", "1", "'Grupo matriz 1'");

            await InserirNaBase("componente_curricular", "512","512","1","1", "'ED.INF. EMEI 4 HS'", "false","false","true","false","false","true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 4H'");

            await InserirNaBase("componente_curricular", "513","512","1","1", "'ED.INF. EMEI 2 HS'", "false","false","true","false","false","true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI 2H'");
            
            await InserirNaBase("componente_curricular", "534", "512", "1","1", "'REG -EMEI -INT/MANHA'", "false","false","true","false","false","true", "'Regência de Classe Infantil'", "'REGÊNCIA INFANTIL EMEI MANHÃ'");

            await InserirNaBase(new Dre()
            {
                Id = 1,
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = tipoEscola,
                CodigoUe = "22"
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7A",
                CodigoTurma = CODIGO_TURMA_7A,
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7B",
                CodigoTurma = CODIGO_TURMA_7B,
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7P",
                CodigoTurma = CODIGO_TURMA_7P,
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });
        }

        private List<FiltroPendenciaDiarioBordoTurmaAulaDto> ObterFiltroUseCase()
        {
            return new List<FiltroPendenciaDiarioBordoTurmaAulaDto>()
            {
                new FiltroPendenciaDiarioBordoTurmaAulaDto()
                {
                    CodigoTurma = CODIGO_TURMA_7P,
                    TurmaComModalidade = Modalidade_EI_7P,
                    NomeEscola = Nome_Escola_Emei_Paraisopolis,
                    AulasProfessoresComponentesCurriculares = new List<AulaProfessorComponenteDto>()
                    {
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 1,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1001,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 1,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1002,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 1,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1003,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 2,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1001,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 2,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1002,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 2,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1003,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 3,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1001,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 3,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1002,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 3,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_4h_512,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_4h_512,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_512_Rf_1003,
                           PendenciaId = 0
                       }
                    }
                },
                new FiltroPendenciaDiarioBordoTurmaAulaDto()
                {
                    CodigoTurma = CODIGO_TURMA_7A,
                    TurmaComModalidade = Modalidade_EI_7A,
                    NomeEscola = Nome_Escola_Emei_Paraisopolis,
                    AulasProfessoresComponentesCurriculares = new List<AulaProfessorComponenteDto>()
                    {
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 4,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1004,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 4,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1005,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 4,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1006,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 5,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1004,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 5,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1005,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 5,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1006,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 6,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1004,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 6,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1005,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 6,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_2h_513,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_2h_513,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_513_Rf_1006,
                           PendenciaId = 0
                       },
                    }
                },
                new FiltroPendenciaDiarioBordoTurmaAulaDto()
                {
                    CodigoTurma = CODIGO_TURMA_7B,
                    TurmaComModalidade = Modalidade_EI_7B,
                    NomeEscola = Nome_Escola_Emei_Sao_Paulo,
                    AulasProfessoresComponentesCurriculares = new List<AulaProfessorComponenteDto>()
                    {
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 7,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1007,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 7,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1008,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 7,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1009,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 8,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1007,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 8,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1008,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 8,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1009,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 9,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1007,
                           PendenciaId = 0
                       },
                       new AulaProfessorComponenteDto()
                       {
                           AulaId = 9,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1008,
                           PendenciaId = 0
                       },
                        new AulaProfessorComponenteDto()
                       {
                           AulaId = 9,
                           ComponenteCurricularId = Codigo_Regencia_Infantil_Emei_Manha_534,
                           DescricaoComponenteCurricular = Descricao_Regencia_Infantil_Emei_Manha_534,
                           PeriodoEscolarId = 1,
                           ProfessorRf = Professor_534_Rf_1009,
                           PendenciaId = 0
                       },
                    }
                },
            };
        }        
    }
}
