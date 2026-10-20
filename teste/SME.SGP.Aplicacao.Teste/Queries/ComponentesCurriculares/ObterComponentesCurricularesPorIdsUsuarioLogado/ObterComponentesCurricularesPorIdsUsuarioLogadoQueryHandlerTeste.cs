using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorIdsUsuarioLogado
{
    public class ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandlerTeste
    {
        [Fact(DisplayName = "Componentes curriculares - Deve priorizar componente com codigo exato ao agrupamento equivalente")]
        public async Task DevePriorizarComponenteComCodigoExatoAoAgrupamentoEquivalente()
        {
            const long codigoClubeDaLeitura = 1521;
            const string codigoTurma = "3030568";

            var usuario = new Usuario
            {
                Login = "8098468",
                PerfilAtual = Perfis.PERFIL_POSL
            };
            var turma = new Turma
            {
                CodigoTurma = codigoTurma,
                ModalidadeCodigo = Modalidade.Fundamental
            };
            var componentesEol = new List<ComponenteCurricularEol>
            {
                new()
                {
                    Codigo = 812547,
                    CodigoComponenteTerritorioSaber = 1217,
                    Descricao = "I -COMUNICAÇÃO E NOVAS LINGUAGENS - ACADEMIA ESTUDANTIL DE LETRAS",
                    TerritorioSaber = true,
                    TurmaCodigo = codigoTurma,
                    CodigosTerritoriosAgrupamento = new long[] { 1217, codigoClubeDaLeitura }
                },
                new()
                {
                    Codigo = codigoClubeDaLeitura,
                    CodigoComponenteTerritorioSaber = codigoClubeDaLeitura,
                    Descricao = "I -COMUNICAÇÃO E NOVAS LINGUAGENS - CLUBE DA LEITURA",
                    TerritorioSaber = true,
                    TurmaCodigo = codigoTurma,
                    CodigosTerritoriosAgrupamento = new long[0]
                }
            };
            var componentesSgp = new List<DisciplinaDto>
            {
                new()
                {
                    CodigoComponenteCurricular = codigoClubeDaLeitura,
                    CodigoComponenteCurricularTerritorioSaber = codigoClubeDaLeitura,
                    Nome = "CLUBE DA LEITURA",
                    TerritorioSaber = true
                }
            };

            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);
            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesDoProfessorNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesEol);
            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesSgp);

            var handler = new ObterComponentesCurricularesPorIdsUsuarioLogadoQueryHandler(mediator.Object);

            var resultado = (await handler.Handle(
                new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new[] { codigoClubeDaLeitura }, codigoTurma),
                CancellationToken.None)).Single();

            Assert.Equal(codigoClubeDaLeitura, resultado.CodigoComponenteCurricular);
            Assert.Equal("I -COMUNICAÇÃO E NOVAS LINGUAGENS - CLUBE DA LEITURA", resultado.Nome);
        }
    }
}
