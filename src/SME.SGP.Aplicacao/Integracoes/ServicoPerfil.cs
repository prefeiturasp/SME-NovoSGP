using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoPerfil : IServicoPerfil
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ServicoPerfil(IRepositorioPrioridadePerfil repositorioPrioridadePerfil, IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        private async Task<bool> VerificarProfCJSemTurmaTitular(string login, bool ehCJ, IEnumerable<Guid> perfis)
        {
            if (ehCJ && perfis.Contains(Perfis.PERFIL_PROFESSOR) && perfis.Contains(Perfis.PERFIL_CJ))
            {
                var lstTurmasAtribuidas = await repositorioAbrangencia.ObterAbrangenciaPorFiltro(String.Empty, login, Perfis.PERFIL_PROFESSOR, false);

                if (lstTurmasAtribuidas == null || !lstTurmasAtribuidas.Any())
                    return true;
            }

            return false;
        }

        public async Task<PerfisPorPrioridadeDto> DefinirPerfilPrioritario(IEnumerable<Guid> perfis, Usuario usuario, bool ehCJ)
        {
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfis);
            var possuiTurmaAtiva = repositorioAbrangencia.PossuiAbrangenciaTurmaAtivaPorLogin(usuario.Login);
            var ehProfCJSemTurmaTitular = await VerificarProfCJSemTurmaTitular(usuario.Login, ehCJ, perfis);

            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirPerfilAtual(usuario.ObterPerfilPrioritario(possuiTurmaAtiva, ehProfCJSemTurmaTitular));

            var perfisPorPrioridade = new PerfisPorPrioridadeDto
            {
                PerfilSelecionado = usuario.PerfilAtual,
                Perfis = MapearPerfisParaDto(perfisUsuario),
                PossuiPerfilSmeOuDre = usuario.PossuiPerfilSmeOuDre(),
                PossuiPerfilSme = usuario.PossuiPerfilSme(),
                PossuiPerfilDre = usuario.PossuiPerfilDre(),
                EhProfessor = usuario.EhProfessor(),
                EhProfessorCj = usuario.EhProfessorCj(),
                EhProfessorPoa = usuario.EhProfessorPoa()
            };
            return perfisPorPrioridade;
        }

        private IList<PerfilDto> MapearPerfisParaDto(IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            return perfisUsuario?.Select(c => new PerfilDto
            {
                CodigoPerfil = c.CodigoPerfil,
                NomePerfil = c.NomePerfil
            }).ToList();
        }
    }
}