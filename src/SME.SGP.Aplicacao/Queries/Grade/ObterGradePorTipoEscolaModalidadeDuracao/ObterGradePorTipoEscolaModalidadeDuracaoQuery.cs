using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterGradePorTipoEscolaModalidadeDuracaoQuery: IRequest<GradeDto>
    {
        public ObterGradePorTipoEscolaModalidadeDuracaoQuery(TipoEscola tipoEscola, Modalidade modalidade, int duracao)
        {
            TipoEscola = tipoEscola;
            Modalidade = modalidade;
            Duracao = duracao;
        }

        public TipoEscola TipoEscola { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Duracao { get; set; }
    }
}
