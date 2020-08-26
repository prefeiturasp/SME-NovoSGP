using MediatR;
using SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Servicos
{
    //public class ServicoGithubTeste
    //{
    //    private readonly IMediator mediator;

    //    public ServicoGithubTeste(IMediator _mediator)
    //    {
    //        this.mediator = _mediator;
    //    }

    //    [Fact(DisplayName = "Servico Github")]
    //    [Trait("Servico Github", "Deve retornar a última versão da aplicação no github")]
    //    public async Task Deve_Retornar_Ultima_Versao_Com_Sucesso()
    //    {
    //        // Arrange & Act
    //        var result =  await mediator.Send(new ObterUltimaVersaoQuery());

    //        // Assert
    //        Assert.Equal("v1.2", result);
    //    }
    //}
}
