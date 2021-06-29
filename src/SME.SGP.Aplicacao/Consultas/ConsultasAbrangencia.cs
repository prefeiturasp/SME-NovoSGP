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

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto, bool consideraHistorico)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterAbrangenciaPorFiltro(texto, login, perfil, consideraHistorico);
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

            return await mediator.Send(new ObterUsuarioAbrangenciaAnosLetivosQuery(login, consideraHistorico, perfil, anoMinimo));
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

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterDres(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
        }

        public async Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterSemestres(login, perfil, modalidade, consideraHistorico, anoLetivo);

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

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return (await repositorioAbrangencia.ObterUes(codigoDre, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo)).OrderBy(c => c.Nome).ToList();
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var anosInfantilDesconsiderar = modalidade == Modalidade.EducacaoInfantil && desconsideraNovosAnosInfantil ? await ObterAnosInfantilParaDesconsiderar(anoLetivo) : null;

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(codigoUe, login, perfil, modalidade, tipos.Any() ? tipos : null, periodo, consideraHistorico, anoLetivo, anosInfantilDesconsiderar);

            result.ToList().ForEach(a =>
            {
                var modalidadeEnum = (Modalidade)a.CodigoModalidade;
                a.ModalidadeTurmaNome = $"{modalidadeEnum.ShortName()} - {a.Nome}";
            });

            return OrdernarTurmasItinerario(result);
        }

        private async Task<string[]> ObterAnosInfantilParaDesconsiderar(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AgrupamentoTurmasFiltro, anoLetivo));
            if (parametro != null && !string.IsNullOrEmpty(parametro.Valor))
            {
                var modalidadesAnos = parametro.Valor.Split(';');
                Dictionary<int, string[]> dictionary = new Dictionary<int, string[]>();
                foreach (string modalidadeAno in modalidadesAnos)
                {
                    if (!string.IsNullOrEmpty(modalidadeAno))
                    {
                        string[] valor = modalidadeAno.Split('=');
                        dictionary.Add(int.Parse(valor[0]), valor[1].Split(','));
                    }
                }
                if (dictionary.ContainsKey(1))
                    return dictionary[1];
            }
            return null;
        }

        private IEnumerable<AbrangenciaTurmaRetorno> OrdernarTurmasItinerario(IEnumerable<AbrangenciaTurmaRetorno> result)
        {
            List<AbrangenciaTurmaRetorno> turmasOrdenadas = new List<AbrangenciaTurmaRetorno> ();

            var turmasItinerario = result.Where(t => t.TipoTurma == (int)TipoTurma.Itinerarios2AAno || t.TipoTurma == (int)TipoTurma.ItinerarioEnsMedio);
            var turmas = result.Where(t => !turmasItinerario.Any(ti => ti.Id == t.Id));
            
            turmasOrdenadas.AddRange(turmas.OrderBy(a => a.ModalidadeTurmaNome));
            turmasOrdenadas.AddRange(turmasItinerario.OrderBy(a => a.ModalidadeTurmaNome));

            return turmasOrdenadas;
        }
    }
}