using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Ocorrencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Ocorrencia
{
    public class Ao_alterar_ocorrencia : OcorrenciaTesteBase
    {
        public Ao_alterar_ocorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        [Fact(DisplayName = "Ocorrencia - Alterar Descricao contendo palavra tempo")]
        public async Task AlterarOcorrenciaDescricaoContendoPalavraTempo()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = 1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "tempo, Tempo, TEMPO",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);

            dtoIncluir.ShouldNotBeNull();
            dtoIncluir.Descricao.ShouldBeEquivalentTo("tempo, Tempo, TEMPO");

        }
        [Fact(DisplayName = "Ocorrencia - Alterar Ocorrencia com Turma")]
        public async Task AlterarOcorrenciaComTurma()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = 1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "Lorem Ipsum é simplesmente uma simulação de texto da",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);
            await ValidarAlteracao(dtoIncluir);

        }
        
        [Fact(DisplayName = "Ocorrencia - Alterar Ocorrencia Com Ue sem Turma")]
        public async Task AlterarOcorrenciaComUeSemTurma()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = null,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "Lorem Ipsum é simplesmente uma simulação de texto da",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);
            await ValidarAlteracao(dtoIncluir);
        }
        
        [Fact(DisplayName = "Ocorrencia - Alterar Ocorrencia Com Turma e Aluno")]
        public async Task AlterarOcorrenciaComTurmaEAluno()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = 1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "Lorem Ipsum é simplesmente uma simulação de texto da",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
                CodigosAlunos = new List<long>(){1,2}
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);
            
            await ValidarAlteracao(dtoIncluir,temAlunos:true);
        }

        [Fact(DisplayName = "Ocorrencia - Alterar Ocorrencia Com Ue e Servidor")]
        public async Task AlterarOcorrenciaComUeEServidor()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = 1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "Lorem Ipsum é simplesmente uma simulação de texto da",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
                CodigosServidores = new List<string>(){"rf1","rf2"}
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);
            
            await ValidarAlteracao(dtoIncluir,temServidor:true);
        }
        
        [Fact(DisplayName = "Ocorrencia - Alterar Ocorrencia Com Turma, Aluno e Servidor")]
        public async Task AlterarOcorrenciaComTurmaAlunoEServidor()
        {
            await CriarDadosBasicos();
            var dtoIncluir = new InserirOcorrenciaDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DreId = 1,
                UeId = 1,
                Modalidade = 5,
                Semestre = 3,
                TurmaId = 1,
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Titulo = "Lorem ipsum",
                Descricao = "Lorem Ipsum é simplesmente uma simulação de texto da",
                OcorrenciaTipoId = 1,
                HoraOcorrencia = "17:34",
                CodigosServidores = new List<string>(){"rf1","rf2"},
                CodigosAlunos = new List<long>(){1,2}
            };
            var useCaseIncluir = InserirOcorrenciaUseCase();
            await useCaseIncluir.Executar(dtoIncluir);
            
            await ValidarAlteracao(dtoIncluir,true,true);
        }
        private async Task ValidarAlteracao(InserirOcorrenciaDto dtoIncluir,bool temAlunos = false,bool temServidor = false)
        {
            var obterTodos = ObterTodos<Dominio.Ocorrencia>();
            obterTodos.ShouldNotBeNull();


            var obterOcorrenciaUseCase = ObterOcorrenciaUseCase();
            var ocorrencia = await obterOcorrenciaUseCase.Executar(obterTodos.FirstOrDefault()!.Id);

            var alterarDto = new AlterarOcorrenciaDto
            {
                Id = obterTodos.FirstOrDefault()!.Id,
                UeId = ocorrencia.UeId,
                OcorrenciaTipoId = 2,
                DataOcorrencia = dtoIncluir.DataOcorrencia,
                Descricao = "Descricao Alterada",
                Titulo = "Titulo Alterado",
                CodigosAlunos = temAlunos ? new List<long>(){1,2} : new List<long>(),
                CodigosServidores = temServidor ? new List<string>(){"1","2"} : new List<string>()
            };
            var alterarUseCase = AlterarOcorrenciaUseCase();
            await alterarUseCase.Executar(alterarDto);

            var obterTodosAlterados = ObterTodos<Dominio.Ocorrencia>();
            obterTodosAlterados.ShouldNotBeNull();
            obterTodosAlterados.FirstOrDefault()?.Titulo.ShouldBeEquivalentTo(alterarDto.Titulo);
            obterTodosAlterados.FirstOrDefault()?.Descricao.ShouldBeEquivalentTo(alterarDto.Descricao);
            obterTodosAlterados.FirstOrDefault()?.OcorrenciaTipoId.ShouldBeEquivalentTo(alterarDto.OcorrenciaTipoId);
        }
    }
}