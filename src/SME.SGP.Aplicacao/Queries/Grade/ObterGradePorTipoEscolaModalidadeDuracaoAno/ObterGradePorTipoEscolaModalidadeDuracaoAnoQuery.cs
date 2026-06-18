using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery : IRequest<Grade>
    {
        public ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery(TipoEscola tipoEscola, Modalidade modalidade, int duracao, int ano, string anoLetivo)
        {
            TipoEscola = tipoEscola;
            Modalidade = modalidade;
            Duracao = duracao;
            Ano = ano;
            AnoLetivo = anoLetivo;
        }

        public TipoEscola TipoEscola { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Duracao { get; set; }
        public int Ano { get; set; }
        public string AnoLetivo { get; set; }
    }
}
