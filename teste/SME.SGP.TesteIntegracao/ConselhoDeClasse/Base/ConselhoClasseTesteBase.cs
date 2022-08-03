using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.Base
{
    public class ConselhoClasseTesteBase : TesteBaseComuns
    {
        protected const string JUSTIFICATIVA = "Lançamento de nota";

        protected ConselhoClasseTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorTurmasCodigoQueryFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBase(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtroConselhoClasseDto.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtroConselhoClasseDto);

            if (filtroConselhoClasseDto.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtroConselhoClasseDto.ConsiderarAnoAnterior);

            if (filtroConselhoClasseDto.CriarPeriodoAbertura)
                await CriarPeriodoReabertura(filtroConselhoClasseDto.TipoCalendarioId);

            await CriarParametrosNotas();

            await CriarAbrangencia(filtroConselhoClasseDto.Perfil);
            
            await CriarCiclo();

            await CriarNotasTipoEParametros(filtroConselhoClasseDto.ConsiderarAnoAnterior);

            await CriaConceito();

            await CriaComponenteCurricularGrupoAreaOrdenacao();

            if (filtroConselhoClasseDto.InserirFechamentoAlunoPadrao)
                await InserirFechamentoAluno(filtroConselhoClasseDto);
            
            if (filtroConselhoClasseDto.InserirConselhoClassePadrao)
                await InserirConselhoClassePadrao(filtroConselhoClasseDto);

            await CriarConselhoClasseParecerAno();
        }

        private async Task CriarConselhoClasseParecerAno()
        {
            var camposConselhoClasseParecer = "nome,aprovado,frequencia,conselho,inicio_vigencia,fim_vigencia,criado_por,criado_rf,criado_em,alterado_por,alterado_rf,alterado_em,nota";
            
            await InserirNaBaseComCampos("conselho_classe_parecer",camposConselhoClasseParecer,"'Promovido'","true","true","false","'2014-01-01'","null","'SISTEMA'","0","'2014-01-01'","null","null","null","true");
            await InserirNaBaseComCampos("conselho_classe_parecer",camposConselhoClasseParecer,"'Promovido pelo conselho'","true","false","true","'2014-01-01'","null","'SISTEMA'","0","'2014-01-01'","null","null","null","false");
            await InserirNaBaseComCampos("conselho_classe_parecer",camposConselhoClasseParecer,"'Continuidade dos estudos'","true","true","false","'2014-01-01'","null","'SISTEMA'","0","'2014-01-01'","null","null","null","false");
            await InserirNaBaseComCampos("conselho_classe_parecer",camposConselhoClasseParecer,"'Retido'","false","false","true","'2014-01-01'","null","'SISTEMA'","0","'2014-01-01'","null","null","null","true");
            await InserirNaBaseComCampos("conselho_classe_parecer",camposConselhoClasseParecer,"'Retido por frequência'","false","true","false","'2014-01-01'","null","'SISTEMA'","0","'2014-01-01'","null","null","null","false");

            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3, AnoTurma = 1, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3, AnoTurma = 1, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3, AnoTurma = 2, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3, AnoTurma = 4, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 3, AnoTurma = 5, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 1, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 1, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 2, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 4, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 5, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 2, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 3, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 4, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 3, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 6, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 7, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 8, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 9, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 1, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 2, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 2, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
               ParecerId = 2, AnoTurma = 3, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
               CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 4, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 3, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 6, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 7, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 8, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 9, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 1, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 2, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 3, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 2, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 3, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 4, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 3, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 6, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 7, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 8, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 9, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 1, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 2, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 3, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 2, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 3, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 4, Modalidade = 3, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 3, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 6, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 7, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 8, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 9, Modalidade = 5, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 1, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 2, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 3, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 3, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 2, AnoTurma = 4, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 4, AnoTurma = 4, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 5, AnoTurma = 4, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
            
            await InserirNaBase(new ConselhoClasseParecerAno()
            {
                ParecerId = 1, AnoTurma = 4, Modalidade = 6, InicioVigencia = DateTimeExtension.HorarioBrasilia().Date.AddYears(-1),
                CriadoPor = SISTEMA_CODIGO_RF, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        protected async Task CriaConceito()
        {
            await InserirNaBase(new Conceito()
            {
                Descricao = "Excelente",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "E",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Bom",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "B",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Conceito()
            {
                Descricao = "Ruim",
                Aprovado = true,
                Ativo = true,
                InicioVigencia = DATA_01_01,
                FimVigencia = DATA_31_12,
                Valor = "R",
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task InserirConselhoClassePadrao(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var fechamentoTurmas = ObterTodos<FechamentoTurma>();

            long conselhoClasseId = 1;
            
            long conselhoClasseAlunoId = 1;

            foreach (var fechamentoTurma in fechamentoTurmas)
            {
                await InserirNaBase(new ConselhoClasse()
                {
                    FechamentoTurmaId = fechamentoTurma.Id,
                    Situacao = filtroConselhoClasseDto.SituacaoConselhoClasse,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                });
                
                foreach (var alunoCodigo in ObterAlunos())
                {
                    await InserirNaBase(new ConselhoClasseAluno()
                    {
                        ConselhoClasseId = conselhoClasseId,
                        AlunoCodigo = alunoCodigo,
                        CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                    });

                    foreach (var componenteCurricular in ObterComponentesCurriculares())
                    {
                        await InserirNaBase(new ConselhoClasseNota()
                        {
                            ComponenteCurricularCodigo = componenteCurricular,
                            ConselhoClasseAlunoId = conselhoClasseAlunoId,
                            Justificativa = JUSTIFICATIVA,
                            Nota = filtroConselhoClasseDto.TipoNota == TipoNota.Nota ? new Random().Next(0, 10) : null,
                            ConceitoId = filtroConselhoClasseDto.TipoNota == TipoNota.Conceito ? new Random().Next(1, 3) : null,
                            CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
                        });    
                    }
                    conselhoClasseAlunoId++;
                }

                conselhoClasseId++;
            }
        }
        
        private IEnumerable<long> ObterComponentesCurriculares()
        {
            return new List<long>()
            {
                long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                long.Parse(COMPONENTE_HISTORIA_ID_7),
                long.Parse(COMPONENTE_GEOGRAFIA_ID_8),
                long.Parse(COMPONENTE_CIENCIAS_ID_89),
                long.Parse(COMPONENTE_EDUCACAO_FISICA_ID_6),
                COMPONENTE_CURRICULAR_ARTES_ID_139,
                long.Parse(COMPONENTE_MATEMATICA_ID_2)
            };
        }

        private IEnumerable<string> ObterAlunos()
        {
            return new List<string>()
            {
                ALUNO_CODIGO_1, ALUNO_CODIGO_2, ALUNO_CODIGO_3, ALUNO_CODIGO_4, ALUNO_CODIGO_5
            };
        }


        private async Task InserirFechamentoAluno(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            var bimestres = ObterBimestresComBimestreFinal();

            long fechamentoTurmaDisciplinaId = 1;
            
            long fechamentoAlunoId = 1;

            foreach (var bimestre in bimestres)
            {
                await InserirNaBase(new FechamentoTurma()
                {
                    TurmaId = TURMA_ID_1,
                    PeriodoEscolarId = bimestre,
                    CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                });

                foreach (var componenteCurricular in ObterComponentesCurriculares())
                {
                    await InserirNaBase(new FechamentoTurmaDisciplina()
                    {
                        DisciplinaId = componenteCurricular,
                        FechamentoTurmaId = 1,
                        CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                    });

                    foreach (var alunoCodigo in ObterAlunos())
                    {
                        await InserirNaBase(new FechamentoAluno()
                        {
                            FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                            AlunoCodigo = alunoCodigo,
                            CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                        });

                        await InserirNaBase(new FechamentoNota()
                        {
                            DisciplinaId = componenteCurricular,
                            FechamentoAlunoId = fechamentoAlunoId,
                            Nota = filtroConselhoClasseDto.TipoNota == TipoNota.Nota ? new Random().Next(0, 10) : null,
                            ConceitoId = filtroConselhoClasseDto.TipoNota == TipoNota.Conceito
                                ? new Random().Next(1, 3)
                                : null,
                            CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
                        });

                        fechamentoAlunoId++;
                    }

                    fechamentoTurmaDisciplinaId++;
                }
            }
        }

        private static List<int?> ObterBimestresComBimestreFinal()
        {
            return new List<int?>() { BIMESTRE_1,BIMESTRE_2,BIMESTRE_3,BIMESTRE_4, null };
        }

        protected async Task CriarTurmaTipoCalendario(FiltroConselhoClasseDto filtroConselhoClasseDto)
        {
            await CriarTipoCalendario(filtroConselhoClasseDto.TipoCalendario, filtroConselhoClasseDto.ConsiderarAnoAnterior);
            await CriarTurma(filtroConselhoClasseDto.Modalidade, filtroConselhoClasseDto.AnoTurma, filtroConselhoClasseDto.ConsiderarAnoAnterior);
        }

        private async Task CriarNotasTipoEParametros(bool consideraAnoAnterior = false)
        {
            var dataBase = consideraAnoAnterior ? new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 01) : new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Nota,
                Descricao = NOTA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaTipoValor()
            {
                Ativo = true,
                InicioVigencia = dataBase,
                TipoNota = TipoNota.Conceito,
                Descricao = CONCEITO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 1,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 2,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 3,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 4,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 5,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 6,
                TipoNotaId = 2,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 7,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaConceitoCicloParametro()
            {
                CicloId = 8,
                TipoNotaId = 1,
                QtdMinimaAvalicoes = 1,
                PercentualAlerta = 50,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new NotaParametro()
            {
                Minima = 0,
                Media = 5,
                Maxima = 10,
                Incremento = 0.5,
                Ativo = true,
                InicioVigencia = dataBase,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_FREQUENCIA_CRITICO_NOME,
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                Descricao = PERCENTUAL_FREQUENCIA_CRITICO_DESCRICAO,
                Valor = "75",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarCiclo()
        {
            await InserirNaBase(new Ciclo()
            {
                Descricao = ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_1,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_2,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 1,
                Ano = ANO_3,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = INTERDISCIPLINAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_4,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_5,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 2,
                Ano = ANO_6,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = AUTORAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_7,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_8,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 3,
                Ano = ANO_9,
                Modalidade = Modalidade.Fundamental
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = MEDIO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_1,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_2,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_3,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 4,
                Ano = ANO_4,
                Modalidade = Modalidade.Medio
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_ALFABETIZACAO,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 5,
                Ano = ANO_1,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_BASICA,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 6,
                Ano = ANO_2,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_COMPLEMENTAR,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 7,
                Ano = ANO_3,
                Modalidade = Modalidade.EJA
            });

            await InserirNaBase(new Ciclo()
            {
                Descricao = EJA_FINAL,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CicloAno()
            {
                CicloId = 8,
                Ano = ANO_4,
                Modalidade = Modalidade.EJA
            });
        }

        protected async Task CriarAbrangencia(string perfil)
        {
            await InserirNaBase(new Abrangencia()
            {
                DreId = DRE_ID_1,
                Historico = false,
                Perfil = new Guid(perfil),
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                UsuarioId = USUARIO_ID_1
            });
        }

        private async Task CriarParametrosNotas()
        {
            var dataAtualAnoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1);

            var dataAtualAnoAtual = DateTimeExtension.HorarioBrasilia();

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAnterior.Year.ToString(),
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = DATA_INICIO_SGP,
                Descricao = DATA_INICIO_SGP,
                Tipo = TipoParametroSistema.DataInicioSGP,
                Valor = dataAtualAnoAtual.Year.ToString(),
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Descricao = PERCENTUAL_ALUNOS_INSUFICIENTES,
                Tipo = TipoParametroSistema.PercentualAlunosInsuficientes,
                Valor = NUMERO_50,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAnterior.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAnterior,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = MEDIA_BIMESTRAL,
                Descricao = MEDIA_BIMESTRAL,
                Tipo = TipoParametroSistema.MediaBimestre,
                Valor = NUMERO_5,
                Ano = dataAtualAnoAtual.Year,
                Ativo = true,
                CriadoEm = dataAtualAnoAtual,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarPeriodoReabertura()
        {
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, bool considerarAnoAnterior = false)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = considerarAnoAnterior ? DATA_01_01.AddYears(-1) : DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task CriaComponenteCurricularGrupoAreaOrdenacao()
        {
            await InserirNaBase("componente_curricular_grupo_area_ordenacao", "1", "1", "1");
        }

        protected class FiltroConselhoClasseDto
        {
            public FiltroConselhoClasseDto()
            {
                CriarPeriodoEscolar = true;
                TipoCalendarioId = TIPO_CALENDARIO_1;
                CriarPeriodoAbertura = true;
                ConsiderarAnoAnterior = false;
            }
            public DateTime? DataReferencia { get; set; }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public int Bimestre { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public bool CriarPeriodoAbertura { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public SituacaoConselhoClasse SituacaoConselhoClasse { get; set; }
            public TipoNota TipoNota { get; set; }
            public bool InserirConselhoClassePadrao { get; set; }
            public bool InserirFechamentoAlunoPadrao { get; set; }
        }

        protected class ConselhoClasseFakeDto
        {
            public long FechamentoTurmaId { get; set; }
            public long Situacao { get; set; }
            public IEnumerable<ConselhoClasseAlunoFakeDto> ConselhosClasseAlunos { get; set; }
        }
        
        protected class ConselhoClasseAlunoFakeDto
        {
            public long ConselhoClasseId { get; set; }
            public string AlunoCodigo { get; set; }
            public string RecomendacoesAluno { get; set; } = string.Empty;
            public string RecomendacoesFamilia { get; set; } = string.Empty;
            public string AnotacoesPedagogicas { get; set; } = string.Empty;
            public long? ConselhoClasseParecerId { get; set; } 
            public IEnumerable<ConselhoClasseNotaFakeDto> ConselhosClasseNotas { get; set; }
        }
        
        protected class ConselhoClasseNotaFakeDto
        {
            public long ComponenteCurricularCodigo { get; set; }
            public long? Nota { get; set; }
            public long? ConceitoId { get; set; }
            public string Justificativa { get; set; } = string.Empty;
        }    
        
        protected IMediator RetornarServicoMediator()
        {
            return ServiceProvider.GetService<IMediator>();
        }
        
        protected IGerarParecerConclusivoUseCase RetornarGerarParecerConclusivoUseCase()
        {
            return ServiceProvider.GetService<IGerarParecerConclusivoUseCase>();
        }
        
        protected IConsolidarConselhoClasseUseCase RetornarConsolidarConselhoClasseUseCase()
        {
            return ServiceProvider.GetService<IConsolidarConselhoClasseUseCase>();
        }
        
        protected IReprocessarParecerConclusivoAlunoUseCase RetornarReprocessarParecerConclusivoAlunoUseCase()
        {
            return ServiceProvider.GetService<IReprocessarParecerConclusivoAlunoUseCase>();
        }
    }
}