using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Usuario : EntidadeBase
    {
        private readonly Guid PERFIL_PROFESSOR = Guid.Parse("40E1E074-37D6-E911-ABD6-F81654FE895D");
        public string CodigoRf { get; set; }
        public string Email { get; set; }
        public DateTime? ExpiracaoRecuperacaoSenha { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        public Guid? TokenRecuperacaoSenha { get; set; }
        public DateTime UltimoLogin { get; set; }
        private IList<Notificacao> notificacoes { get; set; }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }

        public void AtualizaUltimoLogin()
        {
            this.UltimoLogin = DateTime.Now;
        }

        public void IniciarRecuperacaoDeSenha()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new NegocioException("Você não tem um e-mail cadastrado para recuperar sua senha. Para restabelecer o seu acesso, procure o Diretor da sua UE ou Administrador do SGP da sua unidade.");
            }

            TokenRecuperacaoSenha = Guid.NewGuid();
            ExpiracaoRecuperacaoSenha = DateTime.Now.AddHours(6);
        }

        public Guid ObterPerfilPrioritario(IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            var possuiPerfilPrioritario = perfisUsuario.Any(c => c.CodigoPerfil == PERFIL_PROFESSOR);
            if (possuiPerfilPrioritario)
            {
                return PERFIL_PROFESSOR;
            }
            return perfisUsuario.FirstOrDefault().CodigoPerfil;
        }
    }
}