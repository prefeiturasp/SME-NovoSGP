using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ConsultasUsuario : IConsultasUsuario
    {
        private readonly IServicoEOL servicoEOL;

        public ConsultasUsuario (IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
        }
        public async Task<MeusDadosDto> BuscarMeusDados(string login)
        {
            var meusDados = await servicoEOL.ObterMeusDados(login);
            return meusDados;
        }
    }
}
