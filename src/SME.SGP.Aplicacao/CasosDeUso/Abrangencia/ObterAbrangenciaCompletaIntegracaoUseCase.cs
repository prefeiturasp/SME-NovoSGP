using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaCompletaIntegracaoUseCase : AbstractUseCase, IObterAbrangenciaCompletaIntegracaoUseCase
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterAbrangenciaCompletaIntegracaoUseCase(IMediator mediator, IRepositorioCache repositorioCache) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<AbrangenciaCompletaRetornoDto> Executar(string login, Guid perfil, bool consideraHistorico, int anoLetivo, int semestre, Modalidade modalidade, string codigoDre, string codigoUe, string codigoTurma, bool includeTurmas)
        {
            var buscarTurmas = includeTurmas || !string.IsNullOrWhiteSpace(codigoTurma);

            var chaveCache = string.Format(NomeChaveCache.ABRANGENCIA_COMPLETA_INTEGRACAO,
                login, perfil, consideraHistorico, anoLetivo, semestre, (int)modalidade, codigoDre, codigoUe, codigoTurma, buscarTurmas);

            return await repositorioCache.ObterAsync<AbrangenciaCompletaRetornoDto>(chaveCache, async () =>
            {
                var dres = await ObterDres(login, perfil, consideraHistorico, anoLetivo, semestre, modalidade, codigoDre);
                var ues = await ObterUes(login, perfil, consideraHistorico, anoLetivo, semestre, modalidade, codigoUe, dres);
                var turmas = await ObterTurmas(login, perfil, consideraHistorico, anoLetivo, semestre, modalidade, buscarTurmas, codigoTurma, ues);

                return new AbrangenciaCompletaRetornoDto
                {
                    Dres = dres,
                    Ues = ues,
                    Turmas = turmas
                };
            });
        }

        private async Task<List<AbrangenciaDreRetornoDto>> ObterDres(string login, Guid perfil, bool consideraHistorico, int anoLetivo, int semestre, Modalidade modalidade, string codigoDre)
        {
            var dres = (await mediator.Send(new ObterAbrangenciaDresQuery(login, perfil, modalidade, semestre, consideraHistorico, anoLetivo, "")))
                .OrderBy(d => d.Nome);

            if (!string.IsNullOrWhiteSpace(codigoDre))
                dres = dres.Where(d => d.Codigo == codigoDre).OrderBy(d => d.Nome);

            return dres.ToList();
        }

        private async Task<List<AbrangenciaUeIntegracaoRetornoDto>> ObterUes(string login, Guid perfil, bool consideraHistorico, int anoLetivo, int semestre, Modalidade modalidade, string codigoUe, List<AbrangenciaDreRetornoDto> dres)
        {
            var resultado = new List<AbrangenciaUeIntegracaoRetornoDto>();

            foreach (var dre in dres)
            {
                var dto = new UEsPorDreDto
                {
                    CodigoDre = dre.Codigo,
                    Modalidade = modalidade,
                    AnoLetivo = anoLetivo,
                    ConsideraHistorico = consideraHistorico,
                    Periodo = semestre,
                    ConsideraNovasUEs = false,
                    FiltrarTipoEscolaPorAnoLetivo = false,
                    Filtro = ""
                };

                var ues = (await mediator.Send(new ObterUEsPorDREQuery(dto, login, perfil))).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(codigoUe))
                    ues = ues.Where(u => u.Codigo == codigoUe);

                resultado.AddRange(ues.Select(u => new AbrangenciaUeIntegracaoRetornoDto
                {
                    Codigo = u.Codigo,
                    NomeSimples = u.NomeSimples,
                    TipoEscola = u.TipoEscola,
                    Id = u.Id,
                    Nome = u.Nome,
                    EhInfantil = u.EhInfantil,
                    CodigoDre = dre.Codigo
                }));
            }

            return resultado;
        }

        private async Task<List<AbrangenciaTurmaIntegracaoRetornoDto>> ObterTurmas(string login, Guid perfil, bool consideraHistorico, int anoLetivo, int semestre, Modalidade modalidade, bool includeTurmas, string codigoTurma, List<AbrangenciaUeIntegracaoRetornoDto> ues)
        {
            if (!includeTurmas)
                return new List<AbrangenciaTurmaIntegracaoRetornoDto>();

            var resultado = new List<AbrangenciaTurmaIntegracaoRetornoDto>();

            foreach (var ue in ues)
            {
                var turmas = (await mediator.Send(new ObterTurmasPorUeLoginPerfilQuery(
                    ue.Codigo, login, perfil, modalidade, semestre, consideraHistorico, anoLetivo))).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(codigoTurma))
                    turmas = turmas.Where(t => t.Codigo == codigoTurma);

                resultado.AddRange(turmas.Select(t => new AbrangenciaTurmaIntegracaoRetornoDto
                {
                    Codigo = t.Codigo,
                    Nome = t.Nome,
                    Ano = t.Ano,
                    AnoLetivo = t.AnoLetivo,
                    CodigoModalidade = t.CodigoModalidade,
                    Semestre = t.Semestre,
                    EnsinoEspecial = t.EnsinoEspecial,
                    Id = t.Id,
                    TipoTurma = t.TipoTurma,
                    CodigoUe = ue.Codigo
                }));
            }

            return resultado;
        }
    }
}
