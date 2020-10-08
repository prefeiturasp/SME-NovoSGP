using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirCodigoCorrelacaoCommand : IRequest<bool>
    {
        public InserirCodigoCorrelacaoCommand(Guid codigoCorrelacao, string usuarioRf, TipoRelatorio tipoRelatorio, TipoFormatoRelatorio formato)
        {
            CodigoCorrelacao = codigoCorrelacao;
            UsuarioRf = usuarioRf;
            TipoRelatorio = tipoRelatorio;
            Formato = formato;
        }

        public Guid CodigoCorrelacao { get; set; }

        public string UsuarioRf { get; set; }

        public TipoRelatorio TipoRelatorio { get; set; }

        public TipoFormatoRelatorio Formato { get; set; }
    }
}
