using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAtribuicoes : IConsultasAtribuicoes
    {
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IMediator mediator;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioUeConsulta repositorioUe;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        private const int ANO_LETIVO_MINIMO = 2014;

        public ConsultasAtribuicoes(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                                    IRepositorioDreConsulta repositorioDre,
                                    IServicoEol servicoEol,
                                    IRepositorioUeConsulta repositorioUe,
                                    IServicoUsuario servicoUsuario,
                                    IConsultasAbrangencia consultasAbrangencia,
                                    IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.servicoEOL = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var somenteInfantil = perfilAtual == Perfis.PERFIL_CJ_INFANTIL;
                var anosLetivos = new List<int>();

                if (!consideraHistorico)
                    await ObterAtribuicoesEolAnosLetivos(loginAtual, perfilAtual, anosLetivos);

                if (somenteInfantil)
                    ObterAtribuicoesCjAnosLetivos(loginAtual, anosLetivos, Modalidade.EducacaoInfantil, consideraHistorico);
                else
                    ObterAtribuicoesCjAnosLetivos(loginAtual, anosLetivos, historico: consideraHistorico);

                anosLetivos.AddRange(await mediator
                    .Send(new ObterAnosAtribuicaoCJQuery(loginAtual, consideraHistorico)));

                return anosLetivos.Distinct();
            }
            else
                return await consultasAbrangencia.ObterAnosLetivos(consideraHistorico, ANO_LETIVO_MINIMO);
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(int anoLetivo, bool consideraHistorico)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (anoLetivo == 0)
                anoLetivo = DateTime.Now.Year;

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var somenteInfantil = perfilAtual == Perfis.PERFIL_CJ_INFANTIL;
                var codigosDres = new List<string>();

                if (!consideraHistorico)
                    await ObterAtribuicoesEolDre(loginAtual, perfilAtual, codigosDres);

                if (somenteInfantil)
                    ObterAtribuicoesCjDre(loginAtual, codigosDres, anoLetivo, Modalidade.EducacaoInfantil, historico: consideraHistorico);
                else
                    ObterAtribuicoesCjDre(loginAtual, codigosDres, anoLetivo, historico: consideraHistorico);

                await ObterAtribuicoesEsporadicasDreAsync(loginAtual, codigosDres, somenteInfantil, anoLetivo, consideraHistorico);

                var dres = repositorioDre
                    .ListarPorCodigos(codigosDres.Distinct().ToArray());

                if (dres.NaoEhNulo() && dres.Any())
                    return TransformarDresEmDresDto(dres);
                else
                    return null;
            }
            else
                return await consultasAbrangencia.ObterDres(null, 0, consideraHistorico, anoLetivo);
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, int anoLetivo, bool consideraHistorico)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (anoLetivo == 0)
                anoLetivo = DateTime.Now.Year;

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var codigosUes = new List<string>();
                var somenteInfantil = perfilAtual == Perfis.PERFIL_CJ_INFANTIL;

                if (!consideraHistorico)
                    await ObterAtribuicoesEolUe(loginAtual, perfilAtual, codigosUes, codigoDre);

                if (somenteInfantil)
                    await ObterAtribuicoesCjUe(loginAtual, codigosUes, codigoDre, anoLetivo, Modalidade.EducacaoInfantil, consideraHistorico);
                else
                    await ObterAtribuicoesCjUe(loginAtual, codigosUes, codigoDre, anoLetivo, historico: consideraHistorico);

                await ObterAtribuicoesEsporadicasUeAsync(loginAtual, codigosUes, codigoDre, somenteInfantil, anoLetivo, consideraHistorico);

                // A atribuição CJ ainda não irá contemplar os novos tipos de UEs na listagem
                var novosTiposUe = await mediator.Send(new ObterNovosTiposUEPorAnoQuery(DateTime.Today.Year)); // para atribuição CJ considera o ano letivo atual
                var ues = from ue in repositorioUe.ListarPorCodigos(codigosUes.Distinct().ToArray())
                          where !novosTiposUe.Split(',').Contains(((int)ue.TipoEscola).ToString())
                          select ue;

                if (ues.NaoEhNulo() && ues.Any())
                    return TransformarUesEmUesDto(ues);
                else
                    return null;
            }
            else
                return await consultasAbrangencia.ObterUes(codigoDre, null, 0, consideraHistorico, anoLetivo);
        }

        private void ObterAtribuicoesCjAnosLetivos(string professorRf, List<int> anosLetivos, Modalidade? modalidade = null, bool historico = false)
        {
            var atribuicoesCjAtivas = repositorioAtribuicaoCJ
                .ObterAtribuicaoAtiva(professorRf, historico);

            anosLetivos.AddRange((from acj in atribuicoesCjAtivas
                                  where !modalidade.HasValue || (modalidade.Value == acj.Modalidade)
                                  select acj.Turma.AnoLetivo).Distinct());
        }

        private void ObterAtribuicoesCjDre(string professorRf, List<string> codigosDres, int anoLetivo, Modalidade? modalidade = null, bool historico = false)
        {
            var atribuicoesCjAtivas = repositorioAtribuicaoCJ
                .ObterAtribuicaoAtiva(professorRf, historico);

            codigosDres.AddRange((from acj in atribuicoesCjAtivas
                                  where (!modalidade.HasValue || (modalidade.Value == acj.Modalidade)) &&
                                        acj.Turma.AnoLetivo == anoLetivo
                                  select acj.DreId).Distinct());
        }

        private async Task ObterAtribuicoesCjUe(string professorRf, List<string> codigosUes, string codigoDre, int anoLetivo, Modalidade? modalidade = null, bool historico = false)
        {
            var atribuicoesCjAtivas = await repositorioAtribuicaoCJ
                .ObterPorFiltros(modalidade, string.Empty, string.Empty, 0, professorRf, string.Empty, true, codigoDre, anoLetivo: anoLetivo, historico: historico);

            codigosUes.AddRange((from acj in atribuicoesCjAtivas
                                 where !modalidade.HasValue || (modalidade.Value == acj.Modalidade)
                                 select acj.UeId).Distinct());
        }

        private async Task ObterAtribuicoesEolAnosLetivos(string professorRf, Guid perfil, List<int> anosLetivos)
        {
            var abrangencia = await servicoEOL
                .ObterAbrangencia(professorRf, perfil);

            anosLetivos.AddRange((from dre in abrangencia.Dres
                                  from ue in dre.Ues
                                  from t in ue.Turmas
                                  select t.AnoLetivo).Distinct());
        }

        private async Task ObterAtribuicoesEolDre(string professorRf, Guid perfil, List<string> codigosDres)
        {
            var abrangencia = await servicoEOL
                .ObterAbrangencia(professorRf, perfil);

            codigosDres.AddRange((from dre in abrangencia.Dres
                                  select dre.Codigo).Distinct());
        }

        private async Task ObterAtribuicoesEolUe(string professorRf, Guid perfil, List<string> codigosUes, string codigoDre)
        {
            var abrangencia = await servicoEOL
                .ObterAbrangencia(professorRf, perfil);

            codigosUes.AddRange((from dre in abrangencia.Dres
                                 from ue in dre.Ues
                                 select ue.Codigo).Distinct());
        }

        private async Task ObterAtribuicoesEsporadicasDreAsync(string professorRf, List<string> codigosDres, bool somenteInfantil, int anoLetivo, bool historico)
        {
            var atribuicoesEsporadicas = await mediator
                .Send(new ObterAtribuicoesPorRFEAnoQuery(professorRf, somenteInfantil, anoLetivo, historico: historico));

            if (atribuicoesEsporadicas.NaoEhNulo() && atribuicoesEsporadicas.Any())
            {
                foreach (var atribuicaoEsporadica in atribuicoesEsporadicas)
                {
                    if (!codigosDres.Any(a => a == atribuicaoEsporadica.DreId))
                        codigosDres.Add(atribuicaoEsporadica.DreId);
                }
            }
        }

        private async Task ObterAtribuicoesEsporadicasUeAsync(string professorRf, List<string> codigosUes, string codigoDre, bool somenteInfantil, int anoLetivo, bool historico)
        {
            var atribuicoesEsporadicas = await mediator
                .Send(new ObterAtribuicoesPorRFEAnoQuery(professorRf, somenteInfantil, anoLetivo, historico: historico));

            if (atribuicoesEsporadicas.NaoEhNulo() && atribuicoesEsporadicas.Any())
            {
                foreach (var atribuicaoEsporadica in atribuicoesEsporadicas)
                {
                    if (atribuicaoEsporadica.DreId == codigoDre && (!codigosUes.Any(a => a == atribuicaoEsporadica.UeId)))
                        codigosUes.Add(atribuicaoEsporadica.UeId);
                }
            }
        }

        private AbrangenciaDreRetornoDto TransformaDreEmDto(Dre dre)
        {
            return new AbrangenciaDreRetornoDto()
            {
                Abreviacao = dre.Abreviacao,
                Codigo = dre.CodigoDre,
                Nome = dre.Nome
            };
        }

        private IEnumerable<AbrangenciaDreRetornoDto> TransformarDresEmDresDto(IEnumerable<Dre> dres)
        {
            foreach (var dre in dres)
                yield return TransformaDreEmDto(dre);
        }

        private IEnumerable<AbrangenciaUeRetorno> TransformarUesEmUesDto(IEnumerable<Ue> ues)
        {
            foreach (var ue in ues)
            {
                yield return new AbrangenciaUeRetorno()
                {
                    Codigo = ue.CodigoUe,
                    NomeSimples = ue.Nome,
                    TipoEscola = ue.TipoEscola
                };
            }
        }
    }
}