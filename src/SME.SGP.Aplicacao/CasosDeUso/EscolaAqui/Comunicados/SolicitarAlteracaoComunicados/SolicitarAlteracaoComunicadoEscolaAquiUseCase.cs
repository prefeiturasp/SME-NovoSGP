using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarAlteracaoComunicadoEscolaAquiUseCase : ISolicitarAlteracaoComunicadoEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public SolicitarAlteracaoComunicadoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Executar(long id, ComunicadoAlterarDto comunicado)
        {
            return await mediator.Send(new SolicitarAlteracaoComunicadoEscolaAquiCommand(
                  id
                , comunicado.DataEnvio
                , comunicado.DataExpiracao
                , comunicado.Descricao
                , comunicado.GruposId
                , comunicado.Titulo
                , comunicado.AnoLetivo
                , comunicado.SeriesResumidas
                , comunicado.CodigoDre
                , comunicado.CodigoUe
                , comunicado.AlunosEspecificados
                , comunicado.Modalidade
                , comunicado.Semestre
                , comunicado.Alunos
                , comunicado.Turmas
                , comunicado.TipoCalendarioId
                , comunicado.EventoId));
        }
    }
}
