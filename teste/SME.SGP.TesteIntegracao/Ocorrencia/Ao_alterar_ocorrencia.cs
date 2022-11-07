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

            var obterTodos = ObterTodos<Dominio.Ocorrencia>();
            obterTodos.ShouldNotBeNull();
            

             var obterOcorrenciaUseCase = ObterOcorrenciaUseCase();
             var ocorrencia = await obterOcorrenciaUseCase.Executar(obterTodos.FirstOrDefault()!.Id);
             
             var alterarDto = new AlterarOcorrenciaDto
             {
                 Id = obterTodos.FirstOrDefault()!.Id,
                 UeId = ocorrencia.UeId,
                 Modalidade = 5,
             };
             //
             //
             // var alterarUseCase = AlterarOcorrenciaUseCase();
             // var alterar = await alterarUseCase.Executar(alterarDto);

        }
    }
}