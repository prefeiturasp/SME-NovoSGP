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
using MediatR;

namespace SME.SGP.Aplicacao.Consultas
{
    public class ConsultaRecuperacaoParalelaPeriodo : IConsultaRecuperacaoParalelaPeriodo
    {
        private readonly IMediator mediator;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo;        

        public ConsultaRecuperacaoParalelaPeriodo(IMediator mediator, IServicoUsuario servicoUsuario,
            IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioRecuperacaoParalelaPeriodo = repositorioRecuperacaoParalelaPeriodo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodo));
        }

        public async Task<IEnumerable<RecuperacaoParalelaPeriodoPAPDto>> BuscarListaPeriodos(string turmaId)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var turmaPossuiComponente = await mediator.Send(new TurmaPossuiComponenteCurricularPAPQuery(turmaId, usuarioLogado.Login, usuarioLogado.PerfilAtual));

            if (!turmaPossuiComponente)
                return null;

            return repositorioRecuperacaoParalelaPeriodo.Listar().Select(x => (RecuperacaoParalelaPeriodoPAPDto)x);            
        }
    }
}
