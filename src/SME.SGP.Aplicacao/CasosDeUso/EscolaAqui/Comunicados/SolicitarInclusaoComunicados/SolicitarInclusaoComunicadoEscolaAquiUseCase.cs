using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarInclusaoComunicadoEscolaAquiUseCase : ISolicitarInclusaoComunicadoEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public SolicitarInclusaoComunicadoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string> Executar(ComunicadoInserirDto comunicado)
        {
            return await mediator.Send(new SolicitarInclusaoComunicadoEscolaAquiCommand(
                  comunicado.DataEnvio
                , comunicado.DataExpiracao
                , comunicado.Descricao
                , comunicado.GruposId
                , comunicado.Titulo
                , comunicado.AnoLetivo
                , comunicado.CodigoDre
                , comunicado.CodigoUe
                , comunicado.AlunosEspecificados
                , comunicado.Modalidade
                , comunicado.Semestre
                , comunicado.Alunos
                , comunicado.Turmas
                , comunicado.SeriesResumidas
                , comunicado.TipoCalendarioId
                , comunicado.EventoId
                ));
        }
    }
}
