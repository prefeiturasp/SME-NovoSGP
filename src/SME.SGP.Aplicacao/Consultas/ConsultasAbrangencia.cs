﻿using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Enumerados;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAbrangencia : IConsultasAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoEol servicoEOL;

        public ConsultasAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario, IServicoEol servicoEOL)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
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

        public async Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterAnosLetivos(login, perfil, consideraHistorico);
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
            var anos = Enumerable.Range(2014, 8).OrderByDescending(x => x).AsEnumerable();

            return Task.FromResult(anos);
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterDres(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> ObterModalidades(int anoLetivo, bool consideraHistorico)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var lista = await repositorioAbrangencia.ObterModalidades(login, perfil, anoLetivo, consideraHistorico);

            var listaModalidades = from a in lista
                                   select new EnumeradoRetornoDto() { Id = a, Descricao = ((Modalidade)a).GetAttribute<DisplayAttribute>().Name };

            return listaModalidades.OrderBy(a => a.Descricao);
        }

        public async Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterSemestres(login, perfil, modalidade, consideraHistorico, anoLetivo);

            return retorno
                    .Where(a => a != 0);
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var result = await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);

            return result;
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return (await repositorioAbrangencia.ObterUes(codigoDre, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo)).OrderBy(c => c.Nome).ToList();
        }
    }
}