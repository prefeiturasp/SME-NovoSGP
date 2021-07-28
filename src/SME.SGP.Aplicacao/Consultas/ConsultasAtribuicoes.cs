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
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioUe repositorioUe;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ConsultasAtribuicoes(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IRepositorioDre repositorioDre, IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica,
            IServicoEol servicoEol, IRepositorioUe repositorioUe, IServicoUsuario servicoUsuario, IConsultasAbrangencia consultasAbrangencia, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoEOL = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(int anoLetivo)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var somenteInfantil = perfilAtual == Perfis.PERFIL_CJ_INFANTIL;
                var codigosDres = new List<string>();

                if (somenteInfantil)
                    ObterAtribuicoesCjDre(loginAtual, codigosDres, Modalidade.EducacaoInfantil);
                else
                    ObterAtribuicoesCjDre(loginAtual, codigosDres);

                if (anoLetivo == 0)
                    anoLetivo = DateTime.Now.Year;

                await ObterAtribuicoesEsporadicasDreAsync(loginAtual, codigosDres, somenteInfantil, anoLetivo);
                await ObterAtribuicoesEolDre(loginAtual, perfilAtual, codigosDres);

                var dres = repositorioDre.ListarPorCodigos(codigosDres.Distinct().ToArray());

                if (dres != null && dres.Any())
                    return TransformarDresEmDresDto(dres);
                else return null;
            }
            else
                return await consultasAbrangencia.ObterDres(null, 0, anoLetivo != DateTime.Now.Year, anoLetivo);
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, int anoLetivo)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (anoLetivo == 0)
                anoLetivo = DateTime.Now.Year;

            if (perfilAtual == Perfis.PERFIL_CJ || perfilAtual == Perfis.PERFIL_CJ_INFANTIL)
            {
                var codigosUes = new List<string>();
                var somenteInfantil = perfilAtual == Perfis.PERFIL_CJ_INFANTIL;

                if (anoLetivo == DateTime.Now.Year)
                {
                    await ObterAtribuicoesEolUe(loginAtual, perfilAtual, codigosUes, codigoDre);

                    if (somenteInfantil)
                        await ObterAtribuicoesCjUe(loginAtual, codigosUes, codigoDre, Modalidade.EducacaoInfantil);
                    else
                        await ObterAtribuicoesCjUe(loginAtual, codigosUes, codigoDre);
                }    

                await ObterAtribuicoesEsporadicasUeAsync(loginAtual, codigosUes, codigoDre, somenteInfantil, anoLetivo);

                IEnumerable<Ue> ues = repositorioUe.ListarPorCodigos(codigosUes.Distinct().ToArray());

                if (ues != null && ues.Any())
                    return TransformarUesEmUesDto(ues);
                else return null;
            }
            else return await consultasAbrangencia.ObterUes(codigoDre, null, 0, anoLetivo != DateTime.Now.Year, anoLetivo);
        }

        private void ObterAtribuicoesCjDre(string professorRf, List<string> codigosDres, Modalidade? modalidade = null)
        {
            var atribuicoesCjAtivas = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(professorRf);

            if (atribuicoesCjAtivas != null && atribuicoesCjAtivas.Any())
            {
                codigosDres.AddRange(atribuicoesCjAtivas.Where(c => modalidade.HasValue ? c.Modalidade == modalidade : true).Select(a => a.DreId).Distinct());
            }
        }

        private async Task ObterAtribuicoesCjUe(string professorRf, List<string> codigosUes, string codigoDre, Modalidade? modalidade = null)
        {
            var atribuicoesCjAtivas = await repositorioAtribuicaoCJ.ObterPorFiltros(modalidade, string.Empty, string.Empty, 0, professorRf, string.Empty, true, codigoDre);

            if (atribuicoesCjAtivas != null && atribuicoesCjAtivas.Any())
            {
                codigosUes.AddRange(atribuicoesCjAtivas.Select(a => a.UeId).Distinct());
            }
        }

        private async Task ObterAtribuicoesEolDre(string professorRf, Guid perfil, List<string> codigosDres)
        {
            var abrangencia = await servicoEOL.ObterAbrangencia(professorRf, perfil);

            if (abrangencia != null && abrangencia.Dres != null && abrangencia.Dres.Any())
            {
                codigosDres.AddRange(abrangencia.Dres.Select(a => a.Codigo).Distinct());
            }
        }

        private async Task ObterAtribuicoesEolUe(string professorRf, Guid perfil, List<string> codigosUes, string codigoDre)
        {
            var abrangencia = await servicoEOL.ObterAbrangencia(professorRf, perfil);

            if (abrangencia != null && abrangencia.Dres != null && abrangencia.Dres.Any(a => a.Codigo == codigoDre))
            {
                codigosUes.AddRange(abrangencia.Dres.FirstOrDefault(a => a.Codigo == codigoDre).Ues.Select(a => a.Codigo));
            }
        }

        private async Task ObterAtribuicoesEsporadicasDreAsync(string professorRf, List<string> codigosDres, bool somenteInfantil, int anoLetivo)
        {
            var atribuicoesEsporadicas = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(professorRf, somenteInfantil, anoLetivo));
            if (atribuicoesEsporadicas != null && atribuicoesEsporadicas.Any())
            {
                foreach (var atribuicaoEsporadica in atribuicoesEsporadicas)
                {
                    if (!codigosDres.Any(a => a == atribuicaoEsporadica.DreId))
                    {
                        codigosDres.Add(atribuicaoEsporadica.DreId);
                    }
                }
            }
        }

        private async Task ObterAtribuicoesEsporadicasUeAsync(string professorRf, List<string> codigosUes, string codigoDre, bool somenteInfantil, int anoLetivo)
        {
            
            var atribuicoesEsporadicas = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(professorRf, somenteInfantil, anoLetivo));
            if (atribuicoesEsporadicas != null && atribuicoesEsporadicas.Any())
            {
                foreach(var atribuicaoEsporadica in atribuicoesEsporadicas)
                {
                    if (atribuicaoEsporadica.DreId == codigoDre && (!codigosUes.Any(a => a == atribuicaoEsporadica.UeId)))
                    {
                        codigosUes.Add(atribuicaoEsporadica.UeId);
                    }
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
            {
                yield return TransformaDreEmDto(dre);
            }
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