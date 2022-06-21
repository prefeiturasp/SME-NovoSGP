using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.TestarAulaRecorrencia
{
    public class Ao_alterar_aula_com_recorrencia : AulaMockComponentePortugues
    {
        private DateTime dataInicio = new DateTime(2022, 05, 08);
        private DateTime dataFim = new DateTime(2022, 05, 08);
        public Ao_alterar_aula_com_recorrencia(CollectionFixture collectionFixture) : base(collectionFixture) { }



        [Fact]
        public async Task Altera_aula_com_recorrencia()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, dataInicio, dataFim, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), new System.DateTime(2022, 02, 10), RecorrenciaAula.RepetirBimestreAtual);
            await CriaAulaRecorrentePortugues(RecorrenciaAula.RepetirBimestreAtual);

            var usecase = ServiceProvider.GetService<IAlterarAulaUseCase>();
            var aula = ObterAulaPortugues(TipoAula.Normal, RecorrenciaAula.RepetirBimestreAtual);
            aula.RecorrenciaAula = RecorrenciaAula.RepetirTodosBimestres;
            var retorno = usecase.Executar(aula);
            
        }
    }
}
