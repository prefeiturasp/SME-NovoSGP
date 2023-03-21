using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.TesteIntegracao.AulaUnicaGrade
{
    public class Ao_obter_aulas_por_turma_componente_data : AulaTeste
    {
        public Ao_obter_aulas_por_turma_componente_data(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Obter_aulas()
        {
            await CriarDadosBasicosAulaSemPeriodoEscolar(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);

            var dataBase = DateTimeExtension.HorarioBrasilia();
            
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase, RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(1), RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(2), RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(3), RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(4), RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(5), RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);
            
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase, RecorrenciaAula.AulaUnica);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(1), RecorrenciaAula.AulaUnica);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(2), RecorrenciaAula.AulaUnica);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(3), RecorrenciaAula.AulaUnica);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(4), RecorrenciaAula.AulaUnica);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataBase.AddDays(5), RecorrenciaAula.AulaUnica);

            var aulas = ObterTodos<Dominio.Aula>();

            var useCase = ServiceProvider.GetService<IObterAulasPorTurmaComponenteDataUseCase>();

            for (int i = 0; i <= 5; i++)
            {
                var retorno = await useCase.Executar(new FiltroObterAulasPorTurmaComponenteDataDto(TURMA_CODIGO_1,COMPONENTE_CURRICULAR_PORTUGUES_ID_138, dataBase.AddDays(i)));
                retorno.ShouldNotBeNull();
                retorno.Count().ShouldBeEquivalentTo(2);
            }
        }
    }
}
