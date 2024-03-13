using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.FrequenciaTurmaEvasao
{
    public class Ao_obter_alunos_frequencia_turma_evasao : TesteBase
    {
        public Ao_obter_alunos_frequencia_turma_evasao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_abaixo_50_porcento_por_dre()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                DreCodigo = "1",
                Mes = 0
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(13);
            for (int i = 1; i <= 10; i++)
                resultados.Items.Take(10).Any(al => al.Aluno.Equals($"Aluno {i} ({i})")
                                          && al.Turma.Equals("EM-7A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 1")
                                          && al.PercentualFrequencia.Equals(40)).ShouldBeTrue();

            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-7A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 1");

            for (int i = 11; i <= 13; i++)
                resultados.Items.Skip(10).Take(3).Any(al => al.Aluno.Equals($"Aluno {i-10} ({i-10})")
                                          && al.Turma.Equals("EM-8A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 2")
                                          && al.PercentualFrequencia.Equals(40)).ShouldBeTrue();

            var ultimoAluno = resultados.Items.LastOrDefault();
            ultimoAluno.Aluno.ShouldBe("Aluno 3 (3)");
            ultimoAluno.Turma.ShouldBe("EM-8A");
            ultimoAluno.Dre.ShouldBe("DRE - BT");
            ultimoAluno.Ue.ShouldBe("UE - 2");
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_sem_presenca_por_dre()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                DreCodigo = "1",
                Mes = 5
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(6);
            resultados.Items.Take(1).Any(al => al.Aluno.Equals($"Aluno 1 (1)")
                                          && al.Turma.Equals("EM-7A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 1")
                                          && al.PercentualFrequencia.Equals(0)).ShouldBeTrue();

            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-7A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 1");

            for (int i = 1; i <= 5; i++)
                resultados.Items.Skip(1).Take(5).Any(al => al.Aluno.Equals($"Aluno {i} ({i})")
                                          && al.Turma.Equals("EM-8A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 2")
                                          && al.PercentualFrequencia.Equals(0)).ShouldBeTrue();

            var ultimoAluno = resultados.Items.LastOrDefault();
            ultimoAluno.Aluno.ShouldBe("Aluno 5 (5)");
            ultimoAluno.Turma.ShouldBe("EM-8A");
            ultimoAluno.Dre.ShouldBe("DRE - BT");
            ultimoAluno.Ue.ShouldBe("UE - 2");
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_abaixo_50_porcento_por_ue()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                UeCodigo = "1",
                Mes = 5
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(4);
            for (int i = 1; i <= 4; i++)
                resultados.Items.Take(10).Any(al => al.Aluno.Equals($"Aluno {i} ({i})")
                                          && al.Turma.Equals("EM-7A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 1")
                                          && al.PercentualFrequencia.Equals(40)).ShouldBeTrue();

            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-7A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 1");


            var ultimoAluno = resultados.Items.LastOrDefault();
            ultimoAluno.Aluno.ShouldBe("Aluno 4 (4)");
            ultimoAluno.Turma.ShouldBe("EM-7A");
            ultimoAluno.Dre.ShouldBe("DRE - BT");
            ultimoAluno.Ue.ShouldBe("UE - 1");
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_sem_presenca_por_ue()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                UeCodigo = "2",
                Mes = 5
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(5);
            for (int i = 1; i <= 5; i++)
                resultados.Items.Any(al => al.Aluno.Equals($"Aluno {i} ({i})")
                                          && al.Turma.Equals("EM-8A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 2")
                                          && al.PercentualFrequencia.Equals(0)).ShouldBeTrue();

            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-8A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 2");
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_abaixo_50_porcento_por_turma()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                TurmaCodigo = "1",
                Mes = 5
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(4);
            for (int i = 1; i <= 4; i++)
                resultados.Items.Take(10).Any(al => al.Aluno.Equals($"Aluno {i} ({i})")
                                          && al.Turma.Equals("EM-7A")
                                          && al.Dre.Equals("DRE - BT")
                                          && al.Ue.Equals("UE - 1")
                                          && al.PercentualFrequencia.Equals(40)).ShouldBeTrue();

            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-7A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 1");


            var ultimoAluno = resultados.Items.LastOrDefault();
            ultimoAluno.Aluno.ShouldBe("Aluno 4 (4)");
            ultimoAluno.Turma.ShouldBe("EM-7A");
            ultimoAluno.Dre.ShouldBe("DRE - BT");
            ultimoAluno.Ue.ShouldBe("UE - 1");
        }

        [Fact]
        public async Task Deve_obter_apenas_alunos_sem_presenca_por_turma()
        {
            await CriarItensBasicos();
            await CriarRegistrosParaConsulta();

            var useCase = ServiceProvider.GetService<IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase>();

            var filtro = new FiltroGraficoFrequenciaTurmaEvasaoAlunoDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                Modalidade = Modalidade.Medio,
                TurmaCodigo = "1",
                Mes = 5
            };

            var resultados = await useCase.Executar(filtro);
            resultados.Items.Count().ShouldBe(1);
            var primeiroAluno = resultados.Items.FirstOrDefault();
            primeiroAluno.Aluno.ShouldBe("Aluno 1 (1)");
            primeiroAluno.Turma.ShouldBe("EM-7A");
            primeiroAluno.Dre.ShouldBe("DRE - BT");
            primeiroAluno.Ue.ShouldBe("UE - 1");
            primeiroAluno.PercentualFrequencia.ShouldBe(0);
        }

        private async Task CriarItensBasicos()
        {
            await InserirNaBase(new Dre
            {
                Id = 1,
                CodigoDre = "1",
                Abreviacao = "DRE - BT"
            });

            await InserirNaBase(new Ue
            {
                Id = 1,
                CodigoUe = "1",
                DreId = 1,
                Nome = "UE - 1",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Ue
            {
                Id = 2,
                CodigoUe = "2",
                DreId = 1,
                Nome = "UE - 2",
                TipoEscola = TipoEscola.Nenhum
            });

            await InserirNaBase(new Dominio.Turma
            {
                Id = 1,
                UeId = 1,
                Ano = "1",
                CodigoTurma = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = "7A"
            });

            await InserirNaBase(new Dominio.Turma
            {
                Id = 2,
                UeId = 2,
                Ano = "1",
                CodigoTurma = "2",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                ModalidadeCodigo = Modalidade.Medio,
                Nome = "8A"
            });

            await InserirNaBase(new TipoCalendario
            {
                Id = 1,
                Nome = "",
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        private async Task CriarRegistrosParaConsulta()
        {
            await InserirNaBase(new Dominio.FrequenciaTurmaEvasao
            {
                TurmaId = 1,
                Mes = 0,
                QuantidadeAlunosAbaixo50Porcento = 10,
                QuantidadeAlunos0Porcento = 0
            });
            await CriarRegistrosParaConsultaAlunosFreqAbaixo50Porcento(10);
            await InserirNaBase(new Dominio.FrequenciaTurmaEvasao
            {
                TurmaId = 2,
                Mes = 0,
                QuantidadeAlunosAbaixo50Porcento = 3,
                QuantidadeAlunos0Porcento = 0
            });
            await CriarRegistrosParaConsultaAlunosFreqAbaixo50Porcento(3);
            await InserirNaBase(new Dominio.FrequenciaTurmaEvasao
            {
                TurmaId = 1,
                Mes = 5,
                QuantidadeAlunosAbaixo50Porcento = 4,
                QuantidadeAlunos0Porcento = 1
            });
            await CriarRegistrosParaConsultaAlunosFreqAbaixo50Porcento(4);
            await CriarRegistrosParaConsultaAlunosSemPresenca(1);
            await InserirNaBase(new Dominio.FrequenciaTurmaEvasao
            {
                TurmaId = 2,
                Mes = 5,
                QuantidadeAlunosAbaixo50Porcento = 0,
                QuantidadeAlunos0Porcento = 5
            });
            await CriarRegistrosParaConsultaAlunosSemPresenca(5);
        }

        private async Task CriarRegistrosParaConsultaAlunosFreqAbaixo50Porcento(int qdadeRegistros)
        {
            for (int i = 1; i <= qdadeRegistros; i++)
            {
                var idTurmaEvasaoFrequencia = ObterTodos<Dominio.FrequenciaTurmaEvasao>().Count();
                await InserirNaBase(new Dominio.FrequenciaTurmaEvasaoAluno
                {
                    FrequenciaTurmaEvasaoId = idTurmaEvasaoFrequencia,
                    PercentualFrequencia = 40,
                    AlunoCodigo = i.ToString(),
                    AlunoNome = $"Aluno {i}"
                });
            }
        }

        private async Task CriarRegistrosParaConsultaAlunosSemPresenca(int qdadeRegistros)
        {
            for (int i = 1; i <= qdadeRegistros; i++)
            {
                var idTurmaEvasaoFrequencia = ObterTodos<Dominio.FrequenciaTurmaEvasao>().Count();
                await InserirNaBase(new Dominio.FrequenciaTurmaEvasaoAluno
                {
                    FrequenciaTurmaEvasaoId = idTurmaEvasaoFrequencia,
                    PercentualFrequencia = 0,
                    AlunoCodigo = i.ToString(),
                    AlunoNome = $"Aluno {i}"
                });
            }
        }

    }
}
