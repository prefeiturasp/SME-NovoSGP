using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_gerar_pendencia_diario_bordo : TesteBase
    {
        private const string Professor_512_Rf_1001 = "1001";
        private const string Professor_512_Rf_1002 = "1002";
        private const string Professor_512_Rf_1003 = "1003";
        private const string Professor_513_Rf_1004 = "1004";
        private const string Professor_534_Rf_1005 = "1005";
        private const string Professor_534_Rf_1006 = "1006";

        private const int Regencia_Infantil_Emei_Manha_534 = 534;
        private const int Regencia_Infantil_Emei_4h_512 = 512;
        private const int Regencia_Infantil_Emei_2h_513 = 513;

        private const string Turma_Emei_7P = "7P";
        private const string Turma_Emei_7A = "7A";
        private const string Turma_Emei_7B = "7B";

        public Ao_gerar_pendencia_diario_bordo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            
        }
        [Fact]
        public async Task Deve_gerar_pendencias_de_acordo_componente_do_professor()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            await CriarCadastrosBasicos();

            var salvarPendenciaDiarioBordoCommand = ObterSalvarPendenciaDiarioBordoCommand();

            foreach (var item in salvarPendenciaDiarioBordoCommand)
                await mediator.Send(item);

            foreach (var item in salvarPendenciaDiarioBordoCommand.Select(s => s.ProfessorComponente))
            {
                var pendenciaRetorno = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(item.DisciplinaId, item.CodigoRf,1));

                pendenciaRetorno.ShouldBeGreaterThan(0);

                if (item.CodigoRf.Equals(Professor_512_Rf_1001))
                    pendenciaRetorno.ShouldBe(1);
                else if (item.CodigoRf.Equals(Professor_512_Rf_1002))
                    pendenciaRetorno.ShouldBe(2);
                else if (item.CodigoRf.Equals(Professor_512_Rf_1003))
                    pendenciaRetorno.ShouldBe(3);
                else if (item.CodigoRf.Equals(Professor_513_Rf_1004))
                    pendenciaRetorno.ShouldBe(4);
                else if (item.CodigoRf.Equals(Professor_534_Rf_1005))
                    pendenciaRetorno.ShouldBe(5);
                else if (item.CodigoRf.Equals(Professor_534_Rf_1006))
                    pendenciaRetorno.ShouldBe(6);
            }
        }


        private async Task CriarCadastrosBasicos()
        {
            await InserirNaBase(new TipoCalendario()
            {
                Situacao = true,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 10),
                Nome = "2022 - Calendário Infantil ",
                Periodo = Periodo.Anual,
                AnoLetivo = 2022,
                Excluido = false
            });

            await InserirNaBase(new PeriodoEscolar()
            {
                Bimestre = 1,
                PeriodoFim = new DateTime(2022, 08, 20),
                PeriodoInicio = new DateTime(2022, 02, 01),
                TipoCalendarioId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(2022, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Regencia_Infantil_Emei_4h_512.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = Turma_Emei_7P,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 05, 09),
            });

            await InserirNaBase(new Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(2022, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Regencia_Infantil_Emei_2h_513.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = Turma_Emei_7A,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 05, 09),
            });

            await InserirNaBase(new Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(2022, 05, 09),
                ProfessorRf = "Sistema",
                DisciplinaId = Regencia_Infantil_Emei_Manha_534.ToString(),
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = Turma_Emei_7B,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 05, 09),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1001,
                Login = Professor_512_Rf_1001,
                Nome = Professor_512_Rf_1001,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1002,
                Login = Professor_512_Rf_1002,
                Nome = Professor_512_Rf_1002,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_512_Rf_1003,
                Login = Professor_512_Rf_1003,
                Nome = Professor_512_Rf_1003,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_513_Rf_1004,
                Login = Professor_513_Rf_1004,
                Nome = Professor_513_Rf_1004,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_534_Rf_1005,
                Login = Professor_534_Rf_1005,
                Nome = Professor_534_Rf_1005,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = Professor_534_Rf_1006,
                Login = Professor_534_Rf_1006,
                Nome = Professor_534_Rf_1006,
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
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
                TipoEscola = TipoEscola.EMEF,
                CodigoUe = "22"
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7P",
                CodigoTurma = "EI - 7P",
                Ano = "1",
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7A",
                CodigoTurma = "EI - 7A",
                Ano = "1",
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new Turma()
            {
                Nome = "7B",
                CodigoTurma = "EI - 7B",
                Ano = "1",
                AnoLetivo = 2022,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });
        }

        private List<SalvarPendenciaDiarioBordoCommand> ObterSalvarPendenciaDiarioBordoCommand()
        {
            var lstPendenciasDiarioBordo = new List<SalvarPendenciaDiarioBordoCommand>()
            {
                new SalvarPendenciaDiarioBordoCommand()
                {
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_512_Rf_1001,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = Regencia_Infantil_Emei_4h_512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    },
                    CodigoTurma = Turma_Emei_7P,
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)"
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_512_Rf_1002,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = Regencia_Infantil_Emei_4h_512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1,
                    },
                    CodigoTurma = Turma_Emei_7P,
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)"
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = Turma_Emei_7P,
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_512_Rf_1003,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = Regencia_Infantil_Emei_4h_512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = Turma_Emei_7A,
                    TurmaComModalidade = "EI-7A",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_513_Rf_1004,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 2H",
                        DisciplinaId = Regencia_Infantil_Emei_2h_513
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = Turma_Emei_7B,
                    TurmaComModalidade = "EI-7B",
                    NomeEscola = "EMEI SAO PAULO",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_534_Rf_1005,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI MANHÃ",
                        DisciplinaId = Regencia_Infantil_Emei_Manha_534
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = Turma_Emei_7B,
                    TurmaComModalidade = "EI-7B",
                    NomeEscola = "EMEI SAO PAULO",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = Professor_534_Rf_1006,
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI MANHÃ",
                        DisciplinaId = Regencia_Infantil_Emei_Manha_534
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
            };

            return lstPendenciasDiarioBordo;
        }        
    }
}
