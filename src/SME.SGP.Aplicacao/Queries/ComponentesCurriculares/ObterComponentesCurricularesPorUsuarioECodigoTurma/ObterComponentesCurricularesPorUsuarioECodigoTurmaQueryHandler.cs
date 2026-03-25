using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorUsuarioECodigoTurmaQueryHandler : IRequestHandler<ObterComponentesCurricularesPorUsuarioECodigoTurmaQuery, IEnumerable<DisciplinaNomeDto>>
    {
        private readonly IMediator mediator;
        private readonly int ANO_LETIVO_INICIOU_AGRUPAMENTO = 2024;
        
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
            bool realizarAgrupamentoComponente = obterTurma.AnoLetivo <= ANO_LETIVO_INICIOU_AGRUPAMENTO;
            var componentesCurricularesEol = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, codigoRf,
                                                               perfilAtual,
                                                               realizarAgrupamentoComponente));

            if (componentesCurricularesEol.EhNulo() || !componentesCurricularesEol.Any())
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
            var dto = new AtribuicoesPorTurmaEProfessorDto()
            {
                TurmaId = turmaCodigo,
                UsuarioRf = login,
                Substituir = true
            };
            var atribuicoes = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(dto));

            if (atribuicoes.EhNulo() || !atribuicoes.Any())
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
