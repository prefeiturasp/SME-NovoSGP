using Microsoft.AspNetCore.Http;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Consultas
{
    public class ConsultasEventosAulasCalendarioTeste
    {
        private readonly IComandosDiasLetivos comandosDiasLetivos;
        private readonly Mock<IComandosDiasLetivos> comandosDiasLetivosMock;

        private readonly Mock<IConsultasAbrangencia> consultasAbrangencia;
        private readonly Mock<IConsultasDisciplina> consultasDisciplina;
        private readonly ConsultasEventosAulasCalendario consultasEventosAulasCalendario;
        private readonly Mock<IHttpContextAccessor> httpContext;
        private readonly Mock<IRepositorioAtividadeAvaliativa> repositorioAtividadeAvaliativa;
        private readonly Mock<IRepositorioAtividadeAvaliativaDisciplina> repositorioAtividadeAvaliativaDisciplina;
        private readonly Mock<IRepositorioAtividadeAvaliativaRegencia> repositorioAtividadeAvaliativaRegencia;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCj;
        private readonly Mock<IRepositorioAula> repositorioAula;
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioParametrosSistema> repositorioParametrosSistema;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly Mock<IRepositorioTipoCalendario> repositorioTipoCalendatio;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ConsultasEventosAulasCalendarioTeste()
        {
            repositorioAula = new Mock<IRepositorioAula>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            servicoUsuario = new Mock<IServicoUsuario>();
            httpContext = new Mock<IHttpContextAccessor>();
            comandosDiasLetivosMock = new Mock<IComandosDiasLetivos>();
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistema>();
            repositorioTipoCalendatio = new Mock<IRepositorioTipoCalendario>();
            servicoEOL = new Mock<IServicoEOL>();
            repositorioAtividadeAvaliativa = new Mock<IRepositorioAtividadeAvaliativa>();
            consultasAbrangencia = new Mock<IConsultasAbrangencia>();
            repositorioAtividadeAvaliativaRegencia = new Mock<IRepositorioAtividadeAvaliativaRegencia>();
            repositorioAtividadeAvaliativaDisciplina = new Mock<IRepositorioAtividadeAvaliativaDisciplina>();
            repositorioAtribuicaoCj = new Mock<IRepositorioAtribuicaoCJ>();
            consultasDisciplina = new Mock<IConsultasDisciplina>();
            consultasEventosAulasCalendario = new ConsultasEventosAulasCalendario(repositorioEvento.Object,
                                                                                  comandosDiasLetivosMock.Object,
                                                                                  repositorioAula.Object,
                                                                                  servicoUsuario.Object,
                                                                                  servicoEOL.Object,
                                                                                  consultasAbrangencia.Object,
                                                                                  repositorioAtividadeAvaliativa.Object,
                                                                                  repositorioPeriodoEscolar.Object,
                                                                                  repositorioAtividadeAvaliativaRegencia.Object,
                                                                                  repositorioAtividadeAvaliativaDisciplina.Object,
                                                                                  consultasDisciplina.Object);
            comandosDiasLetivos = new ComandosDiasLetivos(repositorioPeriodoEscolar.Object,
                                                          repositorioEvento.Object,
                                                          repositorioTipoCalendatio.Object,
                                                          repositorioParametrosSistema.Object);
        }

        [Fact(DisplayName = "Deve_Buscar_Evento_E_Aulas_Do_Ano_Todo_Por_Tipo_Calendario_Dre_Ue")]
        public async Task Deve_Buscar_Evento_E_Aulas_Do_Ano_Todo_Por_Tipo_Calendario_Dre_Ue()
        {
            List<DateTime> diasLetivos = new List<DateTime>();
            List<DateTime> diasNaoLetivos = new List<DateTime>();

            FiltroEventosAulasCalendarioDto filtro = new FiltroEventosAulasCalendarioDto
            {
                DreId = "1",
                UeId = "2",
                TurmaId = "1",
                TipoCalendarioId = 1
            };

            PeriodoEscolar periodoEscolar = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar2 = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar3 = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar4 = new PeriodoEscolar();

            AulaDto aula = new AulaDto();
            AulaDto aula2 = new AulaDto();
            Evento evento = new Evento();
            Evento evento2 = new Evento();

            NovoPerioEscolar(periodoEscolar, 1, new DateTime(2019, 02, 01), new DateTime(2019, 03, 30));
            NovoPerioEscolar(periodoEscolar2, 2, new DateTime(2019, 04, 01), new DateTime(2019, 06, 30));
            NovoPerioEscolar(periodoEscolar3, 3, new DateTime(2019, 07, 01), new DateTime(2019, 09, 30));
            NovoPerioEscolar(periodoEscolar4, 4, new DateTime(2019, 10, 01), new DateTime(2019, 11, 30));
            NovaAula(aula, new DateTime(2019, 11, 1));
            NovaAula(aula2, new DateTime(2019, 12, 1));
            NovoEvento(evento, "Teste 1", new DateTime(2019, 11, 21), new DateTime(2019, 12, 1));
            NovoEvento(evento2, "Teste 2", new DateTime(2019, 12, 1), new DateTime(2019, 12, 1));

            IEnumerable<PeriodoEscolar> periodos = new List<PeriodoEscolar> { periodoEscolar, periodoEscolar2, periodoEscolar3, periodoEscolar4 };
            IEnumerable<AulaDto> aulas = new List<AulaDto> { aula, aula2 };
            IEnumerable<Evento> eventos = new List<Evento> { evento, evento2 };

            repositorioPeriodoEscolar.Setup(r => r.ObterPorTipoCalendario(It.IsAny<long>())).Returns(periodos);
            repositorioAula.Setup(r => r.ObterAulas(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(aulas));
            repositorioEvento.Setup(r => r.ObterEventosPorTipoDeCalendarioDreUe(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(eventos);

            var dias = comandosDiasLetivos.BuscarDiasLetivos(1);
            var diasEventosNaoLetivos = comandosDiasLetivos.ObterDias(eventos, diasNaoLetivos, Dominio.EventoLetivo.Nao);
            var diasEventosLetivos = comandosDiasLetivos.ObterDias(eventos, diasLetivos, Dominio.EventoLetivo.Sim);

            comandosDiasLetivosMock.Setup(r => r.BuscarDiasLetivos(It.IsAny<long>())).Returns(dias);
            comandosDiasLetivosMock.Setup(r => r.ObterDias(It.IsAny<IEnumerable<Evento>>(), It.IsAny<List<DateTime>>(), EventoLetivo.Nao)).Returns(diasEventosNaoLetivos);
            comandosDiasLetivosMock.Setup(r => r.ObterDias(It.IsAny<IEnumerable<Evento>>(), It.IsAny<List<DateTime>>(), EventoLetivo.Sim)).Returns(diasEventosLetivos);

            var eventosAulas = await consultasEventosAulasCalendario.ObterEventosAulasMensais(filtro);
            Assert.True(eventosAulas.ToList().Where(x => x.Mes == 11).Select(s => s.EventosAulas).FirstOrDefault() == 8);
        }

        [Fact(DisplayName = "Deve_Buscar_Tipos_De_Evento_E_Aulas_Do_Mes_Por_Tipo_Calendario_Dre_Ue")]
        public async Task Deve_Buscar_Tipos_De_Evento_E_Aulas_Do_Mes_Por_Tipo_Calendario_Dre_Ue()
        {
            FiltroEventosAulasCalendarioMesDto filtro = new FiltroEventosAulasCalendarioMesDto
            {
                DreId = "1",
                UeId = "2",
                TurmaId = "1",
                TipoCalendarioId = 1,
                Mes = 11
            };

            PeriodoEscolar periodoEscolar = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar2 = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar3 = new PeriodoEscolar();
            PeriodoEscolar periodoEscolar4 = new PeriodoEscolar();
            AulaDto aula = new AulaDto();
            AulaDto aula2 = new AulaDto();
            Evento evento = new Evento();
            Evento evento2 = new Evento();

            NovaAula(aula, new DateTime(2019, 11, 1));
            NovaAula(aula2, new DateTime(2019, 11, 2));
            NovoEvento(evento, "Teste 1", new DateTime(2019, 11, 21), new DateTime(2019, 12, 1));
            NovoEvento(evento2, "Teste 2", new DateTime(2019, 11, 1), new DateTime(2019, 11, 1));

            NovoPerioEscolar(periodoEscolar, 1, new DateTime(2019, 02, 01), new DateTime(2019, 03, 30));
            NovoPerioEscolar(periodoEscolar2, 2, new DateTime(2019, 04, 01), new DateTime(2019, 06, 30));
            NovoPerioEscolar(periodoEscolar3, 3, new DateTime(2019, 07, 01), new DateTime(2019, 09, 30));
            NovoPerioEscolar(periodoEscolar4, 4, new DateTime(2019, 10, 01), new DateTime(2019, 11, 30));

            IEnumerable<PeriodoEscolar> periodos = new List<PeriodoEscolar> { periodoEscolar, periodoEscolar2, periodoEscolar3, periodoEscolar4 };
            IEnumerable<AulaDto> aulas = new List<AulaDto> { aula, aula2 };
            IEnumerable<Evento> eventos = new List<Evento> { evento, evento2 };

            repositorioAula.Setup(r => r.ObterAulas(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null, null, null)).Returns(Task.FromResult(aulas));
            repositorioEvento.Setup(r => r.ObterEventosPorTipoDeCalendarioDreUeMes(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(eventos));
            repositorioPeriodoEscolar.Setup(r => r.ObterPorTipoCalendario(It.IsAny<long>())).Returns(periodos);

            var dias = comandosDiasLetivos.BuscarDiasLetivos(1);

            comandosDiasLetivosMock.Setup(r => r.BuscarDiasLetivos(It.IsAny<long>())).Returns(dias);

            var eventosAulas = await consultasEventosAulasCalendario.ObterTipoEventosAulas(filtro);
            Assert.True(eventosAulas.Count() == 11);
        }

        private static void NovaAula(AulaDto aula, DateTime dataAula)
        {
            aula.DataAula = dataAula;
            aula.Quantidade = 2;
            aula.TipoAula = TipoAula.Normal;
            aula.TipoCalendarioId = 1;
            aula.TurmaId = "1";
            aula.UeId = "1";
        }

        private static void NovoEvento(Evento evento, string nome, DateTime dataInicio, DateTime dataFim)
        {
            evento.DataInicio = dataInicio;
            evento.DataFim = dataFim;
            evento.DreId = "1";
            evento.UeId = "2";
            evento.Nome = nome;
            evento.Letivo = EventoLetivo.Nao;
            evento.Descricao = "Feriado";
        }

        private static void NovoPerioEscolar(PeriodoEscolar periodoEscolar, int bimestre, DateTime inicio, DateTime fim)
        {
            periodoEscolar.Bimestre = bimestre;
            periodoEscolar.PeriodoInicio = inicio;
            periodoEscolar.PeriodoFim = fim;
            periodoEscolar.TipoCalendarioId = 1;
        }
    }
}