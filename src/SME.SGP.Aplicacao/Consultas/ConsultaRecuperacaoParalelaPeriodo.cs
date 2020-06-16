using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultaRecuperacaoParalelaPeriodo : IConsultaRecuperacaoParalelaPeriodo
    {
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo;        

        public ConsultaRecuperacaoParalelaPeriodo(IServicoEol servicoEOL, IServicoUsuario servicoUsuario,
            IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioRecuperacaoParalelaPeriodo = repositorioRecuperacaoParalelaPeriodo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodo));
        }

        public async Task<IEnumerable<RecuperacaoParalelaPeriodoPAPDto>> BuscarListaPeriodos(string turmaId)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var turmaPossuiComponente = await servicoEOL.TurmaPossuiComponenteCurricularPAP(turmaId, usuarioLogado.Login, usuarioLogado.PerfilAtual);

            if (!turmaPossuiComponente)
                return null;

            return repositorioRecuperacaoParalelaPeriodo.Listar().Select(x => (RecuperacaoParalelaPeriodoPAPDto)x);            
        }
    }
}
