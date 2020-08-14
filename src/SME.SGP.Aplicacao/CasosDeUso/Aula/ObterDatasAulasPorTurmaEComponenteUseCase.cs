using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasAulasPorTurmaEComponenteUseCase : AbstractUseCase, IObterDatasAulasPorTurmaEComponenteUseCase
    {
        private readonly IServicoUsuario servicoUsuario;

        public ObterDatasAulasPorTurmaEComponenteUseCase(IMediator mediator, IServicoUsuario servicoUsuario) : base(mediator) 
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<DatasAulasDto>> Executar(ConsultaDatasAulasDto param)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var professorRF = usuarioLogado.EhProfessor() && !usuarioLogado.EhProfessorInfantil() ? usuarioLogado.CodigoRf : string.Empty;

            return await mediator.Send(new ObterDatasAulasPorProfessorEComponenteQuery(professorRF, param.TurmaCodigo, param.ComponenteCurricularCodigo, usuarioLogado.EhProfessorCj(), usuarioLogado.EhProfessor() || usuarioLogado.EhProfessorCj()));
        }
    }
}
