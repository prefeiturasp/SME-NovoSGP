using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterEventosEscolaAquiPorDreUeTurmaMesQuery : IRequest<IEnumerable<EventoEADto>>
    {
        public ObterEventosEscolaAquiPorDreUeTurmaMesQuery(string dre_id, string ue_id, string turma_id, int modalidadeCalendario, DateTime mesAno)
        {
            Dre_id = dre_id;
            Ue_id = ue_id;
            Turma_id = turma_id;
            ModalidadeCalendario = modalidadeCalendario;
            MesAno = mesAno;    
        }

        public string Dre_id { get; set; }
        public string Ue_id { get; set; }
        public string Turma_id { get; set; }
        public int ModalidadeCalendario { get; set; }
        public DateTime MesAno { get; set; }


    }

    public class ObterEventosPorDreUeTurmaMesQueryValidator : AbstractValidator<ObterEventosEscolaAquiPorDreUeTurmaMesQuery>
    {
        public ObterEventosPorDreUeTurmaMesQueryValidator()
        {
            RuleFor(x => x.ModalidadeCalendario)
                .NotEmpty()
                .WithMessage("A Modalidade Calendário é obrigatória para busca de eventos.");

            RuleFor(x => x.MesAno)
                .NotEmpty()
                .WithMessage("O Mês/Ano é obrigatório para busca de eventos.");
        }
    }
}