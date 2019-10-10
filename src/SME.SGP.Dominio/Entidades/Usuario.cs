using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public void DefinirEmail(string novoEmail, IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            if (perfisUsuario != null && perfisUsuario.Any() &&
                (PossuiPerfilDre(perfisUsuario) ||
                 PossuiPerfilSme(perfisUsuario)) &&
                !novoEmail.Contains("@sme.prefeitura.sp.gov.br"))
            {
                throw new NegocioException("Usuários da SME ou DRE devem utilizar e-mail profissional. Ex: usuario@sme.prefeitura.sp.gov.br");
            }
            Email = novoEmail;
        }

        public void FinalizarRecuperacaoSenha()
        {
            TokenRecuperacaoSenha = null;
            ExpiracaoRecuperacaoSenha = null;
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
            if (perfisUsuario == null || !perfisUsuario.Any())
            {
                return Guid.Empty;
            }
            var possuiPerfilPrioritario = perfisUsuario.OrderBy(c => c.Ordem).Any(c => c.CodigoPerfil == PERFIL_PROFESSOR);
            if (possuiPerfilPrioritario)
            {
                return PERFIL_PROFESSOR;
            }
            return perfisUsuario.FirstOrDefault().CodigoPerfil;
        }

        public bool PodeReiniciarSenha()
        {
            return !string.IsNullOrEmpty(Email);
        }

        public bool PossuiPerfilDre(IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            return perfisUsuario.Any(c => c.Tipo == TipoPerfil.DRE);
        }

        public bool PossuiPerfilSme(IEnumerable<PrioridadePerfil> perfisUsuario)
        {
            return perfisUsuario.Any(c => c.Tipo == TipoPerfil.SME);
        }

        public bool TokenRecuperacaoSenhaEstaValido()
        {
            return ExpiracaoRecuperacaoSenha > DateTime.Now;
        }

        public void ValidarSenha(string novaSenha)
        {
            if (novaSenha.Length < 8)
                throw new NegocioException("A senha deve conter no minimo 8 caracteres.");

            if (novaSenha.Length > 12)
                throw new NegocioException("A senha deve conter no máximo 12 caracteres.");

            if (novaSenha.Contains(" "))
                throw new NegocioException("A senhão não pode conter espaço em branco.");

            var regexSenha = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=((?=.*[!@#$\-%&/\\\[\]|*()_=+])|(?=.*?[0-9]+)))");
            if (!regexSenha.IsMatch(novaSenha))
                throw new NegocioException("A senha deve conter pelo menos 1 letra maiúscula, 1 minúscula, 1 número e/ou 1 caractere especial.");
        }
    }
}