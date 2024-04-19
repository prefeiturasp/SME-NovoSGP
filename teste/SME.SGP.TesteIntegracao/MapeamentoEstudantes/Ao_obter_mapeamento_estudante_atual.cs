using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{

    public class Ao_obter_mapeamento_estudante_atual : MapeamentoBase
    {
        public Ao_obter_mapeamento_estudante_atual(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            Task.Run(() => CriarDadosBase()).Wait();
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
        public async Task Ao_obter_identificador2()
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
        public async Task Ao_obter_identificador6()
        {
            var idMapeamento = await  InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes =  ObterTodos<Questao>();

            var secaoMapeamento =  ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await  InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase =  ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_1);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador3()
        {
            var idMapeamento = await  InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes =  ObterTodos<Questao>();

            var secaoMapeamento =  ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await  InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase =  ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_1);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador4()
        {
            var idMapeamento = await  InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var questoes =  ObterTodos<Questao>();

            var secaoMapeamento =  ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await  InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase =  ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(ALUNO_CODIGO_1, TURMA_ID_1, BIMESTRE_1);

            identificador.ShouldBe(idMapeamento);
        }
    }
}
