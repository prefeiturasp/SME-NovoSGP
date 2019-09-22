using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Usuario : EntidadeBase
    {
        public string CodigoRf { get; set; }
        public DateTime ExpiracaoRecuperacaoSenha { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        public Guid TokenRecuperacaoSenha { get; set; }
        private IList<Notificacao> notificacoes { get; set; }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }

        public void IniciarRecuperacaoDeSenha()
        {
            //se não tiver e-mail cadastrado
            if (false)
            {
                //se for usuario com nivel UE
                throw new NegocioException("Você não tem um e-mail cadastrado para recuperar sua senha. Para restabelecer o seu acesso, procure o Diretor da sua unidade.");
                //se for usuario com nivel DRE ou SME
                throw new NegocioException("Você não tem um e-mail cadastrado para recuperar sua senha. Para restabelecer o seu acesso, procure o Administrador do SGP da sua unidade.");
            }
            TokenRecuperacaoSenha = Guid.NewGuid();
            ExpiracaoRecuperacaoSenha = DateTime.Now.AddHours(6);
        }
    }
}