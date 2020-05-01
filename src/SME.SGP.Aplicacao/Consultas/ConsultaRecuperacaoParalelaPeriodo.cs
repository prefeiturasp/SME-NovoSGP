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
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo;
        private readonly IEnumerable<int> componentesCurricularesPAP = new List<int>
        {
            1322, 1033, 1051, 1052, 1053, 1054
        };

        public ConsultaRecuperacaoParalelaPeriodo(IServicoEOL servicoEOL, IServicoUsuario servicoUsuario, 
            IRepositorioRecuperacaoParalelaPeriodo repositorioRecuperacaoParalelaPeriodo)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioRecuperacaoParalelaPeriodo = repositorioRecuperacaoParalelaPeriodo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodo));
        }

        public async Task<IEnumerable<RecuperacaoParalelaPeriodoPAPDto>> BuscarListaPeriodos(string turmaId)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            var disciplinasTurma = await
                servicoEOL.
                ObterComponentesCurricularesPorCodigoTurmaLoginEPerfil(
                    turmaId, usuarioLogado.Login, usuarioLogado.PerfilAtual);

            if (disciplinasTurma is null || !disciplinasTurma.Any())
                return null;

            if (!componentesCurricularesPAP.Any(x => disciplinasTurma.Any(y => x == y.Codigo)))
                return null;

            return repositorioRecuperacaoParalelaPeriodo.Listar().Select(x => (RecuperacaoParalelaPeriodoPAPDto)x);
        }
    }
}
