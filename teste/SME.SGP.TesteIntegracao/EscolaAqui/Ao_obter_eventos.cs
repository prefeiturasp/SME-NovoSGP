using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.EscolaAqui
{
    public class Ao_obter_eventos : TesteBaseComuns
    {
        
        public Ao_obter_eventos(CollectionFixture collectionFixture) : base(collectionFixture)
        {}

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);     
        }

        [Fact(DisplayName = "Retornar eventos dre/ue/turma de Escola Aqui e Atividades Avaliativas")]
        public async Task Deve_retornar_eventos_dre_ue_turma()
        {
            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);
            await CriarItensComuns(false, DateTime.Now.Date, DateTime.Now.Date, 1);
            await CriarTurma(Modalidade.Fundamental);
            await CriarAtividadeAvaliativaFundamental(DateTime.Now.Date);
            await CriarEvento(EventoLetivo.Opcional, DateTime.Now.Date, DateTime.Now.Date, true, 1);
            await CriarEvento(EventoLetivo.Sim, DateTime.Now.Date, DateTime.Now.Date, false, 2);
            
            var evento = ObterTodos<Evento>();
            var eventoTipo = ObterTodos<EventoTipo>();
            var eventoTipoEscolaAqui = eventoTipo.Where(et => et.EventoEscolaAqui).FirstOrDefault();
            var eventoEscolaAqui = evento.Where(e => e.TipoEventoId == eventoTipoEscolaAqui.Id);

            var useCase = ServiceProvider.GetService<IObterEventosEscolaAquiPorDreUeTurmaMesUseCase>();

            var retorno = await useCase.Executar(new Infra.FiltroEventosEscolaAquiDto() { CodigoDre = DRE_CODIGO_1, CodigoUe = UE_CODIGO_1, 
                                                                                          CodigoTurma = TURMA_CODIGO_1, MesAno = DateTime.Now.Date, 
                                                                                          ModalidadeCalendario = (int)ModalidadeTipoCalendario.FundamentalMedio });
            retorno.ShouldNotBeEmpty();
            retorno.Count().ShouldBe(2);
            retorno.Where(e => !String.IsNullOrEmpty(e.componente_curricular)).Count().ShouldBe(1);
            eventoEscolaAqui.All(ev => retorno.Any(e => e.tipo_evento != 0 && ev.Id.ToString() == e.evento_id));
        }
    }
}