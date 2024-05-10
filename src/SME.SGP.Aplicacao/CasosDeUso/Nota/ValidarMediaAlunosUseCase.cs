using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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

            var notasConceitos = await mediator.Send(new ObterNotasPorAlunosAtividadesAvaliativasQuery(filtro.AtividadesAvaliativasIds.ToArray(), filtro.AlunosIds.ToArray(), filtro.DisciplinaId, filtro.CodigoTurma));

            var atividadesAvaliativas = await repositorioAtividadeAvaliativa.ListarPorIds(filtro.AtividadesAvaliativasIds);

            var notasPorAvaliacoes = notasConceitos.GroupBy(x => x.AtividadeAvaliativaID);
            var percentualAlunosInsuficientes = double.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.PercentualAlunosInsuficientes, DateTime.Today.Year)));

            foreach (var notasPorAvaliacao in notasPorAvaliacoes)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAvaliacao.RotaValidarMediaAlunosAtividadeAvaliativa,
                                                               ObterFiltroAtividadeAvaliativa(filtro, atividadesAvaliativas, percentualAlunosInsuficientes, notasPorAvaliacao.Key, notasPorAvaliacao.ToList(), filtro.ConsideraHistorico),
                                                               Guid.NewGuid(), null));

            return true;
        }

        private FiltroValidarMediaAlunosAtividadeAvaliativaDto ObterFiltroAtividadeAvaliativa(FiltroValidarMediaAlunosDto filtroValidarMedia, System.Collections.Generic.IEnumerable<AtividadeAvaliativa> atividadesAvaliativas, double percentualAlunosInsuficientes, long notaChaveAvaliativa, IEnumerable<NotaConceito> notasPorAvaliacao, bool consideraHistorico = false)
        {
            return new FiltroValidarMediaAlunosAtividadeAvaliativaDto(atividadesAvaliativas, percentualAlunosInsuficientes, notaChaveAvaliativa, notasPorAvaliacao, filtroValidarMedia.Usuario, filtroValidarMedia.DisciplinaId, filtroValidarMedia.HostAplicacao, filtroValidarMedia.TemAbrangenciaUeOuDreOuSme, consideraHistorico);
        }
    }
}
