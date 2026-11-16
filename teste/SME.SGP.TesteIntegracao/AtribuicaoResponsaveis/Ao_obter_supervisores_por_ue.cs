using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_obter_supervisores_por_ue : TesteBase
    {
        private const string CODIGO_UE_1 = "1";
        private const string CODIGO_DRE_1 = "1";
        private const string SUPERVISOR_1 = "1";
        private const string SUPERVISOR_2 = "2";
        private const string PAAI_10 = "10";
        private const string PSICOLOGO_11 = "11";
        private const string SISTEMA = "Sistema";

        public Ao_obter_supervisores_por_ue(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Supervisores por UE - deve retornar vazio quando a UE só tem responsáveis de outros tipos")]
        public async Task Deve_retornar_vazio_quando_ue_possui_apenas_responsaveis_de_outros_tipos()
        {
            await InserirResponsavel(PAAI_10, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(PSICOLOGO_11, TipoResponsavelAtribuicao.PsicologoEscolar);

            var retorno = await ObterRepositorio().ObtemSupervisoresPorUeAsync(CODIGO_UE_1);

            retorno.ShouldBeEmpty();
        }

        [Fact(DisplayName = "Supervisores por UE - deve retornar apenas o supervisor escolar quando há outros tipos na UE")]
        public async Task Deve_retornar_apenas_supervisor_escolar_quando_ue_possui_outros_tipos()
        {
            await InserirResponsavel(SUPERVISOR_1, TipoResponsavelAtribuicao.SupervisorEscolar);
            await InserirResponsavel(PAAI_10, TipoResponsavelAtribuicao.PAAI);
            await InserirResponsavel(PSICOLOGO_11, TipoResponsavelAtribuicao.PsicologoEscolar);

            var retorno = (await ObterRepositorio().ObtemSupervisoresPorUeAsync(CODIGO_UE_1)).ToList();

            retorno.Count.ShouldBe(1);
            retorno.Single().SupervisorId.ShouldBe(SUPERVISOR_1);
        }

        [Fact(DisplayName = "Supervisores por UE - deve ignorar supervisor escolar excluído")]
        public async Task Deve_retornar_vazio_quando_supervisor_escolar_esta_excluido()
        {
            await InserirResponsavel(SUPERVISOR_2, TipoResponsavelAtribuicao.SupervisorEscolar, excluido: true);
            await InserirResponsavel(PAAI_10, TipoResponsavelAtribuicao.PAAI);

            var retorno = await ObterRepositorio().ObtemSupervisoresPorUeAsync(CODIGO_UE_1);

            retorno.ShouldBeEmpty();
        }

        private IRepositorioSupervisorEscolaDre ObterRepositorio()
            => ServiceProvider.GetService<IRepositorioSupervisorEscolaDre>();

        private async Task InserirResponsavel(string responsavel, TipoResponsavelAtribuicao tipo, bool excluido = false)
        {
            await InserirNaBase(new SupervisorEscolaDre()
            {
                DreId = CODIGO_DRE_1,
                EscolaId = CODIGO_UE_1,
                SupervisorId = responsavel,
                Tipo = (int)tipo,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA,
                CriadoRF = SISTEMA,
                Excluido = excluido
            });
        }
    }
}
