using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotasConceitos : IComandosNotasConceitos
    {
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IServicoDeNotasConceitos servicosDeNotasConceitos;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ComandosNotasConceitos(IServicoDeNotasConceitos servicosDeNotasConceitos, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa, IRepositorioNotasConceitosConsulta repositorioNotasConceitos, IServicoUsuario servicoUsuario,
            IMediator mediator)
        {
            this.servicosDeNotasConceitos = servicosDeNotasConceitos ?? throw new System.ArgumentNullException(nameof(servicosDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new System.ArgumentNullException(nameof(repositorioNotasConceitos));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
            
        }

        public async Task Salvar(NotaConceitoListaDto notaConceitoLista)
        {
            var notasConceitosDto = notaConceitoLista.NotasConceitos;

            var alunos = notasConceitosDto.Select(x => x.AlunoId).ToList();

            var avaliacoes = notasConceitosDto.Select(x => x.AtividadeAvaliativaId).ToList();

            var notasBanco = repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativas(avaliacoes, alunos, notaConceitoLista.DisciplinaId);

            var professorRf = servicoUsuario.ObterRf();

            if (notasBanco == null || !notasBanco.Any())
                await IncluirTodasNotas(notasConceitosDto, professorRf, notaConceitoLista.TurmaId, notaConceitoLista.DisciplinaId);
            else
                await TratarInclusaoEdicaoNotas(notasConceitosDto, notasBanco, professorRf, notaConceitoLista.TurmaId, notaConceitoLista.DisciplinaId);


            var atividadeAvaliativa = await repositorioAtividadeAvaliativa.ObterPorIdAsync(notasConceitosDto.Select(x => x.AtividadeAvaliativaId).FirstOrDefault());
            var aula = await mediator.Send(new ObterAulaPorComponenteCurricularIdTurmaIdEDataQuery(notaConceitoLista.DisciplinaId, notaConceitoLista.TurmaId, atividadeAvaliativa.DataAvaliacao));

            if(aula != null)
                await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Avaliacao));
        }

        private async Task IncluirTodasNotas(IEnumerable<NotaConceitoDto> notasConceitosDto, string professorRf, string turmaId, string disiplinaId)
        {
            var notasSalvar = notasConceitosDto.Select(x => ObterEntidadeInclusao(x)).ToList();
            await servicosDeNotasConceitos.Salvar(notasSalvar, professorRf, turmaId, disiplinaId);
        }

        private NotaConceito ObterEntidadeEdicao(NotaConceitoDto dto, NotaConceito entidade)
        {
            entidade.Nota = dto.Nota;
            entidade.ConceitoId = dto.Conceito;

            return entidade;
        }

        private NotaConceito ObterEntidadeInclusao(NotaConceitoDto Dto)
        {
            return new NotaConceito
            {
                AtividadeAvaliativaID = Dto.AtividadeAvaliativaId,
                AlunoId = Dto.AlunoId,
                Nota = Dto.Nota,
                ConceitoId = Dto.Conceito,
            };
        }

        private async Task TratarInclusaoEdicaoNotas(IEnumerable<NotaConceitoDto> notasConceitosDto, IEnumerable<NotaConceito> notasBanco, string professorRf, string turmaId, string disciplinaId)
        {
            var notasEdicao = notasConceitosDto.Where(dto => notasBanco.Any(banco => banco.AlunoId == dto.AlunoId && banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaId))
                .Select(dto => ObterEntidadeEdicao(dto, notasBanco.FirstOrDefault(banco => banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaId && banco.AlunoId == dto.AlunoId)));

            var notasInclusao = notasConceitosDto.Where(dto => !notasBanco.Any(banco => banco.AlunoId == dto.AlunoId && banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaId)).Select(dto => ObterEntidadeInclusao(dto));

            var notasSalvar = new List<NotaConceito>();

            notasSalvar.AddRange(notasEdicao);
            notasSalvar.AddRange(notasInclusao);

            await servicosDeNotasConceitos.Salvar(notasSalvar, professorRf, turmaId, disciplinaId);
        }
    }
}