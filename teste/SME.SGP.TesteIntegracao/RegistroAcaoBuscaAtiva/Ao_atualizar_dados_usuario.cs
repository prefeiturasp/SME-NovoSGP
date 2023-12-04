using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroAcaoBuscaAtiva
{
    public class Ao_atualizar_dados_usuario : RegistroAcaoBuscaAtivaTesteBase
    {
        public Ao_atualizar_dados_usuario(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosResponsavelQuery, IEnumerable<DadosResponsavelAlunoEolDto>>), typeof(ObterDadosResponsavelQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Busca ativa - Ao converter valores atualização dados do usuário")]
        public async Task Ao_converter_valores_atualizacao_dados_usuário()
        {
            var mediator = ServiceProvider.GetService<IMediator>();
            var dto = new AtualizarDadosUsuarioDto()
            {
                CPF = "1",
                Celular = "(99) 9876-5432",
                FoneComercial = "(99) 1234-5678",
                FoneResidencial = "(99) 4321 - 9876",
                Email = "conveter@teste.com"
            };

            var dadosResponsaveis = await mediator.Send(new ObterDadosResponsavelQuery("1"));

            var dadosEol = new DadosResponsavelAlunoBuscaAtivaDto(dadosResponsaveis.FirstOrDefault(), dto);

            dadosEol.Email.ShouldBe(dto.Email);
            dadosEol.DDDCelular.ShouldBe("99");
            dadosEol.NumeroCelular.ShouldBe("98765432");
            dadosEol.DDDComercial.ShouldBe("99");
            dadosEol.NumeroComercial.ShouldBe("12345678");
            dadosEol.DDDResidencial.ShouldBe("99");
            dadosEol.NumeroResidencial.ShouldBe("43219876");

            var dadosProdam = new DadosResponsavelAlunoProdamDto(dadosResponsaveis.FirstOrDefault(), dto);

            dadosProdam.Email.ShouldBe(dto.Email);
            dadosProdam.DDDCelular.ShouldBe("99");
            dadosProdam.NumeroCelular.ShouldBe("98765432");
            dadosProdam.DDDTelefoneComercial.ShouldBe("99");
            dadosProdam.NumeroTelefoneComercial.ShouldBe("12345678");
            dadosProdam.DDDTelefoneFixo.ShouldBe("99");
            dadosProdam.NumeroTelefoneFixo.ShouldBe("43219876");
        }
    }
}
