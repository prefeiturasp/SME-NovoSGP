using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoPendenciasFechamentoCommand : IRequest<bool>
    {
        public ExcluirNotificacaoPendenciasFechamentoCommand(string turmaCodigo, int ano)
        {
            TurmaCodigo = turmaCodigo;
            Ano = ano;
        }

        public string TurmaCodigo { get; set;  }
        public int Ano { get; set; }
    }
}
