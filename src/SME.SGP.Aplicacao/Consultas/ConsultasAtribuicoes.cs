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

        public ConsultasAtribuicoes(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IRepositorioDre repositorioDre, IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica,
            IServicoEol servicoEol, IRepositorioUe repositorioUe, IServicoUsuario servicoUsuario, IConsultasAbrangencia consultasAbrangencia)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoEOL = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres()
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (perfilAtual == Perfis.PERFIL_CJ)
            {
                var codigosDres = new List<string>();

                ObterAtribuicoesCjDre(loginAtual, codigosDres);
                ObterAtribuicoesEsporadicasDre(loginAtual, codigosDres);
                await ObterAtribuicoesEolDre(loginAtual, codigosDres);

                var dres = repositorioDre.ListarPorCodigos(codigosDres.Distinct().ToArray());

                if (dres != null && dres.Any())
                    return TransformarDresEmDresDto(dres);
                else return null;
            }
            else
                return await consultasAbrangencia.ObterDres(null, 0);
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre)
        {
            var loginAtual = servicoUsuario.ObterLoginAtual();
            var perfilAtual = servicoUsuario.ObterPerfilAtual();

            if (perfilAtual == Perfis.PERFIL_CJ)
            {
                var codigosUes = new List<string>();
                await ObterAtribuicoesCjUe(loginAtual, codigosUes, codigoDre);
                ObterAtribuicoesEsporadicasUe(loginAtual, codigosUes, codigoDre);
                await ObterAtribuicoesEolUe(loginAtual, codigosUes, codigoDre);

                IEnumerable<Ue> ues = repositorioUe.ListarPorCodigos(codigosUes.Distinct().ToArray());

                if (ues != null && ues.Any())
                    return TransformarUesEmUesDto(ues);
                else return null;
            }
            else return await consultasAbrangencia.ObterUes(codigoDre, null, 0);
        }

        private void ObterAtribuicoesCjDre(string professorRf, List<string> codigosDres)
        {
            var atribuicoesCjAtivas = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(professorRf);

            if (atribuicoesCjAtivas != null && atribuicoesCjAtivas.Any())
            {
                codigosDres.AddRange(atribuicoesCjAtivas.Select(a => a.DreId).Distinct());
            }
        }

        private async Task ObterAtribuicoesCjUe(string professorRf, List<string> codigosUes, string codigoDre)
        {
            var atribuicoesCjAtivas = await repositorioAtribuicaoCJ.ObterPorFiltros(null, string.Empty, string.Empty, 0, professorRf, string.Empty, true, codigoDre);

            if (atribuicoesCjAtivas != null && atribuicoesCjAtivas.Any())
            {
                codigosUes.AddRange(atribuicoesCjAtivas.Select(a => a.UeId).Distinct());
            }
        }

        private async Task ObterAtribuicoesEolDre(string professorRf, List<string> codigosDres)
        {
            var abrangencia = await servicoEOL.ObterAbrangencia(professorRf, Perfis.PERFIL_CJ);

            if (abrangencia != null && abrangencia.Dres != null && abrangencia.Dres.Any())
            {
                codigosDres.AddRange(abrangencia.Dres.Select(a => a.Codigo).Distinct());
            }
        }

        private async Task ObterAtribuicoesEolUe(string professorRf, List<string> codigosUes, string codigoDre)
        {
            var abrangencia = await servicoEOL.ObterAbrangencia(professorRf, Perfis.PERFIL_CJ);

            if (abrangencia != null && abrangencia.Dres != null && abrangencia.Dres.Any(a => a.Codigo == codigoDre))
            {
                codigosUes.AddRange(abrangencia.Dres.FirstOrDefault(a => a.Codigo == codigoDre).Ues.Select(a => a.Codigo));
            }
        }

        private void ObterAtribuicoesEsporadicasDre(string professorRf, List<string> codigosDres)
        {
            var atribuicaoEsporadica = repositorioAtribuicaoEsporadica.ObterUltimaPorRF(professorRf);
            if (atribuicaoEsporadica != null && !codigosDres.Any(a => a == atribuicaoEsporadica.DreId))
            {
                codigosDres.Add(atribuicaoEsporadica.DreId);
            }
        }

        private void ObterAtribuicoesEsporadicasUe(string professorRf, List<string> codigosUes, string codigoDre)
        {
            if (codigosUes == null || !codigosUes.Any())
                return;
            else
            {
                var atribuicaoEsporadica = repositorioAtribuicaoEsporadica.ObterUltimaPorRF(professorRf);
                if (atribuicaoEsporadica != null && atribuicaoEsporadica.DreId == codigoDre
                    && (codigosUes.Any(a => a != atribuicaoEsporadica.UeId)))
                {
                    codigosUes.Add(atribuicaoEsporadica.UeId);
                }
            }
        }

        private AbrangenciaDreRetorno TransformaDreEmDto(Dre dre)
        {
            return new AbrangenciaDreRetorno()
            {
                Abreviacao = dre.Abreviacao,
                Codigo = dre.CodigoDre,
                Nome = dre.Nome
            };
        }

        private IEnumerable<AbrangenciaDreRetorno> TransformarDresEmDresDto(IEnumerable<Dre> dres)
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
                    Nome = ue.Nome,
                    TipoEscola = ue.TipoEscola
                };
            }
        }
    }
}