import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import Semana from './Semana';
import DiaCompleto from './DiaCompleto';

const Div = styled.div``;

const DiaDaSemana = props => {
  const { nomeDia } = props;

  return (
    <Div className="col">
      <Div className="text-muted text-center small">{nomeDia}</Div>
    </Div>
  );
};

DiaDaSemana.propTypes = {
  nomeDia: PropTypes.string,
};

DiaDaSemana.defaultProps = {
  nomeDia: '',
};

const MesCompleto = props => {
  const { meses, filtros } = props;

  const mesesLista = meses.split(',');
  const mesesCalendario = useSelector(state => state.calendarioEscolar.meses);

  const [mesSelecionado, setMesSelecionado] = useState(-1);
  const [ultimoUsado, setUltimoUsado] = useState(-1);

  const [diasDaSemana, setDiasDaSemana] = useState([]);
  const [estaAberto, setEstaAberto] = useState(false);

  useEffect(() => {
    mesesLista.forEach(mes => {
      if (mesesCalendario[mes].estaAberto) {
        setMesSelecionado(parseInt(mes, 10));
      }
    });
  }, [mesesCalendario]);

  useEffect(() => {
    if (mesSelecionado > 0) {
      const dataAtual = new Date();
      const data = new Date(dataAtual.getFullYear(), mesSelecionado - 1, 1);
      data.setDate(data.getDate() - data.getDay() - 1);

      const diasDaSemanaLista = [];
      for (let numSemanas = 0; numSemanas < 6; numSemanas += 1) {
        diasDaSemanaLista[numSemanas] = [];
        for (let numDias = 0; numDias < 7; numDias += 1) {
          diasDaSemanaLista[numSemanas].push(
            new Date(data.setDate(data.getDate() + 1))
          );
        }
      }

      setDiasDaSemana(diasDaSemanaLista);
      setUltimoUsado(mesSelecionado);
      setEstaAberto(true);
    }
  }, [mesSelecionado]);

  return (
    mesSelecionado > 0 &&
    estaAberto && (
      <Div className="border border-top-0 w-100">
        <Div className="w-100 d-flex py-3">
          <DiaDaSemana nomeDia="Domingo" />
          <DiaDaSemana nomeDia="Segunda" />
          <DiaDaSemana nomeDia="Terça" />
          <DiaDaSemana nomeDia="Quarta" />
          <DiaDaSemana nomeDia="Quinta" />
          <DiaDaSemana nomeDia="Sexta" />
          <DiaDaSemana nomeDia="Sábado" />
        </Div>
        <Semana
          inicial
          dias={diasDaSemana[0]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[0]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[1]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[1]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[2]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[2]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[3]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[3]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          dias={diasDaSemana[4]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[4]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <Semana
          className="pb-4"
          dias={diasDaSemana[5]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
        <DiaCompleto
          dias={diasDaSemana[5]}
          mesAtual={ultimoUsado}
          filtros={filtros}
        />
      </Div>
    )
  );
};

MesCompleto.propTypes = {
  meses: PropTypes.string,
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

MesCompleto.defaultProps = {
  meses: '',
  filtros: {},
};

export default MesCompleto;
