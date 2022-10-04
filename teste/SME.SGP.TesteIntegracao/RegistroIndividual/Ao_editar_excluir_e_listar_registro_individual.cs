using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_editar_excluir_e_listar_registro_individual : RegistroIndividualTesteBase
    {
        private const string REGISTRO_EDITADO = "Editando registro individual";
        public Ao_editar_excluir_e_listar_registro_individual(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro Individual - Editar registro individual")]
        public async Task Ao_editar_registro_individual()
        {
            var dto = new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil
            };

            await CriarDadosBasicos(dto);
            await CrieRegistroIndividual();

            var useCase = ObterServicoAlterarRegistroIndividualUseCase();
            var dtoAlterar = new AlterarRegistroIndividualDto()
            {
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                TurmaId = TURMA_ID_1,
                Data = DateTimeExtension.HorarioBrasilia().Date,
                Id = 1,
                Registro = REGISTRO_EDITADO
            };

            await useCase.Executar(dtoAlterar);

            var listaDeRegistro = ObterTodos<Dominio.RegistroIndividual>();
            listaDeRegistro.Any().ShouldBeTrue();
            var registro = listaDeRegistro.FirstOrDefault();
            registro.Registro.ShouldBe(REGISTRO_EDITADO);

        }

        [Fact(DisplayName = "Registro Individual - Excluir registro individual")]
        public async Task Ao_excluir_registro_individual()
        {
            var dto = new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil
            };

            await CriarDadosBasicos(dto);
            await CrieRegistroIndividual();

            var useCase = ObterServicoExcluirRegistroUseCase();

            await useCase.Executar(1);

            var listaDeRegistro = ObterTodos<Dominio.RegistroIndividual>();
            listaDeRegistro.Any().ShouldBeTrue();
            var registro = listaDeRegistro.FirstOrDefault();
            registro.Excluido.ShouldBeTrue();
        }

        [Fact(DisplayName = "Registro Individual - Listar registro individual")]
        public async Task Ao_listar_registro_individual()
        {
            var dto = new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil
            };

            await CriarDadosBasicos(dto);
            await CrieRegistroIndividual();

            var filtro = new FiltroRegistroIndividualAlunoData(
                TURMA_ID_1,
                COMPONENTE_CURRICULAR_CODIGO_512,
                long.Parse(ALUNO_CODIGO_1),
                DateTimeExtension.HorarioBrasilia().Date);
            var useCase = ObterServicoListarRegistroIndividualUseCase();
            var registro = await useCase.Executar(filtro);
            registro.ShouldNotBeNull();
            registro.Id.ShouldBe(1);
        }

        private async Task CrieRegistroIndividual()
        {
            await InserirNaBase(new Dominio.RegistroIndividual
            {
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                DataRegistro = DateTimeExtension.HorarioBrasilia().Date,
                Registro = "Teste",
                TurmaId = TURMA_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }
    }
}
