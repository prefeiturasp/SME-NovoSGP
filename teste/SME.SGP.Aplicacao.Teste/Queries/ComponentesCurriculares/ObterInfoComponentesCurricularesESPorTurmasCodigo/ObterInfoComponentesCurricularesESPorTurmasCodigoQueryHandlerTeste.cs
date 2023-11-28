using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ComponentesCurriculares.ObterInfoComponentesCurricularesESPorTurmasCodigo
{
    public class ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerTeste
    {

        private readonly Mock<IMediator> mediator;
        private readonly ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler obterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler;

        public ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            obterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler = new ObterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler(mediator.Object);
        }

        [Fact(DisplayName = "Componentes Curriculares - ObterComponentesCurricularesPorTurmasCodigoComSucesso")]
        public async Task ObterComponentesCurricularesPorTurmasCodigoComSucesso()
        {
            var dre = new Dre() { CodigoDre = "1" };
            var ue = new Ue() { CodigoUe = "1", Dre = dre };

            var turmas = new List<Turma>()
            {
                new Turma() {  CodigoTurma = "1", AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, Ue = ue, ModalidadeCodigo = Modalidade.Fundamental },
                new Turma() {  CodigoTurma = "2", AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, Ue = ue, ModalidadeCodigo = Modalidade.Fundamental }
            };

            var codigosTurmas = turmas.Select(t => t.CodigoTurma).ToArray();

            mediator.Setup(x => x.Send(It.Is<ObterTurmasDreUePorCodigosQuery>(y => y.Codigos == codigosTurmas), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComComponentesQuery>(y => y.TurmaCodigo == turmas[0].CodigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>()
                {
                    TotalPaginas = 1,
                    TotalRegistros = 1,
                    Items = new List<RetornoConsultaListagemTurmaComponenteDto>() { new RetornoConsultaListagemTurmaComponenteDto() { ComponenteCurricularCodigo = 1 } }
                });

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComComponentesQuery>(y => y.TurmaCodigo == turmas[1].CodigoTurma), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>()
               {
                   TotalPaginas = 1,
                   TotalRegistros = 1,
                   Items = new List<RetornoConsultaListagemTurmaComponenteDto>() { new RetornoConsultaListagemTurmaComponenteDto() { ComponenteCurricularCodigo = 2 } }
               });

            mediator.Setup(x => x.Send(It.Is<ObterInfoPedagogicasComponentesCurricularesPorIdsQuery>(y => y.Ids.Contains(1) && y.Ids.Contains(2)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InfoComponenteCurricular>() { new InfoComponenteCurricular(), new InfoComponenteCurricular() }.AsEnumerable());

            var query = new ObterInfoComponentesCurricularesESPorTurmasCodigoQuery(codigosTurmas);

            var resultado = await obterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact(DisplayName = "Componentes Curriculares - ObterComponentesCurricularesPorTurmasCodigoComExcecaoComponenteNaoLocalizadoParaTurma")]
        public async Task ObterComponentesCurricularesPorTurmasCodigoComExcecaoComponenteNaoLocalizadoParaTurma()
        {
            var dre = new Dre() { CodigoDre = "1" };
            var ue = new Ue() { CodigoUe = "1", Dre = dre };

            var turmas = new List<Turma>()
            {
                new Turma() {  CodigoTurma = "1", AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, Ue = ue, ModalidadeCodigo = Modalidade.Fundamental },
                new Turma() {  CodigoTurma = "2", AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, Ue = ue, ModalidadeCodigo = Modalidade.Fundamental }
            };

            var codigosTurmas = turmas.Select(t => t.CodigoTurma).ToArray();

            mediator.Setup(x => x.Send(It.Is<ObterTurmasDreUePorCodigosQuery>(y => y.Codigos == codigosTurmas), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmas);

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComComponentesQuery>(y => y.TurmaCodigo == turmas[0].CodigoTurma), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>()
                {
                    TotalPaginas = 1,
                    TotalRegistros = 1,
                    Items = new List<RetornoConsultaListagemTurmaComponenteDto>() { new RetornoConsultaListagemTurmaComponenteDto() { ComponenteCurricularCodigo = 1 } }
                });

            mediator.Setup(x => x.Send(It.Is<ObterTurmasComComponentesQuery>(y => y.TurmaCodigo == turmas[1].CodigoTurma), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>() { Items = Enumerable.Empty<RetornoConsultaListagemTurmaComponenteDto>() });

            var query = new ObterInfoComponentesCurricularesESPorTurmasCodigoQuery(codigosTurmas);

            Assert.Equal("Não foram retornados items ao obter turmas com componentes. Turma: 2.", 
                (await Assert.ThrowsAsync<NegocioException>(async () => await obterInfoComponentesCurricularesESPorTurmasCodigoQueryHandler.Handle(query, It.IsAny<CancellationToken>()))).Message);            
        }
    }
}
