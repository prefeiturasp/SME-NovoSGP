using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    public class Ao_obter_mapeamento_estudante_novo : MapeamentoBase
    {

        public Ao_obter_mapeamento_estudante_novo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            if (!collectionFixture.DatabasePublicado)
            {
                Task.Run(() => CriarDadosBase()).Wait();
                collectionFixture.DatabasePublicado = true;
            }
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador()
        {
            var idMapeamento = await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_1);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno2()
        {
            var idMapeamento = await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNome = "Nome do aluno 2",
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_2, TURMA_ID_1, BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno3()
        {
            var idMapeamento = await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_3,
                AlunoNome = "Nome do aluno 3",
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_3, TURMA_ID_1, BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno4()
        {
            var idMapeamento = await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_4,
                AlunoNome = "Nome do aluno 4",
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_4, TURMA_ID_1, BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno5()
        {
            var idMapeamento = await InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_5,
                AlunoNome = "Nome do aluno 5",
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes = ObterTodos<Questao>();

            var secaoMapeamento = ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_5, TURMA_ID_1, BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }
    }
}