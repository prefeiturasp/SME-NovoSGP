using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosGrupoComunicacao : IComandosGrupoComunicacao
    {
        private readonly IRepositorioGrupoComunicacao repositorio;

        public ComandosGrupoComunicacao(IRepositorioGrupoComunicacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task Alterar(GrupoComunicacaoDto dto, long id)
        {
            throw new NotImplementedException();
        }

        public Task Excluir(long id)
        {
            throw new NotImplementedException();
        }

        public Task Inserir(GrupoComunicacaoDto dto)
        {
            throw new NotImplementedException();
        }
    }
}