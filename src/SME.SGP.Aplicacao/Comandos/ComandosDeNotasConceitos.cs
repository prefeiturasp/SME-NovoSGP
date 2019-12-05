using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosDeNotasConceitos
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IServicoDeNotasConceitos servicosDeNotasConceitos;

        public ComandosDeNotasConceitos(IServicoDeNotasConceitos servicosDeNotasConceitos, IRepositorioNotasConceitos repositorioNotasConceitos, IServicoUsuario servicoUsuario)
        {
            this.servicosDeNotasConceitos = servicosDeNotasConceitos ?? throw new System.ArgumentNullException(nameof(servicosDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new System.ArgumentNullException(nameof(repositorioNotasConceitos));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task Salvar(NotaConceitoLista notaConceitoLista)
        {
            var notasConceitosDto = notaConceitoLista.NotasConceitos;

            var alunos = notasConceitosDto.Select(x => x.AlunoId);
            var avaliacoes = notasConceitosDto.Select(x => x.AtividadeAvaliativaID);

            var notasBanco = repositorioNotasConceitos.ObterNotasPorAlunosAtividadesAvaliativas(avaliacoes, alunos);

            var professorRf = servicoUsuario.ObterRf();

            var notasSalvar = new List<NotaConceito>();

            if (notasBanco == null || !notasBanco.Any())
                await IncluirTodasNotas(notasConceitosDto, notasSalvar, professorRf, notaConceitoLista.TurmaId);
            else
                await TratarInclusaoEdicaoNotas(notasConceitosDto, notasBanco, notasSalvar, professorRf, notaConceitoLista.TurmaId);
        }

        private async Task TratarInclusaoEdicaoNotas(IEnumerable<NotaConceitoDto> notasConceitosDto, IEnumerable<NotaConceito> notasBanco, List<NotaConceito> notasSalvar, string professorRf, string turmaId)
        {
            var notasEdicao = notasConceitosDto.Where(dto => notasBanco.Any(banco => banco.AlunoId == dto.AlunoId && banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaID))
                .Select(dto => ObterEntidadeEdicao(dto, notasBanco.FirstOrDefault(banco => banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaID && banco.AlunoId == dto.AlunoId)));

            var notasInclusao = notasConceitosDto.Where(dto => !notasBanco.Any(banco => banco.AlunoId == dto.AlunoId && banco.AtividadeAvaliativaID == dto.AtividadeAvaliativaID)).Select(dto => ObterEntidadeInclusao(dto));

            notasSalvar.AddRange(notasEdicao);
            notasSalvar.AddRange(notasInclusao);

            await servicosDeNotasConceitos.Salvar(notasSalvar, professorRf, turmaId);
        }

        private async Task IncluirTodasNotas(IEnumerable<NotaConceitoDto> notasConceitosDto, List<NotaConceito> notasSalvar, string professorRf, string turmaId)
        {
            notasSalvar = notasConceitosDto.Select(x => ObterEntidadeInclusao(x)).ToList();
            await servicosDeNotasConceitos.Salvar(notasSalvar, professorRf, turmaId);
        }

        private NotaConceito ObterEntidadeEdicao(NotaConceitoDto dto, NotaConceito entidade)
        {
            entidade.Nota = dto.Nota;

            return entidade;
        }

        private NotaConceito ObterEntidadeInclusao(NotaConceitoDto Dto)
        {
            return new NotaConceito
            {
                AtividadeAvaliativaID = Dto.AtividadeAvaliativaID,
                AlunoId = Dto.AlunoId,
                Nota = Dto.Nota,
                Conceito = Dto.Conceito,
            };
        }
    }
}