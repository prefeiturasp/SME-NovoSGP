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
    public class Ao_excluir_ocorrencia : OcorrenciaTesteBase
    {
        public Ao_excluir_ocorrencia(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Ocorrencia - Excluir Ocorrencia com Turma")]
        public async Task ExcluirOcorrenciaComTurma()
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

            var obterTodasOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrencias.ShouldNotBeNull();
            obterTodasOcorrencias.Count.ShouldBeEquivalentTo(1);


            var useCaseExcluir = ExcluirOcorrenciaUseCase();
            await useCaseExcluir.Executar(obterTodasOcorrencias.Select(x => x.Id));

            var obterTodasOcorrenciasAposExcluir = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrenciasAposExcluir.ShouldNotBeNull();
            obterTodasOcorrenciasAposExcluir.Count(x => !x.Excluido).ShouldBeEquivalentTo(0);
        }

        [Fact(DisplayName = "Ocorrencia - Excluir Ocorrencia Com Ue sem Turma")]
        public async Task ExcluirOcorrenciaComUeSemTurma()
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
            
            var obterTodasOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            var useCaseExcluir = ExcluirOcorrenciaUseCase();
            await useCaseExcluir.Executar(obterTodasOcorrencias.Select(x => x.Id));

            var obterTodasOcorrenciasAposExcluir = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrenciasAposExcluir.ShouldNotBeNull();
            obterTodasOcorrenciasAposExcluir.Count(x => !x.Excluido).ShouldBeEquivalentTo(0);
        }

        [Fact(DisplayName = "Ocorrencia - Excluir Ocorrencia Com Turma e Aluno")]
        public async Task ExcluirOcorrenciaComTurmaEAluno()
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
            
            var obterTodasOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            var useCaseExcluir = ExcluirOcorrenciaUseCase();
            await useCaseExcluir.Executar(obterTodasOcorrencias.Select(x => x.Id));

            var obterTodasOcorrenciasAposExcluir = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrenciasAposExcluir.ShouldNotBeNull();
            obterTodasOcorrenciasAposExcluir.Count(x => !x.Excluido).ShouldBeEquivalentTo(0);
        }
        
        
        [Fact(DisplayName = "Ocorrencia - Excluir Ocorrencia Com Ue e Servidor")]
        public async Task ExcluirOcorrenciaComUeEServidor()
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
            
            var obterTodasOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            var useCaseExcluir = ExcluirOcorrenciaUseCase();
            await useCaseExcluir.Executar(obterTodasOcorrencias.Select(x => x.Id));

            var obterTodasOcorrenciasAposExcluir = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrenciasAposExcluir.ShouldNotBeNull();
            obterTodasOcorrenciasAposExcluir.Count(x => !x.Excluido).ShouldBeEquivalentTo(0);
        }
        
        [Fact(DisplayName = "Ocorrencia - Excluir Ocorrencia Com Turma, Aluno e Servidor")]
        public async Task ExcluirOcorrenciaComTurmaAlunoEServidor()
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
            
            var obterTodasOcorrencias = ObterTodos<Dominio.Ocorrencia>();
            var useCaseExcluir = ExcluirOcorrenciaUseCase();
            await useCaseExcluir.Executar(obterTodasOcorrencias.Select(x => x.Id));

            var obterTodasOcorrenciasAposExcluir = ObterTodos<Dominio.Ocorrencia>();
            obterTodasOcorrenciasAposExcluir.ShouldNotBeNull();
            obterTodasOcorrenciasAposExcluir.Count(x => !x.Excluido).ShouldBeEquivalentTo(0);
        }
    }
}