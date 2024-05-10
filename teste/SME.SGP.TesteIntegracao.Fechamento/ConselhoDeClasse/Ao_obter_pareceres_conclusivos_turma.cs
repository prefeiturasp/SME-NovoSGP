using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ConselhoDeClasse;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Fechamento.ConselhoDeClasse
{
    public class Ao_obter_pareceres_conclusivos_turma : ConselhoDeClasseTesteBase
    {
        public Ao_obter_pareceres_conclusivos_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Conselho Classe - Deve retornar todos os pareceres conclusivos da turma")]
        public async Task Ao_obter_pareceres_conclusivos()
        {
            var filtroNota = new FiltroConselhoClasseDto()
            {
                Perfil = ObterPerfilProfessorInfantil(),
                Modalidade = Modalidade.Fundamental,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Bimestre = BIMESTRE_4,
                AnoTurma = "5",
                ComponenteCurricular = COMPONENTE_CURRICULAR_512.ToString(),
                DataAula = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                CriarPeriodoReabertura = false,
            };

            await CriarDadosBaseSemFechamentoTurmaSemAberturaReabertura(filtroNota);

            var useCase = ServiceProvider.GetService<IObterPareceresConclusivosTurmaUseCase>();
            var retorno = await useCase.Executar(TURMA_ID_1);

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            retorno.ToList().Exists(parecer => parecer.Nome == "Retido por frequência");
            retorno.ToList().Exists(parecer => parecer.Nome == "Continuidade dos estudos");
        }
    }
}
