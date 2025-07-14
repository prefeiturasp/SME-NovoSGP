using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class HistoricoEscolarObservacao : EntidadeBase
	{
        public HistoricoEscolarObservacao() { }

        public HistoricoEscolarObservacao(string alunoCodigo, string observacao)
        {
            AlunoCodigo = alunoCodigo;
            Observacao = observacao;
        }

        public string AlunoCodigo { get; set; }
        public string Observacao { get; set; }

        public void Alterar(string observacao)
        {
            Observacao = observacao;
        }
    }
}