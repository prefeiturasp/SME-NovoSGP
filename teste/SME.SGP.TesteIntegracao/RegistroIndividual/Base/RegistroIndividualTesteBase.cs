using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public abstract class RegistroIndividualTesteBase : TesteBaseComuns
    {
        public RegistroIndividualTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<MoverArquivosTemporariosCommand, string>),
                 typeof(MoverArquivosTemporariosCommandHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>),
                typeof(PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery,AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        protected IInserirRegistroIndividualUseCase ObterServicoInserirRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IInserirRegistroIndividualUseCase>();
        }
        protected IAlterarRegistroIndividualUseCase ObterServicoAlterarRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IAlterarRegistroIndividualUseCase>();
        }

        protected async Task CriarDadosBasicos(FiltroRegistroIndividualDto filtroRegistroIndividualDto)
        {
            await CriarTipoCalendario(filtroRegistroIndividualDto.TipoCalendario);

            await CriarDreUePerfil();
            
            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroRegistroIndividualDto.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroRegistroIndividualDto.Modalidade, filtroRegistroIndividualDto.EhAnoAnterior);

            if (filtroRegistroIndividualDto.CriarPeriodoReabertura)
                await CriarPeriodoReabertura(filtroRegistroIndividualDto.TipoCalendarioId);
        }
        
        private async Task CriarTurma(Modalidade modalidade, bool ehAnoAnterior = false)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = ehAnoAnterior,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ehAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : DateTimeExtension.HorarioBrasilia().Year,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });
        }

        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia();

            await CriarPeriodoEscolar(dataReferencia.AddDays(-285), dataReferencia.AddDays(-210), BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-200), dataReferencia.AddDays(-125), BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-115), dataReferencia.AddDays(-40), BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(dataReferencia.AddDays(-20), periodoEscolarValido ? dataReferencia.AddDays(5) : dataReferencia.AddDays(-5), BIMESTRE_4, TIPO_CALENDARIO_1);
        }

    }
}
