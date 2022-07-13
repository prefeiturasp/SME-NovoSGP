using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarMediaAlunosUseCase : AbstractUseCase, IValidarMediaAlunosUseCase
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        public ValidarMediaAlunosUseCase(IMediator mediator, IRepositorioAtividadeAvaliativa repositorio) : base(mediator)
        {
            this.repositorioAtividadeAvaliativa = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroValidarMediaAlunosDto>();

            var dataAtual = DateTime.Now;
            var notasConceitos = await mediator.Send(new ObterNotasPorAlunosAtividadesAvaliativasQuery(filtro.AtividadesAvaliativasIds.ToArray(), filtro.AlunosIds.ToArray(), filtro.DisciplinaId, filtro.CodigoTurma));

            var atividadesAvaliativas = repositorioAtividadeAvaliativa.ListarPorIds(filtro.AtividadesAvaliativasIds);

            var notasPorAvaliacoes = notasConceitos.GroupBy(x => x.AtividadeAvaliativaID);
            var percentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            foreach (var notasPorAvaliacao in notasPorAvaliacoes)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunosAtividadeAvaliativa,
                                                               ObterFiltroAtividadeAvaliativa(filtro, atividadesAvaliativas, percentualAlunosInsuficientes, notasPorAvaliacao, filtro),
                                                               Guid.NewGuid(), null));

            return true;
        }

        private FiltroValidarMediaAlunosAtividadeAvaliativaDto ObterFiltroAtividadeAvaliativa(FiltroValidarMediaAlunosDto validarMediaAlunosDto, System.Collections.Generic.IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, double percentualAlunosInsuficientes, IGrouping<long, NotaConceito> notasPorAvaliacao, FiltroValidarMediaAlunosDto filtroAtividadeAvaliativa)
        {
            return new FiltroValidarMediaAlunosAtividadeAvaliativaDto(atividadesAvaliativas, percentualAlunosInsuficientes, notasPorAvaliacao, validarMediaAlunosDto.Usuario, validarMediaAlunosDto.DisciplinaId, filtroAtividadeAvaliativa);
        }
    }
}
