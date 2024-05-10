using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterTurmasConsideradasNoConselhoQueryHandlerFake
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterTurmasConsideradasNoConselhoQueryHandler query;
        public ObterTurmasConsideradasNoConselhoQueryHandlerFake()
        {
            mediator = new Mock<IMediator>();
            query = new ObterTurmasConsideradasNoConselhoQueryHandler(mediator.Object);
        }


        [Fact(DisplayName = "ObterTurmasConsideradasNoConselhoQueryHandler -  Obter Turmas consideradas para serem utilizadas no conselho de classe")]
        public async Task Deve_Obter_Somente_uma_turma_regular_e_uma_ed_fisica_EJA()
        {
            var ano = DateTimeExtension.HorarioBrasilia().Year;
            var listaTurmas = new List<Turma>()
            {
                new Turma()
                    {
                        Nome = "Turma Teste 1",
                        CodigoTurma = "1",
                        Id = 1,
                        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                        ModalidadeCodigo = Modalidade.EJA,
                    },
                 new Turma()
                    {
                        Nome = "Turma Teste 2",
                        CodigoTurma = "2",
                        Id = 2,
                        TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                        ModalidadeCodigo = Modalidade.EJA,
                    },
                  new Turma()
                    {
                        Nome = "Turma Teste 3",
                        CodigoTurma = "3",
                        Id = 3,
                        TipoTurma = Dominio.Enumerados.TipoTurma.EdFisica,
                        ModalidadeCodigo = Modalidade.EJA,
                    },
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmasPorCodigosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaTurmas);


            var retornoConsulta = await query.Handle(new ObterTurmasConsideradasNoConselhoQuery(listaTurmas.Select(t=> t.CodigoTurma), listaTurmas.FirstOrDefault(l=> l.CodigoTurma == "1")), new CancellationToken());

            Assert.NotNull(retornoConsulta);
            Assert.True(retornoConsulta.Count() == 2);
            Assert.True(retornoConsulta.FirstOrDefault() == "1");
            Assert.Contains("3",retornoConsulta);
        }
    }
}
