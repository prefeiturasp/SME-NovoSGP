using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAbrangencia : IConsultasAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ConsultasAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario, IServicoEol servicoEOL, IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto, bool consideraHistorico, bool consideraNovosAnosInfantil = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var anoLetivo = DateTime.Now.Year;
            var anosInfantilDesconsiderar = !consideraNovosAnosInfantil ? await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(anoLetivo, Modalidade.EducacaoInfantil)) : null;

            return await repositorioAbrangencia.ObterAbrangenciaPorFiltro(texto, login, perfil, consideraHistorico, anosInfantilDesconsiderar);
        }

        public async Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistorica()
        {
            var login = servicoUsuario.ObterLoginAtual();
            return await repositorioAbrangencia.ObterAbrangenciaHistoricaPorLogin(login);
        }

        public async Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, bool consideraHistorico = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            AbrangenciaCompactaVigenteRetornoEOLDTO abrangencia = await servicoEOL.ObterAbrangenciaCompactaVigente(login.ToString(), Guid.Parse(perfil.ToString()));
            bool abrangenciaPermitida = abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.UE
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.Dre
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.SME;

            return await repositorioAbrangencia.ObterAbrangenciaTurma(turma, login, perfil, consideraHistorico, abrangenciaPermitida);
        }

        public async Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico, int anoMinimo)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            List<int> anosLetivos = new List<int>();
            var anos = await mediator.Send(new ObterUsuarioAbrangenciaAnosLetivosQuery(login, consideraHistorico, perfil, anoMinimo));

            anosLetivos.AddRange(anos);

            if ((perfil == Perfis.PERFIL_CJ || perfil == Perfis.PERFIL_CJ_INFANTIL) && consideraHistorico)
            {
                var anosCJ = await mediator.Send(new ObterAnosAtribuicaoCJQuery(login, consideraHistorico));
                if (anosCJ.Any())
                    anosLetivos.AddRange(anosCJ);
                
            }
            anosLetivos.Add(DateTime.Now.Year);
            return anosLetivos.Distinct().ToList();
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> ObterAnosTurmasPorUeModalidade(string codigoUe, Modalidade modalidade, bool consideraHistorico)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterAnosTurmasPorCodigoUeModalidade(login, perfil, codigoUe, modalidade, consideraHistorico);

            if (retorno != null && retorno.Any())
                return TransformarAnosEmOpcoesDropdownDto(retorno.OrderBy(q => q), modalidade);
            else
                return Enumerable.Empty<OpcaoDropdownDto>();
        }

        private IEnumerable<OpcaoDropdownDto> TransformarAnosEmOpcoesDropdownDto(IEnumerable<string> anos, Modalidade modalidade)
        {
            string descModalidade = modalidade.GetAttribute<DisplayAttribute>().Name;
            int anoInt;

            foreach (var ano in anos)
            {
                if (int.TryParse(ano, out anoInt) && anoInt > 0)
                    yield return new OpcaoDropdownDto(ano, $"{ano}º ano - {descModalidade}");
            }
        }

        public Task<IEnumerable<int>> ObterAnosLetivosTodos()
        {
            int qtdeRange = 0;
            int anoInicio = 2014;
            for (int i = anoInicio; i <= DateTime.Today.Year; i++)
                qtdeRange++;
            var anos = Enumerable.Range(anoInicio, qtdeRange).OrderByDescending(x => x).AsEnumerable();
            return Task.FromResult(anos);
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "")
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var filtroEhCodigo = false;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                if (filtro.All(char.IsDigit))
                    filtroEhCodigo = true;
            }
            return await repositorioAbrangencia.ObterDres(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, filtro, filtroEhCodigo);
        }

        public async Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0, string dreCodigo = null, string ueCodigo = null)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterSemestres(login, perfil, modalidade, consideraHistorico, anoLetivo, dreCodigo, ueCodigo);

            return retorno
                    .Where(a => a != 0);
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasRegulares(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var result = await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
            var turmasRegulares = await servicoEOL.DefinirTurmasRegulares(result.Select(x => x.Codigo).ToArray());

            return result.Where(r => turmasRegulares.Contains(r.Codigo));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPrograma(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var result = await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
            var turmasPrograma = await mediator.Send(new ObterTurmasProgramaQuery(result.Select(x => x.Codigo).ToArray()));

            return result.Where(r => turmasPrograma.Contains(r.Codigo));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return (await repositorioAbrangencia.ObterUes(codigoDre, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo)).OrderBy(c => c.Nome).ToList();
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool consideraNovosAnosInfantil = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var anosInfantilDesconsiderar = !consideraNovosAnosInfantil ? await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(anoLetivo, Modalidade.EducacaoInfantil)) : null;
            
            if (tipos.Any())
            {
                var turmasitinerarioEnsinoMedio = await mediator.Send(new ObterTurmaItinerarioEnsinoMedioQuery());
                tipos = tipos.Concat(turmasitinerarioEnsinoMedio.Select(s => s.Id).ToArray()).ToArray();
            }            

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(codigoUe, login, perfil, modalidade, tipos.Any() ? tipos : null, periodo, consideraHistorico, anoLetivo, anosInfantilDesconsiderar);

            return OrdernarTurmasItinerario(result);
        }

        private IEnumerable<AbrangenciaTurmaRetorno> OrdernarTurmasItinerario(IEnumerable<AbrangenciaTurmaRetorno> result)
        {
            List<AbrangenciaTurmaRetorno> turmasOrdenadas = new List<AbrangenciaTurmaRetorno>();

            var turmasItinerario = result.Where(t => t.TipoTurma == (int)TipoTurma.Itinerarios2AAno || t.TipoTurma == (int)TipoTurma.ItinerarioEnsMedio);
            var turmas = result.Where(t => !turmasItinerario.Any(ti => ti.Id == t.Id));

            turmasOrdenadas.AddRange(turmas.OrderBy(a => a.ModalidadeTurmaNome));
            turmasOrdenadas.AddRange(turmasItinerario.OrderBy(a => a.ModalidadeTurmaNome));

            return turmasOrdenadas;
        }
    }
}