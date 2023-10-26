﻿using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendarioPorAnoLetivoModalidadeQuery : IRequest<IEnumerable<TipoCalendario>>
    {
        public ObterTiposCalendarioPorAnoLetivoModalidadeQuery(int anoLetivo, string modalidadesStr, int semestre = 0)
        {
            AnoLetivo = anoLetivo;
            ModalidadesStr = modalidadesStr;

            ObterModalidadesDaLista(ModalidadesStr, out var modalidades);
            Modalidades = modalidades;
            Semestre = semestre;
        }

        public int AnoLetivo { get; }
        public int Semestre { get; }
        public string ModalidadesStr { get; }

        public Modalidade[] Modalidades { get; }

        public static bool ObterModalidadesDaLista(string listaStr, out Modalidade[] modalidades)
        {
            modalidades = null;
            if (listaStr.NaoEstaPreenchido())
                return false;

            var listaModalidadesStr = listaStr.Split(',');
            var listaModalidades = new List<Modalidade>(listaStr.Length);

            foreach (var modalidadeStr in listaModalidadesStr)
            {
                if (modalidadeStr.EstaPreenchido())
                {
                    try
                    {
                        var modalidade = (Modalidade)Enum.Parse(typeof(Modalidade), modalidadeStr);

                        if (Enum.IsDefined(typeof(Modalidade), modalidade))
                            listaModalidades.Add(modalidade);
                        else
                            return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            if (listaModalidades.Any())
            {
                modalidades = listaModalidades.ToArray();
                return true;
            }
            return false;
        }
    }

    public class ObterTiposCalendarioPorAnoLetivoModalidadeoQueryValidator : AbstractValidator<ObterTiposCalendarioPorAnoLetivoModalidadeQuery>
    {
        public ObterTiposCalendarioPorAnoLetivoModalidadeoQueryValidator()
        {
            RuleFor(x => x.ModalidadesStr).NotEmpty().WithMessage("As Modalidades são Obrigatórias");
            RuleFor(x => x).Must(ValidaListaModalidades).WithMessage(" Lista contém modalidade inválida");
        }

        private bool ValidaListaModalidades(ObterTiposCalendarioPorAnoLetivoModalidadeQuery query)
            => ObterTiposCalendarioPorAnoLetivoModalidadeQuery.ObterModalidadesDaLista(query.ModalidadesStr, out _);

    }
}