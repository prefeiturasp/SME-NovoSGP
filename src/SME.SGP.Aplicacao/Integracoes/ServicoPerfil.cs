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

        private async Task<Guid> ObterPerfilPrioritarioCJSemTurmaTitular(string login, bool usuarioPerfilCJPrioritario, bool usuarioPerfilCJInfantilPrioritario)
        {
            IEnumerable<Dto.AbrangenciaFiltroRetorno> lstTurmasAtribuidasCJ;

            if (usuarioPerfilCJPrioritario)
            {
                lstTurmasAtribuidasCJ = await repositorioAbrangencia.ObterAbrangenciaPorFiltro(String.Empty, login, Perfis.PERFIL_PROFESSOR, false);

                if (lstTurmasAtribuidasCJ != null && lstTurmasAtribuidasCJ.Any())
                    return Perfis.PERFIL_PROFESSOR;
                else if (!usuarioPerfilCJInfantilPrioritario)
                    return Perfis.PERFIL_CJ;
            }

            if (usuarioPerfilCJInfantilPrioritario)
            {
                lstTurmasAtribuidasCJ = await repositorioAbrangencia.ObterAbrangenciaPorFiltro(String.Empty, login, Perfis.PERFIL_PROFESSOR_INFANTIL, false);

                if (lstTurmasAtribuidasCJ != null && lstTurmasAtribuidasCJ.Any())
                    return Perfis.PERFIL_PROFESSOR_INFANTIL;
                else if (!usuarioPerfilCJPrioritario)
                    return Perfis.PERFIL_CJ_INFANTIL;
                else
                    return Perfis.PERFIL_CJ;
            }

            return Guid.Empty;

        }

        public async Task<PerfisPorPrioridadeDto> DefinirPerfilPrioritario(IEnumerable<Guid> perfis, Usuario usuario)
        {
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfis);
            var possuiTurmaAtiva = repositorioAbrangencia.PossuiAbrangenciaTurmaAtivaPorLogin(usuario.Login);

            usuario.DefinirPerfis(perfisUsuario);

            var perfilProfCJSemTurmaTitular = await ObterPerfilPrioritarioCJSemTurmaTitular(usuario.Login, usuario.PossuiPerfilCJPrioritario(), usuario.PossuiPerfilCJInfantilPrioritario());
            usuario.DefinirPerfilAtual(usuario.ObterPerfilPrioritario(possuiTurmaAtiva, perfilProfCJSemTurmaTitular));

            var perfisPorPrioridade = new PerfisPorPrioridadeDto
            {
                PerfilSelecionado = usuario.PerfilAtual,
                Perfis = MapearPerfisParaDto(usuario.Perfis),
                PossuiPerfilSmeOuDre = usuario.PossuiPerfilSmeOuDre(),
                PossuiPerfilSme = usuario.PossuiPerfilSme(),
                PossuiPerfilDre = usuario.PossuiPerfilDre(),
                EhProfessor = usuario.EhProfessor(),
                EhProfessorCj = usuario.EhProfessorCj(),
                EhProfessorInfantil = usuario.EhProfessorInfantil(),
                EhProfessorCjInfantil = usuario.EhProfessorCjInfantil(),
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