using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaUnica
{
    public class Ao_gerar_aula_com_auditoria_administrador : TesteBase
    {
        private ItensBasicosBuilder _buider;
        public Ao_gerar_aula_com_auditoria_administrador(CollectionFixture testFixture) : base(testFixture)
        {
            _buider = new ItensBasicosBuilder(this);
        }

        [Fact]
        public async Task Deve_gravar_aula_com_auditoria_para_administrador()
        {

            await _buider.CriaItensComunsEja(true);

            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();

            var dto = new PersistirAulaDto()
            {
                CodigoTurma = "1",
                CodigoComponenteCurricular = 1106,
                DataAula = new (DateTimeExtension.HorarioBrasilia().Year, 02, 10),
                Quantidade = 1,
                CodigoUe = "1",
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                NomeComponenteCurricular = "teste"
            };

            var retorno = await useCase.Executar(dto);

            Assert.IsType<RetornoBaseDto>(retorno);

            var listaDeAuditoria = ObterTodos<Auditoria>();

            listaDeAuditoria.ShouldNotBeEmpty();
            listaDeAuditoria.Exists(auditorio => auditorio.Administrador == "7924488").ShouldBeTrue();
        }
    }
}
