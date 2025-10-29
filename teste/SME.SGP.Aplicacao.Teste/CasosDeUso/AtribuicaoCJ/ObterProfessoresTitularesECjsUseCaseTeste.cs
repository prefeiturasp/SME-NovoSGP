
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtribuicaoCJ
{
    public class ObterProfessoresTitularesECjsUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterProfessoresTitularesECjsUseCase useCase;

        public ObterProfessoresTitularesECjsUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterProfessoresTitularesECjsUseCase(mediatorMock.Object);
        }

        [Fact(Skip = "Teste precisa ser revisado")]
        public async Task Executar_Quando_ProfessoresEol_Existir_E_Atribuicoes_Existir_Deve_Retornar_Dto_Correto()
        {
            var dataReferenciaEsperada = new DateTime(2025, 10, 28, 0, 0, 0, DateTimeKind.Utc);
            var professorTitularEol = new ProfessorTitularDisciplinaEol
            {
                CodigosDisciplinas = "100",
                DisciplinaNome = "Matemática",
                ProfessorNome = "Prof Titular",
                ProfessorRf = "12345"
            };
            var listaProfessoresTitulares = new List<ProfessorTitularDisciplinaEol> { professorTitularEol };

            var atribuicao = new Dominio.AtribuicaoCJ
            {
                DisciplinaId = 100,
                Substituir = true,
                CriadoEm = dataReferenciaEsperada.AddDays(-5),
                CriadoPor = "Usuário Criador",
                AlteradoEm = dataReferenciaEsperada.AddDays(-1),
                AlteradoPor = "Usuário Alterador"
            };
            var listaAtribuicoes = new List<Dominio.AtribuicaoCJ> { atribuicao };

            mediatorMock.Setup(x => x.Send(It.Is<ObterProfessoresTitularesDisciplinasEolQuery>(q => q.DataReferencia == dataReferenciaEsperada), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaProfessoresTitulares);
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAtribuicoes);

            var resultado = await useCase.Executar("UE", "TURMA", "RF", Modalidade.EducacaoInfantil, 2025);

            //Assert.NotNull(resultado);
            //Assert.True(resultado.Itens.First().Substituir);
           // Assert.Equal(atribuicao.CriadoEm, resultado.CriadoEm);
            //Assert.Equal(atribuicao.CriadoPor, resultado.CriadoPor);
        }

        [Fact]
        public async Task Executar_Quando_ProfessoresEol_Existir_E_Ano_Nao_Atual_Deve_Retornar_Dto_Correto()
        {
            var anoLetivo = 2024;
            var dataReferenciaEsperada = new DateTime(anoLetivo, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            var listaProfessoresTitulares = new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { CodigosDisciplinas = "100" } };
            var listaAtribuicoes = new List<Dominio.AtribuicaoCJ>();

            mediatorMock.Setup(x => x.Send(It.Is<ObterProfessoresTitularesDisciplinasEolQuery>(q => q.DataReferencia == dataReferenciaEsperada), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaProfessoresTitulares);
            mediatorMock.Setup(x => x.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaAtribuicoes);

            var resultado = await useCase.Executar("UE", "TURMA", "RF", Modalidade.EducacaoInfantil, anoLetivo);

            Assert.NotNull(resultado);
        }

        [Fact]
        public async Task Executar_Quando_ProfessoresEol_Nao_Existir_Deve_Retornar_Null()
        {
            var listaProfessoresTitulares = new List<ProfessorTitularDisciplinaEol>();

            mediatorMock.Setup(x => x.Send(It.IsAny<ObterProfessoresTitularesDisciplinasEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaProfessoresTitulares);

            var resultado = await useCase.Executar("UE", "TURMA", "RF", Modalidade.EducacaoInfantil, 2025);

            Assert.Null(resultado);
        }

        [Fact]
        public void TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno_Quando_Atribuicoes_E_Professores_Existir_Deve_Mapear_Corretamente()
        {
            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { CodigosDisciplinas = "101", DisciplinaNome = "Português", ProfessorNome = "Prof A", ProfessorRf = "111" },
                new ProfessorTitularDisciplinaEol { CodigosDisciplinas = "102", DisciplinaNome = "História", ProfessorNome = "Prof B", ProfessorRf = "222" }
            };
            var listaAtribuicoes = new List<Dominio.AtribuicaoCJ>
            {
                new Dominio.AtribuicaoCJ { DisciplinaId = 101, Substituir = true, CriadoEm = new DateTime(2025, 1, 1), CriadoPor = "C1", AlteradoEm = new DateTime(2025, 1, 10), AlteradoPor = "A1" },
                new Dominio.AtribuicaoCJ { DisciplinaId = 102, Substituir = false, CriadoEm = new DateTime(2025, 1, 5), CriadoPor = "C2" }
            };

            var metodo = useCase.GetType().GetMethod("TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultado = (AtribuicaoCJTitularesRetornoDto)metodo.Invoke(useCase, new object[] { listaAtribuicoes, professoresTitulares });

            Assert.Equal(2, resultado.Itens.Count);
            Assert.True(resultado.Itens.First(i => i.DisciplinaId == 101).Substituir);
            Assert.False(resultado.Itens.First(i => i.DisciplinaId == 102).Substituir);
            Assert.Equal("Prof B", resultado.Itens.First(i => i.DisciplinaId == 102).ProfessorTitular);
            Assert.Equal(new DateTime(2025, 1, 1), resultado.CriadoEm);
            Assert.Equal("C1", resultado.CriadoPor);
            Assert.Equal(new DateTime(2025, 1, 10), resultado.AlteradoEm);
            Assert.Equal("A1", resultado.AlteradoPor);
        }

        [Fact]
        public void TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno_Quando_Nao_Houver_Atribuicoes_Deve_Retornar_Apenas_Itens()
        {
            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { CodigosDisciplinas = "101", DisciplinaNome = "Português", ProfessorNome = "Prof A", ProfessorRf = "111" }
            };
            var listaAtribuicoes = new List<Dominio.AtribuicaoCJ>();

            var metodo = useCase.GetType().GetMethod("TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultado = (AtribuicaoCJTitularesRetornoDto)metodo.Invoke(useCase, new object[] { listaAtribuicoes, professoresTitulares });

            Assert.Equal(1, resultado.Itens.Count);
            Assert.Equal(default, resultado.CriadoEm);
            Assert.Null(resultado.CriadoPor);
            Assert.Null(resultado.AlteradoEm);
            Assert.Null(resultado.AlteradoPor);
        }

        [Fact]
        public void TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno_Quando_AlteradoEm_Mais_Recente_Deve_Usar_Registro_Alterado()
        {
            var professoresTitulares = new List<ProfessorTitularDisciplinaEol>
            {
                new ProfessorTitularDisciplinaEol { CodigosDisciplinas = "101", DisciplinaNome = "Português", ProfessorNome = "Prof A", ProfessorRf = "111" }
            };
            var atribuicao1 = new Dominio.AtribuicaoCJ { DisciplinaId = 101, CriadoEm = new DateTime(2025, 1, 15), CriadoPor = "C15" };
            var atribuicao2 = new Dominio.AtribuicaoCJ { DisciplinaId = 102, CriadoEm = new DateTime(2025, 1, 1), CriadoPor = "C1", AlteradoEm = new DateTime(2025, 1, 20), AlteradoPor = "A20" };
            var listaAtribuicoes = new List<Dominio.AtribuicaoCJ> { atribuicao1, atribuicao2 };

            var metodo = useCase.GetType().GetMethod("TransformaEntidadesEmDtosAtribuicoesProfessoresRetorno", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var resultado = (AtribuicaoCJTitularesRetornoDto)metodo.Invoke(useCase, new object[] { listaAtribuicoes, professoresTitulares });

            Assert.Equal(new DateTime(2025, 1, 1), resultado.CriadoEm);
            Assert.Equal("C1", resultado.CriadoPor);
            Assert.Equal(new DateTime(2025, 1, 20), resultado.AlteradoEm);
            Assert.Equal("A20", resultado.AlteradoPor);
        }


        [Fact]
        public void VerificaTitularidadeDisciplinaInfantil_Quando_Professor_Encontrado_Deve_Adicionar_Professor_E_Disciplina()
        {
            var disciplina1 = new DisciplinaDto { Id = 1, CodigoComponenteCurricular = 101, NomeComponenteInfantil = "Arte" };
            var disciplinas = new List<DisciplinaDto> { disciplina1 };
            var professorTitularEol = new ProfessorTitularDisciplinaEol
            {
                CodigosDisciplinas = "101,102",
                DisciplinaNome = "Infantil",
                ProfessorNome = "Prof Infantil",
                ProfessorRf = "999"
            };
            var professoresTitularesEol = new List<ProfessorTitularDisciplinaEol> { professorTitularEol };

            var resultado = useCase.VerificaTitularidadeDisciplinaInfantil(professoresTitularesEol, disciplinas);

            Assert.Equal(1, resultado.Count);
            Assert.Equal("1", resultado.First().CodigosDisciplinas);
            Assert.Equal("Arte", resultado.First().DisciplinaNome);
            Assert.Equal("Prof Infantil", resultado.First().ProfessorNome);
            Assert.Equal("999", resultado.First().ProfessorRf);
        }

        [Fact]
        public void VerificaTitularidadeDisciplinaInfantil_Quando_Professor_Nao_Encontrado_Deve_Adicionar_Sem_Titular()
        {
            var disciplina1 = new DisciplinaDto { Id = 1, CodigoComponenteCurricular = 101, NomeComponenteInfantil = "Arte" };
            var disciplina2 = new DisciplinaDto { Id = 2, CodigoComponenteCurricular = 103, NomeComponenteInfantil = "Música" };
            var disciplinas = new List<DisciplinaDto> { disciplina1, disciplina2 };
            var professorTitularEol = new ProfessorTitularDisciplinaEol
            {
                CodigosDisciplinas = "101",
                DisciplinaNome = "Infantil",
                ProfessorNome = "Prof Infantil",
                ProfessorRf = "999"
            };
            var professoresTitularesEol = new List<ProfessorTitularDisciplinaEol> { professorTitularEol };

            var resultado = useCase.VerificaTitularidadeDisciplinaInfantil(professoresTitularesEol, disciplinas);

            Assert.Equal(2, resultado.Count);
            Assert.Equal("Prof Infantil", resultado.First(r => r.CodigosDisciplinas == "1").ProfessorNome);
            Assert.Equal("Não há professor titular", resultado.First(r => r.CodigosDisciplinas == "2").ProfessorNome);
            Assert.Equal("", resultado.First(r => r.CodigosDisciplinas == "2").ProfessorRf);
        }
    }
}
