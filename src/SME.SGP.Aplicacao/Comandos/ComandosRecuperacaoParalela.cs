using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosRecuperacaoParalela : IComandosRecuperacaoParalela
    {
        private readonly IConsultaRecuperacaoParalela consultaRecuperacaoParalela;
        private readonly IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela;
        private readonly IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta repositorioRecuperacaoParalelaPeriodoObjetivoResposta;
        private readonly IUnitOfWork unitOfWork;

        public ComandosRecuperacaoParalela(IRepositorioRecuperacaoParalela repositorioRecuperacaoParalela,
            IRepositorioRecuperacaoParalelaPeriodoObjetivoResposta repositorioRecuperacaoParalelaPeriodoObjetivo,
            IConsultaRecuperacaoParalela consultaRecuperacaoParalela,
            IUnitOfWork unitOfWork)
        {
            this.repositorioRecuperacaoParalela = repositorioRecuperacaoParalela ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalela));
            this.repositorioRecuperacaoParalelaPeriodoObjetivoResposta = repositorioRecuperacaoParalelaPeriodoObjetivo ?? throw new ArgumentNullException(nameof(repositorioRecuperacaoParalelaPeriodoObjetivo));
            this.consultaRecuperacaoParalela = consultaRecuperacaoParalela ?? throw new ArgumentNullException(nameof(consultaRecuperacaoParalela));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RecuperacaoParalelaListagemDto> Salvar(RecuperacaoParalelaDto recuperacaoParalelaDto)
        {
            var list = new List<RecuperacaoParalelaListagemDto>();
            unitOfWork.IniciarTransacao();
            foreach (var item in recuperacaoParalelaDto.Periodo.Alunos)
            {
                var recuperacaoParalela = new RecuperacaoParalela
                {
                    Id = item.Id,
                    TurmaId = item.TurmaId,
                    TurmaRecuperacaoParalelaId = item.TurmaRecuperacaoParalelaId,
                    Aluno_id = item.CodAluno,
                    CriadoEm = recuperacaoParalelaDto.Periodo.CriadoEm ?? default,
                    CriadoRF = recuperacaoParalelaDto.Periodo.CriadoRF ?? null,
                    CriadoPor = recuperacaoParalelaDto.Periodo.CriadoPor ?? null
                };

                await repositorioRecuperacaoParalela.SalvarAsync(recuperacaoParalela);
                await repositorioRecuperacaoParalelaPeriodoObjetivoResposta.Excluir(item.Id, recuperacaoParalelaDto.Periodo.Id);
                foreach (var resposta in recuperacaoParalelaDto.Periodo.Alunos.Where(w => w.CodAluno == item.CodAluno).FirstOrDefault().Respostas)
                {
                    await repositorioRecuperacaoParalelaPeriodoObjetivoResposta.SalvarAsync(new RecuperacaoParalelaPeriodoObjetivoResposta
                    {
                        ObjetivoId = resposta.ObjetivoId,
                        PeriodoRecuperacaoParalelaId = recuperacaoParalelaDto.Periodo.Id,
                        RecuperacaoParalelaId = recuperacaoParalela.Id,
                        RespostaId = resposta.RespostaId
                    });
                }
            }
            unitOfWork.PersistirTransacao();
            return await consultaRecuperacaoParalela.Listar(new Infra.FiltroRecuperacaoParalelaDto
            {
                Ordenacao = recuperacaoParalelaDto.Ordenacao,
                PeriodoId = recuperacaoParalelaDto.Periodo.Id,
                TurmaId = recuperacaoParalelaDto.Periodo.Alunos.FirstOrDefault().TurmaRecuperacaoParalelaId
            });
        }
    }
}