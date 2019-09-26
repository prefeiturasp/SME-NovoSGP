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
        public string Senha { get; set; }

        public void validarSenha()
        {
            validarSenha(null);
        }

        public void validarSenha(List<string> ultimasSenhas)
        {
            var regexSenha = new Regex(@"(?=.*?[A-Z])(?=.*?[a-z])(?=((?=.*[!@#$\-%&/\\\[\]|*()_=+])|(?=.*?[0-9]+)))");

            var regexEspacoBranco = new Regex(@"([\s])");

            var RFCPF = string.IsNullOrWhiteSpace(CodigoRf) ? CPF : CodigoRf;

            var SenhasPadrao = new List<string> { $"Sgp{RFCPF.Substring(RFCPF.Length - 4)}", RFCPF.Substring(RFCPF.Length - 4) };

            if (SenhasPadrao.FirstOrDefault(x => x.Equals(Senha)) != null)
                throw new NegocioException("Não pode ser informado a senha padrão");

            if (regexEspacoBranco.IsMatch(Senha))
                throw new NegocioException("A senhão não pode conter espaço em branco");

            if (!regexSenha.IsMatch(Senha))
                throw new NegocioException("A senha deve conter pelo menos 1 letra Maiuscula, 1 minuscula, 1 numero e/ou caractere especial");

            if (ultimasSenhas != null && ultimasSenhas.FirstOrDefault(x => x.Equals(Senha)) != null)
                throw new NegocioException("A senha informada não pode ser uma das ultimas 5 senhas");
        }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }



    }
}
