using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaDres : IConsultaDres
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultaDres(IServicoEol servicoEOL,
            IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre,
            IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario)

        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new System.ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasPorDre(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var lista = from a in escolasPorDre
                        select new UnidadeEscolarDto()
                        {
                            Codigo = a.Codigo,
                            Nome = a.NomeSimples
                        };

            return lista;
        }

        public async Task<IEnumerable<UnidadeEscolarDto>> ObterEscolasSemAtribuicao(string dreId)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var escolasPorDre = await repositorioAbrangencia.ObterUes(dreId, login, perfil);

            var supervisoresEscolasDres = repositorioSupervisorEscolaDre.ObtemPorDreESupervisor(dreId, string.Empty);

            return TrataEscolasSemSupervisores(escolasPorDre, supervisoresEscolasDres);
        }

        public IEnumerable<DreConsultaDto> ObterTodos()
        {
            var respostaEol = servicoEOL.ObterDres();

            return MapearParaDto(respostaEol);
        }

        private IEnumerable<DreConsultaDto> MapearParaDto(IEnumerable<DreRespostaEolDto> respostaEol)
        {
            foreach (var item in respostaEol)
            {
                yield return new DreConsultaDto()
                {
                    Id = item.CodigoDRE,
                    Nome = item.NomeDRE,
                    Sigla = item.SiglaDRE
                };
            }
        }

        private IEnumerable<UnidadeEscolarDto> TrataEscolasSemSupervisores(IEnumerable<AbrangenciaUeRetorno> escolasPorDre,
                    IEnumerable<SupervisorEscolasDreDto> supervisoresEscolas)
        {
            var escolasComSupervisor = supervisoresEscolas?
                .Select(a => a.EscolaId)
                .ToList();

            List<AbrangenciaUeRetorno> escolasSemSupervisor = null;
            if (escolasComSupervisor != null)
            {
                escolasSemSupervisor = escolasPorDre?
                    .Where(a => !escolasComSupervisor.Contains(a.Codigo))
                    .ToList();
            }

            return escolasSemSupervisor?.OrderBy(c=>c.Nome).Select(t => new UnidadeEscolarDto() { Codigo = t.Codigo, Nome = t.Nome });
        }
    }
}