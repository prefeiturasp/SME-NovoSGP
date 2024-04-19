using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.MapeamentoEstudantes.Base;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes
{
    [Collection(nameof(MapeamentoEstudantesFixture))]
    public class Ao_obter_mapeamento_estudante_novo 
    {
        private readonly MapeamentoEstudantesFixture _fixture;

        public Ao_obter_mapeamento_estudante_novo(MapeamentoEstudantesFixture collectionFixture)
        {
            _fixture = collectionFixture;
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador()
        {
            var idMapeamento = await _fixture.InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TesteBaseConstantes.TURMA_ID_1,
                AlunoCodigo = TesteBaseConstantes.ALUNO_CODIGO_1,
                AlunoNome = "Nome do aluno 1",
                Bimestre = TesteBaseConstantes.BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var questoes = _fixture.ObterTodos<Questao>();

            var secaoMapeamento = _fixture.ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await _fixture.InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var useCase = _fixture.ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(TesteBaseConstantes.ALUNO_CODIGO_1, TesteBaseConstantes.TURMA_ID_1, TesteBaseConstantes.BIMESTRE_1);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno2()
        {
            var idMapeamento = await _fixture.InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TesteBaseConstantes.TURMA_ID_1,
                AlunoCodigo = TesteBaseConstantes.ALUNO_CODIGO_2,
                AlunoNome = "Nome do aluno 2",
                Bimestre = TesteBaseConstantes.BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var questoes = _fixture.ObterTodos<Questao>();

            var secaoMapeamento = _fixture.ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await _fixture.InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var useCase = _fixture.ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(TesteBaseConstantes.ALUNO_CODIGO_2, TesteBaseConstantes.TURMA_ID_1, TesteBaseConstantes.BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno3()
        {
            var idMapeamento = await _fixture.InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TesteBaseConstantes.TURMA_ID_1,
                AlunoCodigo = TesteBaseConstantes.ALUNO_CODIGO_3,
                AlunoNome = "Nome do aluno 3",
                Bimestre = TesteBaseConstantes.BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var questoes = _fixture.ObterTodos<Questao>();

            var secaoMapeamento = _fixture.ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await _fixture.InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var useCase = _fixture.ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(TesteBaseConstantes.ALUNO_CODIGO_3, TesteBaseConstantes.TURMA_ID_1, TesteBaseConstantes.BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno4()
        {
            var idMapeamento = await _fixture.InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TesteBaseConstantes.TURMA_ID_1,
                AlunoCodigo = TesteBaseConstantes.ALUNO_CODIGO_4,
                AlunoNome = "Nome do aluno 4",
                Bimestre = TesteBaseConstantes.BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var questoes = _fixture.ObterTodos<Questao>();

            var secaoMapeamento = _fixture.ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await _fixture.InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var useCase = _fixture.ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(TesteBaseConstantes.ALUNO_CODIGO_4, TesteBaseConstantes.TURMA_ID_1, TesteBaseConstantes.BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }

        [Fact(DisplayName = "Mapeamento de estudante - Obter identificador do mapeamento estudante")]
        public async Task Ao_obter_identificador_aluno5()
        {
            var idMapeamento = await _fixture.InserirNaBaseAsync(new MapeamentoEstudante()
            {
                TurmaId = TesteBaseConstantes.TURMA_ID_1,
                AlunoCodigo = TesteBaseConstantes.ALUNO_CODIGO_5,
                AlunoNome = "Nome do aluno 5",
                Bimestre = TesteBaseConstantes.BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var questoes = _fixture.ObterTodos<Questao>();

            var secaoMapeamento = _fixture.ObterTodos<SecaoMapeamentoEstudante>().FirstOrDefault();

            await _fixture.InserirNaBaseAsync(new MapeamentoEstudanteSecao()
            {
                MapeamentoEstudanteId = idMapeamento,
                SecaoMapeamentoEstudanteId = secaoMapeamento.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = TesteBaseConstantes.SISTEMA_NOME,
                CriadoRF = TesteBaseConstantes.SISTEMA_CODIGO_RF
            });

            var useCase = _fixture.ServiceProvider.GetService<IObterIdentificadorMapeamentoEstudanteUseCase>();
            var identificador = await useCase.Executar(TesteBaseConstantes.ALUNO_CODIGO_5, TesteBaseConstantes.TURMA_ID_1, TesteBaseConstantes.BIMESTRE_2);

            identificador.ShouldBe(idMapeamento);
        }
    }
}
