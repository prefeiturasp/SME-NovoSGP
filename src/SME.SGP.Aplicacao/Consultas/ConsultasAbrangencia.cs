using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
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
        private readonly IMediator mediator;
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;

        public ConsultasAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario, IMediator mediator,
                                    IRepositorioCacheAbrangencia repositorioCacheAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
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
            var abrangencia = await mediator.Send(new ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery(login, Guid.Parse(perfil.ToString())));
            bool abrangenciaPermitida = abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.UE
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.Dre
                                        || abrangencia.Abrangencia.Abrangencia == Infra.Enumerados.Abrangencia.SME;

            return await repositorioAbrangencia.ObterAbrangenciaTurma(turma, login, perfil, consideraHistorico, abrangenciaPermitida);
        }
        
        public async Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico, int anoMinimo, bool usarCache = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            List<int> anosLetivos = new List<int>();
            var anos = await mediator.Send(new ObterUsuarioAbrangenciaAnosLetivosQuery(login, consideraHistorico, perfil, anoMinimo) { UsarCache = usarCache });

            anosLetivos.AddRange(anos);

            if ((perfil == Perfis.PERFIL_CJ || perfil == Perfis.PERFIL_CJ_INFANTIL) && consideraHistorico)
            {
                var anosCJ = await mediator.Send(new ObterAnosAtribuicaoCJQuery(login, consideraHistorico));
                if (anosCJ.Any())
                    anosLetivos.AddRange(anosCJ);

            }
            anosLetivos.Add(DateTime.Now.Year);
            return anosLetivos.Distinct().OrderByDescending(a => a).ToList();
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> ObterAnosTurmasPorUeModalidade(string codigoUe, Modalidade modalidade, bool consideraHistorico,int? anoLetivo)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterAnosTurmasPorCodigoUeModalidade(login, perfil, codigoUe, modalidade, consideraHistorico,anoLetivo);

            if (retorno.NaoEhNulo() && retorno.Any())
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

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "", bool usarCache = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var filtroEhCodigo = !string.IsNullOrWhiteSpace(filtro) && filtro.All(char.IsDigit);

            Task<IEnumerable<AbrangenciaDreRetornoDto>> Buscar()
                => repositorioAbrangencia.ObterDres(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, filtro, filtroEhCodigo);

            var configuracaoCache = usarCache && string.IsNullOrWhiteSpace(filtro)
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance)
                : null;

            if (configuracaoCache.EhNulo())
                return await Buscar();

            // Mesma chave do ObterAbrangenciaDresQueryHandler (mesmos argumentos de RepositorioAbrangencia.ObterDres):
            //   login, perfil, modalidade (null = 0), periodo, consideraHistorico, anoLetivo. Sem cache com filtro de texto.
            var chave = string.Format(NomeChaveCache.ABRANGENCIA_DRES,
                login, perfil, (int)(modalidade ?? 0), periodo, consideraHistorico, anoLetivo);

            return await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(perfil));
        }

        public async Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0, string dreCodigo = null, string ueCodigo = null)
        {
            var login = servicoUsuario
                .ObterLoginAtual();

            var perfil = servicoUsuario
                .ObterPerfilAtual();

            var retorno = await repositorioAbrangencia
                .ObterSemestres(login, perfil, modalidade, consideraHistorico, anoLetivo, dreCodigo, ueCodigo);

            return retorno
                .Where(a => a != 0);
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasRegulares(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var result = await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
            IEnumerable<string> turmasTipo = null;

            if (modalidade.EhCELP())
                turmasTipo = await mediator.Send(new ObterTurmasProgramaQuery(result.Select(x => x.Codigo).ToArray()));
            else
                turmasTipo = await mediator.Send(new ObterTurmasRegularesQuery(result.Select(x => x.Codigo).ToArray()));

            return result.Where(r => turmasTipo.Contains(r.Codigo));
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPrograma(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var result = await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo);
            var turmasPrograma = await mediator.Send(new ObterTurmasProgramaQuery(result.Select(x => x.Codigo).ToArray()));

            return result.Where(r => turmasPrograma.Contains(r.Codigo));
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false, bool usarCache = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            Task<IEnumerable<AbrangenciaUeRetorno>> Buscar()
                => repositorioAbrangencia.ObterUes(codigoDre, login, perfil, new FiltroModalidade(modalidade ?? 0), new FiltroPeriodoLetivo(anoLetivo, consideraHistorico, periodo));

            var configuracaoCache = usarCache
                ? await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance)
                : null;

            IEnumerable<AbrangenciaUeRetorno> ues;

            if (configuracaoCache.EhNulo())
                ues = await Buscar();
            else
            {
                // Mesma chave do ObterUEsPorDREQueryHandler (mesmos argumentos de RepositorioAbrangencia.ObterUes):
                //   codigoDre, login, perfil, modalidade, anoLetivo, consideraHistorico, periodo, ignorarTiposUE.
                // Aqui ignorarTiposUE e filtroTexto não são passados ao repositório (null) → "null" na chave.
                var chave = string.Format(NomeChaveCache.ABRANGENCIA_UES,
                    login, perfil, codigoDre, (int)(modalidade ?? 0), anoLetivo, consideraHistorico, periodo,
                    ((IEnumerable<int>)null).ParaChaveCache());

                ues = await repositorioCacheAbrangencia.ObterComCacheAsync(chave, Buscar, configuracaoCache.ObterMinutosExpiracao(perfil));
            }

            return ues.OrderBy(c => c.Nome).ToList();
        }

        public async Task<IEnumerable<long>> ObterCodigoTurmasAbrangencia(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil = false)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();
            var anosInfantilDesconsiderar = !desconsideraNovosAnosInfantil ? await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(anoLetivo, Modalidade.EducacaoInfantil)) : null;

            var result = await repositorioAbrangencia.ObterTurmasPorTipos(
                codigoUe, 
                login, 
                perfil, 
                new FiltroModalidade(modalidade), 
                tipos.Any() ? tipos : null, 
                new FiltroPeriodoLetivo(anoLetivo, consideraHistorico, periodo), 
                anosInfantilDesconsiderar);
            
            var ordernarTurmasItinerario = OrdernarTurmasItinerario(result);
            return ordernarTurmasItinerario.Select(x => long.Parse(x.Codigo)).ToArray();
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

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasAbrangenciaMesmoComponente(IEnumerable<AbrangenciaTurmaRetorno> turmas, string codigoComponente)
        {
            List<AbrangenciaTurmaRetorno> turmasMesmoComponente = new List<AbrangenciaTurmaRetorno>();
            var componenteCurricular = await mediator.Send(new ObterComponenteCurricularPorIdQuery(long.Parse(codigoComponente)));

            foreach(var turma in turmas)
            {
                var disciplinasTurma = await mediator.Send(new ObterDisciplinasPorCodigoTurmaQuery(turma.Codigo));
                bool possuiMesmoComponente = disciplinasTurma.Any(d => d.CodigoComponenteCurricular.ToString() == codigoComponente  || 
                                                                  ((componenteCurricular?.TerritorioSaber ?? false) && 
                                                                   d.CodigoComponenteTerritorioSaber == componenteCurricular.CodigoComponenteCurricularTerritorioSaber));

                if (possuiMesmoComponente)
                    turmasMesmoComponente.Add(turma);
            }

            return turmasMesmoComponente;
        }
    }
}