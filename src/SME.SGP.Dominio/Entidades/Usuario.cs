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
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Login { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        public Guid? TokenRecuperacaoSenha { get; set; }
        public string Senha { get; set; }
        private IList<Notificacao> notificacoes { get; set; }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
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

        public void ValidarSenha()
        {
            var RFCPF = string.IsNullOrWhiteSpace(CodigoRf) ? CPF : CodigoRf;

            var SenhasPadrao = new List<string> { $"Sgp{RFCPF.Substring(RFCPF.Length - 4)}", RFCPF.Substring(RFCPF.Length - 4) };

            var regexSenha = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=((?=.*[!@#$\-%&/\\\[\]|*()_=+])|(?=.*?[0-9]+)))");

            var regexEspacoBranco = new Regex(@"([\s])");

            if (Senha.Length < 8)
                throw new NegocioException("A senha deve conter no minimo 8 caracteres");

            if (Senha.Length > 12)
                throw new NegocioException("A senha deve conter no maximo 12 caracteres");

            if (SenhasPadrao.Any(x => x.Equals(Senha)))
                throw new NegocioException("Não pode ser informado a senha padrão");

            if (regexEspacoBranco.IsMatch(Senha))
                throw new NegocioException("A senhão não pode conter espaço em branco");

            if (!regexSenha.IsMatch(Senha))
                throw new NegocioException("A senha deve conter pelo menos 1 letra Maiuscula, 1 minuscula, 1 numero e/ou caractere especial");
        }
        public bool TokenRecuperacaoSenhaEstaValido()
        {
            return ExpiracaoRecuperacaoSenha > DateTime.Now;
        }
    }
}