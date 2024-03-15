using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.RelatorioPAP;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ObjetivosAprendizagem
{
    public class Ao_obter_objetivos_aprendizagem_por_ano_componente : TesteBaseComuns
    {

        public Ao_obter_objetivos_aprendizagem_por_ano_componente(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterJuremaIdsPorComponentesCurricularIdQuery, long[]>), typeof(ObterJuremaIdsPorComponentesCurricularIdQueryHandlerFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Deve obter objetivos de aprendizagem por ano e componente curricular EF")]
        public async Task Deve_obter_objetivos_de_aprendizagem_por_ano_e_componente_curricular_EF()
        {
            var useCase = ServiceProvider.GetService<IListarObjetivoAprendizagemPorAnoTurmaEComponenteCurricularUseCase>();
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            var parametro = new PeriodoEscolarListaDto
            {
                TipoCalendario = 1,
                Periodos = new List<PeriodoEscolarDto>
                {
                    new PeriodoEscolarDto
                    {
                        Bimestre = 1,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(1),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 2,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(2),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(3),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 3,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(4),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(5),
                    },
                    new PeriodoEscolarDto
                    {
                        Bimestre = 4,
                        PeriodoInicio = DateTimeExtension.HorarioBrasilia().AddMinutes(6),
                        PeriodoFim = DateTimeExtension.HorarioBrasilia().AddMinutes(7),
                    }
                }
            };
            //var retorno = await useCase.Executar(parametro);
        }
    }
}