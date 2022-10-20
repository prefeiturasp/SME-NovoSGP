using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.DiarioBordo
{
    public class Ao_excluir_por_aula : DiarioBordoTesteBase
    {
        public Ao_excluir_por_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_excluir_diario_bordo()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            { 
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                ContemObservacoes = false,
                ContemDevolutiva = false
            };
            
            await CriarDadosBasicos(filtroDiarioBordo);

            //-> Avalidar dados criados
            var diariosBordos = ObterTodos<Dominio.DiarioBordo>();
            diariosBordos.ShouldNotBeNull();
            
            var diarioBordo = diariosBordos.FirstOrDefault();
            diarioBordo.ShouldNotBeNull();

            //-> Executar
            var msgRabbit = MontarMensagemRabbit(diarioBordo.AulaId);
            msgRabbit.ShouldNotBeNull();
            var useCase = ObterExcluirAulaUseCase();
            await useCase.Executar(msgRabbit);
            
            //-> Avaliar pós executar
            diariosBordos = ObterTodos<Dominio.DiarioBordo>();
            diarioBordo.ShouldNotBeNull();

            foreach (var item in diariosBordos)
                item.Excluido.ShouldBeTrue();
        }
        
        [Fact]
        public async Task Deve_excluir_diario_bordo_e_observacoes()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            { 
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138, 
                ContemObservacoes = true,
                ContemDevolutiva = false
            };
            
            await CriarDadosBasicos(filtroDiarioBordo);

            //-> Avalidar dados criados
            var diariosBordos = ObterTodos<Dominio.DiarioBordo>();
            diariosBordos.ShouldNotBeNull();

            var diarioBordo = diariosBordos.FirstOrDefault();
            diarioBordo.ShouldNotBeNull();

            var diarioBordoId = diarioBordo.Id;
            var observacoes = ObterTodos<DiarioBordoObservacao>().Where(c => c.DiarioBordoId == diarioBordoId);
            observacoes.ShouldNotBeNull();

            //-> Executar
            var msgRabbit = MontarMensagemRabbit(diarioBordo.AulaId);
            msgRabbit.ShouldNotBeNull();
            var useCase = ObterExcluirAulaUseCase();
            await useCase.Executar(msgRabbit);
            
            //-> Avaliar pós executar
            diariosBordos = ObterTodos<Dominio.DiarioBordo>();
            diariosBordos.ShouldNotBeNull();

            foreach (var item in diariosBordos)
            {
                item.Excluido.ShouldBeTrue();
                
                observacoes = ObterTodos<DiarioBordoObservacao>().Where(c => c.DiarioBordoId == item.Id).ToList();
                observacoes.ShouldNotBeNull();

                foreach (var obs in observacoes)
                    obs.Excluido.ShouldBeTrue();                
            }
        }        

        private IExcluirDiarioBordoPorAulaIdUseCase ObterExcluirAulaUseCase()
        {
            return ServiceProvider.GetService<IExcluirDiarioBordoPorAulaIdUseCase>();
        }
        
        private static MensagemRabbit MontarMensagemRabbit(long id)
        {
            var msg = JsonConvert.SerializeObject(new FiltroIdDto(id));
            return new MensagemRabbit(msg);
        }        
    }
}