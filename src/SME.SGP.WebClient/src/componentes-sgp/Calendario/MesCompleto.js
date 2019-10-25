import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Transition } from 'react-spring/renderprops';
import { animated } from 'react-spring';
import Semana from './Semana';
import FullDay from './DiaCompleto';

const Div = styled.div``;

const DiaDaSemana = props => {
  const { nomeDia } = props;

  return (
    <Div className="col">
      <Div className="text-small text-muted font-weight-light text-center">
        {nomeDia}
      </Div>
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
  const { meses } = props;

  const mesesCalendario = useSelector(state => state.calendarioEscolar.meses);
  const diasSemana = {};
  let ultimoUsado = -1;

  // console.log(mesesCalendario);

  const mesesCompleto = meses.split(',');

  // console.log(mesesCompleto);

  let mes = -1;
  let dias = [];
  let estaAberto = false;

  for (let i = 0; i < mesesCompleto.length; i += 1) {
    console.log(
      mesesCalendario[mesesCompleto[i]],
      mesesCalendario[mesesCompleto[i]].estaAberto
    );

    if (mesesCalendario[mesesCompleto[i]].estaAberto) {
      estaAberto = true;
      mes += Number(mesesCompleto[i]);
    }
  }

  console.log(mes);

  if (mes >= 0 && diasSemana[mes] === undefined) {
    let currentDate = new Date();
    let date = new Date(currentDate.getFullYear(), mes, 1);
    date.setDate(date.getDate() - date.getDay() - 1);

    for (let j = 0; j < 6; j++) {
      let week = [];

      for (let k = 0; k < 7; k++)
        week.push(new Date(date.setDate(date.getDate() + 1)));

      dias.push(week);
    }

    diasSemana[mes] = dias;
  }

  if (mes === -1) dias = diasSemana[ultimoUsado];
  else {
    dias = diasSemana[mes];
    ultimoUsado = mes;
  }

  return (
    <Transition
      items={estaAberto}
      from={{
        display: 'none',
        height: 0,
        overflow: 'hidden',
      }}
      enter={{
        display: 'block',
        height: 'auto',
        overflow: 'hidden',
      }}
      leave={{
        height: 0,
        overflow: 'hidden',
      }}
    >
      {// toggle =>
      //   toggle &&
      props => (
        <animated.div
          className="completo border border-top-0 w-100"
          style={props}
        >
          <Div className="w-100 d-flex pt-4">
            <DiaDaSemana nomeDia="Domingo" />
            <DiaDaSemana nomeDia="Segunda" />
            <DiaDaSemana nomeDia="Terça" />
            <DiaDaSemana nomeDia="Quarta" />
            <DiaDaSemana nomeDia="Quinta" />
            <DiaDaSemana nomeDia="Sexta" />
            <DiaDaSemana nomeDia="Sábado" />
          </Div>
          <Semana firstWeek days={dias[0]} currentMonth={ultimoUsado} />
          <FullDay days={dias[0]} />
          <Semana days={dias[1]} currentMonth={ultimoUsado} />
          <FullDay days={dias[1]} />
          <Semana days={dias[2]} currentMonth={ultimoUsado} />
          <FullDay days={dias[2]} />
          <Semana days={dias[3]} currentMonth={ultimoUsado} />
          <FullDay days={dias[3]} />
          <Semana days={dias[4]} currentMonth={ultimoUsado} />
          <FullDay days={dias[4]} />
          <Semana className="pb-4" days={dias[5]} currentMonth={ultimoUsado} />
          <FullDay days={dias[5]} />
        </animated.div>
      )}
    </Transition>
  );
};

MesCompleto.propTypes = {
  meses: PropTypes.string,
};

MesCompleto.defaultProps = {
  meses: '',
};

export default MesCompleto;
