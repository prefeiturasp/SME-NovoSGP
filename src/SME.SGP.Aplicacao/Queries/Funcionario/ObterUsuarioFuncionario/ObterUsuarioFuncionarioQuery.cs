using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioFuncionarioQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public string CodigoDre { get; set; }

        public string CodigoUe { get; set; }

        public string CodigoRf { get; set; }

        public string NomeServidor { get; set; }

        public ObterUsuarioFuncionarioQuery(FiltroFuncionarioDto dto)
        {
            CodigoDre = dto.CodigoDRE;
            CodigoUe = dto.CodigoUE;
            CodigoRf = dto.CodigoRF;
            NomeServidor = dto.NomeServidor;
        }
    }
}
