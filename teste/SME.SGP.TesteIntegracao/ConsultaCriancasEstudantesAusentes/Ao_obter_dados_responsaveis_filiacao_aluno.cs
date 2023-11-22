using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Aluno;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base;
using SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes
{
    public class Ao_obter_dados_responsaveis_filiacao_aluno : ConsultaAlunosAusentesBase
    {
        public Ao_obter_dados_responsaveis_filiacao_aluno(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosResponsaveisAlunoEolQuery, IEnumerable<DadosResponsavelAlunoDto>>), typeof(ObterDadosResponsaveisAlunoEolQueryHandlerFake), ServiceLifetime.Scoped));

            base.RegistrarFakes(services);
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter dados responsáveis filiação aluno sem filiação 2")]
        public async Task Deve_obter_dados_responsaveis_filiacao_aluno_sem_filiacao2()
        {
            await CriarDadosBasicos();

            var useCase = ServiceProvider.GetService<IObterAlunoPorCodigoEolEAnoLetivoUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO_1, DateTimeExtension.HorarioBrasilia().Year, null, true);
            retorno.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.NomeFiliacao1.ShouldBe("Nome responsável 1");
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao1.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao1.Count().ShouldBe(2);
            retorno.DadosResponsavelFiliacao.NomeFiliacao2.ShouldBeNull();
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao2.ShouldBeNull();
            retorno.DadosResponsavelFiliacao.Endereco.ShouldNotBeNull();
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Obter dados responsáveis filiação aluno com filiação 1 e 2")]
        public async Task Deve_obter_dados_responsaveis_filiacao_aluno_com_filiacao_1_e_2()
        {
            await CriarDadosBasicos();

            var useCase = ServiceProvider.GetService<IObterAlunoPorCodigoEolEAnoLetivoUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO_2, DateTimeExtension.HorarioBrasilia().Year, null, true);
            retorno.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.NomeFiliacao1.ShouldBe("Nome responsável 1");
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao1.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao1.Count().ShouldBe(2);
            retorno.DadosResponsavelFiliacao.NomeFiliacao2.ShouldBe("Nome responsável 2");
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao2.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.TelefonesFiliacao2.Count().ShouldBe(2);
            retorno.DadosResponsavelFiliacao.Endereco.ShouldNotBeNull();
        }

        [Fact(DisplayName = "ConsultaAlunosAusentes - Não deve obter dados responsáveis filiação aluno")]
        public async Task Deve_nao_deve_obter_dados_responsaveis_filiacao_aluno()
        {
            await CriarDadosBasicos();

            var useCase = ServiceProvider.GetService<IObterAlunoPorCodigoEolEAnoLetivoUseCase>();

            var retorno = await useCase.Executar(CODIGO_ALUNO_2, DateTimeExtension.HorarioBrasilia().Year, null, false);
            retorno.ShouldNotBeNull();
            retorno.DadosResponsavelFiliacao.ShouldBeNull();
        }
    }
}
