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

                if (item.CodigoRf.Equals("1001"))
                    pendenciaRetorno.ShouldBe(1);
                else if (item.CodigoRf.Equals("1002"))
                    pendenciaRetorno.ShouldBe(2);
                else if (item.CodigoRf.Equals("1003"))
                    pendenciaRetorno.ShouldBe(3);
                else if (item.CodigoRf.Equals("1004"))
                    pendenciaRetorno.ShouldBe(4);
                else if (item.CodigoRf.Equals("1005"))
                    pendenciaRetorno.ShouldBe(5);
                else if (item.CodigoRf.Equals("1006"))
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
                DisciplinaId = "512",
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = "1234",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 05, 09),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1001",
                Login = "1001",
                Nome = "Usuario 1001",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1002",
                Login = "1002",
                Nome = "Usuario 1002",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1003",
                Login = "1003",
                Nome = "Usuario 1003",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1004",
                Login = "1004",
                Nome = "Usuario 1004",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1005",
                Login = "1005",
                Nome = "Usuario 1005",
                PerfilAtual = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(2022, 01, 01),
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = "1006",
                Login = "1006",
                Nome = "Usuario 1006",
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
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1234",
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
                        CodigoRf = "1001",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = 512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    },
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)"
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = "1002",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = 512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1,
                    },
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)"
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7P",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = "1003",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 4H",
                        DisciplinaId = 512
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7A",
                    NomeEscola = "CEU EMEI PARAISOPOLIS (DRE  CL)",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = "1004",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI 2H",
                        DisciplinaId = 513
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7B",
                    NomeEscola = "EMEI SAO PAULO",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = "1005",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI MANHÃ",
                        DisciplinaId = 534
                    },
                    Aula = new AulaComComponenteDto()
                    {
                        Id = 1,
                        PeriodoEscolarId = 1
                    }
                },
                new SalvarPendenciaDiarioBordoCommand()
                {
                    CodigoTurma = "1234",
                    TurmaComModalidade = "EI-7B",
                    NomeEscola = "EMEI SAO PAULO",
                    ProfessorComponente = new ProfessorEComponenteInfantilDto()
                    {
                        CodigoRf = "1006",
                        DescricaoComponenteCurricular = "REGÊNCIA INFANTIL EMEI MANHÃ",
                        DisciplinaId = 534
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
