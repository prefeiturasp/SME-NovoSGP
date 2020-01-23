using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public PerfisPorPrioridadeDto DefinirPerfilPrioritario(IEnumerable<Guid> perfis, Usuario usuario)
        {
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfis);
            var possuiTurmaAtiva = repositorioAbrangencia.PossuiAbrangenciaTurmaAtivaPorLogin(usuario.Login);

            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirPerfilAtual(usuario.ObterPerfilPrioritario(possuiTurmaAtiva));

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