using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.TipoCalendarioValidacoes
{
    public class Validar_tipo_calendario : TesteBaseComuns
    {
        public Validar_tipo_calendario(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        [Fact(DisplayName = "Tipo Calendario - Obter Periodo Escolar Por Calendario E Data")]
        public async Task Ao_obter_periodo_escolar_por_calendario_e_data()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new ObterPeriodoEscolarPorCalendarioEDataQuery(TIPO_CALENDARIO_7,DATA_25_07_INICIO_BIMESTRE_3));
            retorno.ShouldNotBeNull();
            retorno.TipoCalendarioId.ShouldBe(TIPO_CALENDARIO_7);
            retorno.PeriodoInicio.ShouldBe(DATA_25_07_INICIO_BIMESTRE_3);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipo Calendario Por Ano Letivo E Modalidade")]
        public async Task Ao_obter_tipo_calendario_por_ano_letivo_e_modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new ObterTipoCalendarioPorAnoLetivoEModalidadeQuery(ANO_LETIVO_ANO_ATUAL,ModalidadeTipoCalendario.CELP,SEMESTRE_1));
            retorno.ShouldNotBeNull();
            retorno.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            retorno.Semestre.ShouldBe(SEMESTRE_1);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipo Calendario Por Id")]
        public async Task Ao_obter_tipo_calendario_por_id()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var consultasTipoCalendario = ServiceProvider.GetService<IConsultasTipoCalendario>();

            var retorno = consultasTipoCalendario.ObterPorId(TIPO_CALENDARIO_10); 
            retorno.ShouldNotBeNull();
            retorno.Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos de Calendarios")]
        public async Task Ao_obter_tipos_de_calendarios()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var consultasTipoCalendario = ServiceProvider.GetService<IConsultasTipoCalendario>();

            var retorno = await consultasTipoCalendario.Listar(); 
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(10);
            
            var ejaPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_1);
            ejaPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_1);
            celpPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            retorno.LastOrDefault().Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.LastOrDefault().Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Verificar Registro Existente Tipo Calendario por nome")]
        public async Task Ao_verificar_registro_existente_tipo_calendario_por_nome()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new VerificarRegistroExistenteTipoCalendarioQuery(0,NOME_TIPO_CALENDARIO_ANO_ATUAL));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Verificar Registro Existente Tipo Calendario por id e nome")]
        public async Task Ao_verificar_registro_existente_tipo_calendario_por_id_e_nome()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new VerificarRegistroExistenteTipoCalendarioQuery(TIPO_CALENDARIO_10,NOME_TIPO_CALENDARIO_ANO_ATUAL));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Validar período em aberto")]
        public async Task Ao_vlaidar_periodo_em_aberto()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var consultasTipoCalendario = ServiceProvider.GetService<IConsultasTipoCalendario>();

            var retorno = await consultasTipoCalendario.PeriodoEmAberto(new TipoCalendario(){Id = TIPO_CALENDARIO_8},DATA_03_01_INICIO_BIMESTRE_1,BIMESTRE_1); 
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipo Calendario Id Por Ano Letivo E Modalidade")]
        public async Task Ao_obter_tipo_calendario_id_por_ano_letivo_e_modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();

            var retorno = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoEModalidadeQuery(ModalidadeTipoCalendario.CELP,ANO_LETIVO_ANO_ATUAL,SEMESTRE_2));
            retorno.ShouldBe(9);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos Calendarios Por Ano Letivo e Modalidade")]
        public async Task Ao_obter_tipos_de_calendarios_Por_Ano_Letivo_Modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();
            var modalidades = "EducacaoInfantil,EJA,CIEJA,Fundamental,Medio,CMCT,MOVA,ETEC,CELP";
            var retorno = await mediator.Send(new ObterTiposCalendarioPorAnoLetivoModalidadeQuery(ANO_LETIVO_ANO_ATUAL, modalidades,SEMESTRE_2));

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            
            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos Calendarios Por Ano Letivo, Descricao E Modalidades")]
        public async Task Ao_obter_tipos_calendarios_por_ano_letivo_descricao_e_modalidades()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();
            var modalidades = new List<int>() {(int)Modalidade.CELP,(int)Modalidade.EJA, (int)Modalidade.Fundamental};
            var retorno = await mediator.Send(new ObterTiposCalendarioPorAnoLetivoDescricaoEModalidadesQuery(ANO_LETIVO_ANO_ATUAL, modalidades,string.Empty));

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);
            
            var ejaPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_1);
            ejaPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_1);
            celpPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            retorno.LastOrDefault().Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.LastOrDefault().Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos Calendarios Por Busca")]
        public async Task Ao_obter_tipos_calendarios_por_busca()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterTiposCalendariosPorBuscaQuery(NOME_TIPO_CALENDARIO_ANO_ATUAL));

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);
            
            var ejaPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_1);
            ejaPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_1);
            celpPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            retorno.LastOrDefault().Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.LastOrDefault().Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipo Calendario Por Id")]
        public async Task Ao_obter_nome_tipo_calendario_por_id()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
            
            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterNomeTipoCalendarioPorIdQuery(TIPO_CALENDARIO_10));
            retorno.ShouldNotBeNull();
            retorno.ShouldBe(NOME_TIPO_CALENDARIO_ANO_ATUAL);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos Calendarios Por Anos Letivo e Modalidades")]
        public async Task Ao_obter_nome_tipos_calendarios_por_anos_letivo_e_modalidades()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            var mediator = ServiceProvider.GetService<IMediator>();
            var modalidades = new [] {(int)ModalidadeTipoCalendario.CELP,(int)ModalidadeTipoCalendario.EJA, (int)ModalidadeTipoCalendario.FundamentalMedio};
            var retorno = await mediator.Send(new ObterTiposCalendariosPorAnosLetivoModalidadesQuery(new []{ANO_LETIVO_ANO_ATUAL},modalidades, NOME_TIPO_CALENDARIO_ANO_ATUAL));
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);
            
            var ejaPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_1);
            ejaPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_1);
            celpPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            retorno.LastOrDefault().Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.LastOrDefault().Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipos Calendarios Por Anos Letivo e Modalidades sem descrição")]
        public async Task Ao_obter_nome_tipos_calendarios_por_anos_letivo_e_modalidades_sem_descricao()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            var mediator = ServiceProvider.GetService<IMediator>();
            var modalidades = new [] {(int)ModalidadeTipoCalendario.CELP,(int)ModalidadeTipoCalendario.EJA, (int)ModalidadeTipoCalendario.FundamentalMedio};
            var retorno = await mediator.Send(new ObterTiposCalendariosPorAnosLetivoModalidadesQuery(new []{ANO_LETIVO_ANO_ATUAL},modalidades, string.Empty));
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(5);
            
            var ejaPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_1);
            ejaPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var ejaSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.EJA && a.Semestre == SEMESTRE_2);
            ejaSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.EJA);
            ejaSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            var celpPrimeiroSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_1);
            celpPrimeiroSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpPrimeiroSemestre.Semestre.ShouldBe(SEMESTRE_1);

            var celpSegundoSemestre = retorno.FirstOrDefault(a=> a.Modalidade == ModalidadeTipoCalendario.CELP && a.Semestre == SEMESTRE_2);
            celpSegundoSemestre.Modalidade.ShouldBe(ModalidadeTipoCalendario.CELP);
            celpSegundoSemestre.Semestre.ShouldBe(SEMESTRE_2);
            
            retorno.LastOrDefault().Modalidade.ShouldBe(ModalidadeTipoCalendario.FundamentalMedio);
            retorno.LastOrDefault().Semestre.ShouldBeNull();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Periodo Calendario Bimestre Por Ano Letivo e Modalidade")]
        public async Task Ao_obter_periodo_calendario_bimestre_por_ano_letivo_e_modalidade()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterPeriodoCalendarioBimestrePorAnoLetivoModalidadeQuery(ANO_LETIVO_ANO_ATUAL,ModalidadeTipoCalendario.CELP,SEMESTRE_2));
            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(2);
            
            var celpPrimeiroPeriodoEscolar = retorno.FirstOrDefault(f=> f.PeriodoEscolarId == 19);
            celpPrimeiroPeriodoEscolar.Bimestre.ShouldBe(BIMESTRE_1);
            celpPrimeiroPeriodoEscolar.PeriodoInicio.ShouldBe(DATA_25_07_INICIO_BIMESTRE_3);
            
            var celpSegundoPeriodoEscolar = retorno.FirstOrDefault(f=> f.PeriodoEscolarId == 20);
            celpSegundoPeriodoEscolar.Bimestre.ShouldBe(BIMESTRE_2);
            celpSegundoPeriodoEscolar.PeriodoInicio.ShouldBe(DATA_03_10_INICIO_BIMESTRE_4);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Obter Tipo Calendario Id Por Ano Letivo, Modalidade E Data de Referencia")]
        public async Task Ao_obter_tipo_calendario_id_por_ano_letivo_modalidade_e_data_de_referencia()
        {
            await CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();

            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterTipoCalendarioIdPorAnoLetivoModalidadeEDataReferenciaQuery(ANO_LETIVO_ANO_ATUAL,ModalidadeTipoCalendario.CELP,DATA_03_01_INICIO_BIMESTRE_1));
            retorno.ShouldNotBe(0);
            retorno.ShouldBe(TIPO_CALENDARIO_8);
        }
        
        [Fact(DisplayName = "Tipo Calendario - Inserir")]
        public async Task Ao_inserir()
        {
            await CriarEventoTipoResumido("Feriado",
                EventoLocalOcorrencia.SME,
                true,
                EventoTipoData.Unico,
                false,
                EventoLetivo.Nao,
                (long)TipoEvento.Feriado);
            
            var comandosTipoCalendario = ServiceProvider.GetService<IComandosTipoCalendario>();
            await comandosTipoCalendario.Incluir(new TipoCalendarioDto()
            {
                Modalidade = ModalidadeTipoCalendario.CELP,
                Nome = NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL
            });
            var tipoCalendario = ObterTodos<TipoCalendario>();
            tipoCalendario.ShouldNotBeNull();
            var tipoCalendarioInserido = tipoCalendario.FirstOrDefault();
            tipoCalendarioInserido.Semestre = SEMESTRE_1;
            tipoCalendarioInserido.AnoLetivo = ANO_LETIVO_ANO_ATUAL;
        }
        
        [Fact(DisplayName = "Tipo Calendario - Não deve inserir sem semestre")]
        public async Task Ao_inserir_sem_semestre()
        {
            await CriarEventoTipoResumido("Feriado",
                EventoLocalOcorrencia.SME,
                true,
                EventoTipoData.Unico,
                false,
                EventoLetivo.Nao,
                (long)TipoEvento.Feriado);
            
            var comandosTipoCalendario = ServiceProvider.GetService<IComandosTipoCalendario>();
            await comandosTipoCalendario.Incluir(new TipoCalendarioDto()
            {
                Modalidade = ModalidadeTipoCalendario.CELP,
                Nome = NOME_TIPO_CALENDARIO_ANO_ATUAL,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Não deve inserir com semestre para tipo calendário diferente de EJA e CELP")]
        public async Task Ao_inserir_com_semestre_para_tipo_diferente_de_eja_celp()
        {
            await CriarEventoTipoResumido("Feriado",
                EventoLocalOcorrencia.SME,
                true,
                EventoTipoData.Unico,
                false,
                EventoLetivo.Nao,
                (long)TipoEvento.Feriado);
            
            var comandosTipoCalendario = ServiceProvider.GetService<IComandosTipoCalendario>();
            await comandosTipoCalendario.Incluir(new TipoCalendarioDto()
            {
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = NOME_TIPO_CALENDARIO_ANO_ATUAL,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                Semestre = SEMESTRE_1,
            }).ShouldThrowAsync<NegocioException>();
        }
        
        [Fact(DisplayName = "Tipo Calendario - Inserir para tipo fundamental")]
        public async Task Ao_inserir_para_tipo_fundamental()
        {
            await CriarEventoTipoResumido("Feriado",
                EventoLocalOcorrencia.SME,
                true,
                EventoTipoData.Unico,
                false,
                EventoLetivo.Nao,
                (long)TipoEvento.Feriado);
            
            var comandosTipoCalendario = ServiceProvider.GetService<IComandosTipoCalendario>();
            await comandosTipoCalendario.Incluir(new TipoCalendarioDto()
            {
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Nome = NOME_TIPO_CALENDARIO_ANO_ATUAL,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
            });
            
            var tipoCalendario = ObterTodos<TipoCalendario>();
            tipoCalendario.ShouldNotBeNull();
            var tipoCalendarioInserido = tipoCalendario.FirstOrDefault();
            tipoCalendarioInserido.Modalidade = ModalidadeTipoCalendario.FundamentalMedio;
            tipoCalendarioInserido.AnoLetivo = ANO_LETIVO_ANO_ATUAL;
        }
        
        [Fact(DisplayName = "Tipo Calendario - Atualizar")]
        public async Task Ao_atualizar()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.CELP,semestre:SEMESTRE_1);
            
            await CriarEventoTipoResumido("Feriado",
                EventoLocalOcorrencia.SME,
                true,
                EventoTipoData.Unico,
                false,
                EventoLetivo.Nao,
                (long)TipoEvento.Feriado);
            
            var comandosTipoCalendario = ServiceProvider.GetService<IComandosTipoCalendario>();
            await comandosTipoCalendario.Alterar(new TipoCalendarioDto()
            {
                Modalidade = ModalidadeTipoCalendario.CELP,
                Nome = NOME_TIPO_CALENDARIO_ANO_ATUAL,
                Semestre = SEMESTRE_2,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL
            },TIPO_CALENDARIO_1);
            
            var tipoCalendarios = ObterTodos<TipoCalendario>();
            tipoCalendarios.ShouldNotBeNull();
            tipoCalendarios.Count().ShouldBe(1);
            var tipoCalendarioInserido = tipoCalendarios.FirstOrDefault();
            tipoCalendarioInserido.Semestre = SEMESTRE_2;
            tipoCalendarioInserido.AnoLetivo = ANO_LETIVO_ANO_ATUAL;
        }

        [Fact(DisplayName = "Tipo Calendario - Validar comparações de Modalidades")]
        public async Task Ao_validar_modalidade_tipo_calendario()
        {
            ModalidadeTipoCalendario.CELP.ObterModalidades().Any(a=> a.EhCELP()).ShouldBeTrue();
            ModalidadeTipoCalendario.EJA.ObterModalidades().Any(a=> a.EhEJA()).ShouldBeTrue();
            ModalidadeTipoCalendario.FundamentalMedio.ObterModalidades().Any(a=> a.EhFundamental()).ShouldBeTrue();
            ModalidadeTipoCalendario.FundamentalMedio.ObterModalidades().Any(a=> a.EhMedio()).ShouldBeTrue();
            ModalidadeTipoCalendario.Infantil.ObterModalidades().Any(a=> a.EhEducacaoInfantil()).ShouldBeTrue();
            
            Modalidade.CELP.ObterModalidadeTipoCalendario().EhCELP().ShouldBeTrue();
            Modalidade.EJA.ObterModalidadeTipoCalendario().EhEJA().ShouldBeTrue();
            Modalidade.Fundamental.ObterModalidadeTipoCalendario().EhFundamentalMedio().ShouldBeTrue();
            Modalidade.Medio.ObterModalidadeTipoCalendario().EhFundamentalMedio().ShouldBeTrue();
            Modalidade.EducacaoInfantil.ObterModalidadeTipoCalendario().EhEducacaoInfantil().ShouldBeTrue();
        }
    }
}