using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery, IEnumerable<DisciplinaNomeDto>>
    {
        private readonly IMediator mediator;
        
        public ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaNomeDto>> Handle(ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery request, CancellationToken cancellationToken)
        {

            if (request.UsuarioLogado.EhProfessorCj())
                return await ObterComponentesAtribuicaoCj(request.TurmaCodigo, request.UsuarioLogado.CodigoRf);
            else
                return await ObterComponentesCurricularesUsuario(request.TurmaCodigo, request.UsuarioLogado.CodigoRf ?? request.UsuarioLogado.Login, request.UsuarioLogado.PerfilAtual);
        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesCurricularesUsuario(string turmaCodigo, string codigoRf, Guid perfilAtual)
        {
            var obterTurma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            bool realizarAgrupamentoComponente = obterTurma.AnoLetivo != DateTimeExtension.HorarioBrasilia().Year;
            var componentesCurricularesEol = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, codigoRf,
                                                               perfilAtual,
                                                               realizarAgrupamentoComponente));

            if (componentesCurricularesEol == null || !componentesCurricularesEol.Any())
                return null;

            return componentesCurricularesEol.Select(cc => new DisciplinaNomeDto()
            {
                Codigo = cc.Codigo.ToString(),
                Nome = cc.ExibirComponenteEOL && 
                       obterTurma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? cc.DescricaoComponenteInfantil : cc.Descricao
            }).OrderBy(c => c.Nome)?.ToList();

        }

        private async Task<IEnumerable<DisciplinaNomeDto>> ObterComponentesAtribuicaoCj(string turmaCodigo, string login)
        {
            var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, turmaCodigo, string.Empty, 0, login, string.Empty, true));

            if (atribuicoes == null || !atribuicoes.Any())
                return null;

            var disciplinasEol = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(atribuicoes.Select(a => a.DisciplinaId).Distinct().ToArray()));

            return disciplinasEol.Select(cc => new DisciplinaNomeDto()
            {
                Codigo = cc.Id.ToString(),
                Nome = cc.Nome
            }).OrderBy(c => c.Nome)?.ToList();
        }


    }
}
