using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioParecerConclusivoUseCase : IRelatorioParecerConclusivoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioParecerConclusivoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto)
        {
            if (filtroRelatorioParecerConclusivoDto.Modalidade.HasValue && filtroRelatorioParecerConclusivoDto.Modalidade.Value == Modalidade.EducacaoInfantil)
                throw new NegocioException("Não é possível gerar este relatório para a modalidade infantil!");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroRelatorioParecerConclusivoDto.UsuarioNome = usuarioLogado.Nome;            

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ParecerConclusivo, filtroRelatorioParecerConclusivoDto, usuarioLogado, formato: filtroRelatorioParecerConclusivoDto.TipoFormatoRelatorio));
        }
    }
}
