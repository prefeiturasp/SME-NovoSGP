using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SME.SGP.Dominio
{
    public class Usuario : EntidadeBase
    {
        public string CodigoRf { get; set; }
        public string CPF { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        private IList<Notificacao> notificacoes { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public void validarSenha()
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

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }



    }
}
