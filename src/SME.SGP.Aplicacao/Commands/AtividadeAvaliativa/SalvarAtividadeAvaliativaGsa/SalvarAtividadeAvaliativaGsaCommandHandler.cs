using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtividadeAvaliativaGsaCommandHandler : AsyncRequestHandler<SalvarAtividadeAvaliativaGsaCommand>
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IMediator mediator;

        public SalvarAtividadeAvaliativaGsaCommandHandler(IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa, IMediator mediator)
        {
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(SalvarAtividadeAvaliativaGsaCommand request, CancellationToken cancellationToken)
        {
            var atividadeAvaliativa = await repositorioAtividadeAvaliativa.ObterPorAtividadeClassroomId(request.AtividadeClassroomId);

            if (atividadeAvaliativa != null)
                await AlterarAtividade(atividadeAvaliativa, request);
            else
                await InserirAtividade(request);
        }

        private async Task InserirAtividade(SalvarAtividadeAvaliativaGsaCommand request)
        {
            var ano = request.DataCriacao.Year;

            var parametroTipoAtividade = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.TipoAtividadeAvaliativaClassroom, ano));
            if (parametroTipoAtividade != null)
            {
                int.TryParse(parametroTipoAtividade.Valor, out int codigoTipoAtividade);
                var tipoAvaliacaoId = await mediator.Send(new ObterTipoAvaliacaoPorCodigoQuery(codigoTipoAtividade));
                var turma = await mediator.Send(new ObterCodigosDreUePorTurmaQuery(request.TurmaCodigo));

                var atividadeAvaliativa = MapearEntidade(request, tipoAvaliacaoId, turma);
                await SalvarAtividade(atividadeAvaliativa, request.ComponenteCurricularId);
            }
        }

        private async Task SalvarAtividade(AtividadeAvaliativa atividadeAvaliativa, long componenteCurricularId)
        {
            var atividadeId = await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
            await mediator.Send(new SalvarAtividadeAvaliativaDisciplinaCommand(atividadeId, componenteCurricularId.ToString()));
        }

        private AtividadeAvaliativa MapearEntidade(SalvarAtividadeAvaliativaGsaCommand request, long tipoAvaliacaoId, DreUeDaTurmaDto turma)
        {
            return new AtividadeAvaliativa()
            {
                TurmaId = request.TurmaCodigo,
                UeId = turma.UeCodigo,
                DreId = turma.DreCodigo,
                AtividadeClassroomId = request.AtividadeClassroomId,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                DataAvaliacao = request.DataCriacao,
                NomeAvaliacao = request.Titulo,
                DescricaoAvaliacao = request.Descricao,
                ProfessorRf = request.UsuarioRf,
                TipoAvaliacaoId = tipoAvaliacaoId
            };
        }

        private async Task AlterarAtividade(AtividadeAvaliativa atividadeAvaliativa, SalvarAtividadeAvaliativaGsaCommand request)
        {
            atividadeAvaliativa.NomeAvaliacao = request.Titulo;
            atividadeAvaliativa.DescricaoAvaliacao = request.Descricao;
            atividadeAvaliativa.DataAvaliacao = request.DataCriacao;

            await repositorioAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }
    }
}
