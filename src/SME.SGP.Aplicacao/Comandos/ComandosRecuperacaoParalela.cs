using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosRecuperacaoParalela : IComandosRecuperacaoParalela
    {
        private readonly IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela;
        private readonly IUnitOfWork unitOfWork;

        public ComandosRecuperacaoParalela(IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela, IUnitOfWork unitOfWork)
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        Task<IEnumerable<RecuperacaoParalelaDto>> IComandosRecuperacaoParalela.Salvar(IEnumerable<RecuperacaoParalelaDto> recuperacaoParalelaDto)
        {
            throw new NotImplementedException();
        }
    }
}