using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_registrar_aula_unica : TesteBase
    {
        private readonly AulaBuilder _builder;
        public Ao_registrar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            _builder = new AulaBuilder(this);
        }

        [Fact]
        public async Task Ao_registrar_aula_unica_professor_especialista()
        {
            await CarregueBaseEspecialista();

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var dto = new PersistirAulaDto()
            {
                CodigoTurma = "1",
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                DataAula = new DateTime(2022, 02, 10),
                DisciplinaCompartilhadaId = 1106,
                CodigoUe = "1",
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                CodigoComponenteCurricular = 1106,
                NomeComponenteCurricular = "português",
                TipoCalendarioId = 1
            };

            var retorno = await useCase.Executar(dto);

            retorno.ShouldNotBeNull();

            var lista = ObterTodos<Aula>();

            lista.ShouldNotBeEmpty();
            lista.Count().ShouldBeGreaterThanOrEqualTo(1);
        }

        private async Task CarregueBaseEspecialista()
        {
            await _builder.CriaItensComuns();
            _builder.CriarClaimUsuario(_builder.ObtenhaPerfilEspecialista());
            await _builder.CriaUsuario();
            await _builder.CriaTipoCalendario();
            await _builder.CriaTurmaFundamental();
        }
    }
}
