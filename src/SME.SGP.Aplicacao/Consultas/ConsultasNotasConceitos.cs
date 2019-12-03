using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotasConceitos
    {
        private readonly IServicoEOL servicoEOL;
        private readonly IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;

        public ConsultasNotasConceitos(IServicoEOL servicoEOL, IConsultaAtividadeAvaliativa consultasAtividadeAvaliativa, 
            IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTipoCalendario repositorioTipoCalendario)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.consultasAtividadeAvaliativa = consultasAtividadeAvaliativa ?? throw new ArgumentNullException(nameof(consultasAtividadeAvaliativa));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
        }

        public async void ObterBimestre(string turmaId, int bimestre, int anoLetivo, string disciplinaId, ModalidadeTipoCalendario modalidade)
        {
            var atividadesAvaliativa = consultasAtividadeAvaliativa.ObterAvaliacoesDoBimestre(turmaId, anoLetivo, bimestre, modalidade);

            var alunos = await servicoEOL.ObterAlunosPorTurma(turmaId);

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi encontrado alunos para a turma informada");


        }

        
    }
}
