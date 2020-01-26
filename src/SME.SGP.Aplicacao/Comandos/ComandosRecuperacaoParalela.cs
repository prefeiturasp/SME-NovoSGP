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

        async Task<IEnumerable<RecuperacaoParalelaListagemDto>> IComandosRecuperacaoParalela.Salvar(RecuperacaoParalelaDto recuperacaoParalelaDto)
        {
            var list = new List<RecuperacaoParalelaListagemDto>();
            foreach (var item in recuperacaoParalelaDto.Periodo.Alunos)
            {
                var entidade = new RecuperacaoParalela
                {
                    Id = item.Id,
                    TurmaId = item.TurmaId,
                    Aluno_id = item.TurmaId
                };

                await repositorioRecuperacaoParalela.SalvarAsync(entidade);
            }
            return list;
        }
    }
}