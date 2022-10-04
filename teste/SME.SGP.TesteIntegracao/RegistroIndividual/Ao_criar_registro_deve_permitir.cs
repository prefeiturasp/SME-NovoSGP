using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_criar_registro_deve_permitir : RegistroIndividualTesteBase
    {
        public Ao_criar_registro_deve_permitir(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }


        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual na data atual")]
        public async Task Cadastrar_registro_individual_na_data_atual()
        {
            var useCase = InserirRegistroIndividualUseCase();
            
            var obterResgistros = ObterTodos<Dominio.RegistroIndividual>();
            obterResgistros.Count.ShouldBeEquivalentTo(0);
            
            var filtro = new FiltroRegistroIndividualDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil
            };
            await CriarDadosBasicos(filtro);
            
            var registroParaSalvar = new InserirRegistroIndividualDto
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                AlunoCodigo = NUMERO_LONGO_1,
                Registro = "<pre><span>Registro de teste</span></pre>",
                Data = DateTime.Now
            };
            
            var inserirRegistro = await useCase.Executar(registroParaSalvar);
            inserirRegistro.ShouldNotBeNull();
            
            var totalResgistros = ObterTodos<Dominio.RegistroIndividual>();
            totalResgistros.ShouldNotBeNull();
            totalResgistros.Count.ShouldBeEquivalentTo(1);
            
            totalResgistros.FirstOrDefault()?.Id.ShouldBeGreaterThan(0);
            totalResgistros.FirstOrDefault()?.AlunoCodigo.ShouldBeEquivalentTo(registroParaSalvar.AlunoCodigo);
            totalResgistros.FirstOrDefault()?.Registro.ShouldBeEquivalentTo(registroParaSalvar.Registro);
            totalResgistros.FirstOrDefault()?.DataRegistro.Date.ShouldBeEquivalentTo(registroParaSalvar.Data.Date);
        }
        
    }
}