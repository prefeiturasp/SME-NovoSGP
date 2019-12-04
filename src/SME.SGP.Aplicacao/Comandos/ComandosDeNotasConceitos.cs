using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Comandos
{
    public class ComandosDeNotasConceitos
    {
        private readonly IRepositorioNotasConceitos repositorioNotasConceitos;
        private readonly IServicoDeNotasConceitos servicosDeNotasConceitos;

        public ComandosDeNotasConceitos(IServicoDeNotasConceitos servicosDeNotasConceitos, IRepositorioNotasConceitos repositorioNotasConceitos)
        {
            this.servicosDeNotasConceitos = servicosDeNotasConceitos ?? throw new System.ArgumentNullException(nameof(servicosDeNotasConceitos));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new System.ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public void Salvar(IEnumerable<NotaConceitoDto> notaConceitoDto)
        {
        }

        private NotaConceito ObterEntidadeEdicao(NotaConceitoDto Dto)
        {
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