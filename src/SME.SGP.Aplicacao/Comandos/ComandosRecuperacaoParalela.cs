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
            unitOfWork.IniciarTransacao();
            foreach (var item in recuperacaoParalelaDto.Periodo.Alunos)
            {
                var recuperacaoParalela = new RecuperacaoParalela
                {
                    Id = item.Id,
                    TurmaId = item.TurmaId,
                    Aluno_id = item.CodAluno
                };
                await repositorioRecuperacaoParalela.SalvarAsync(recuperacaoParalela);
            }
            unitOfWork.PersistirTransacao();
            return list;
        }
    }
}