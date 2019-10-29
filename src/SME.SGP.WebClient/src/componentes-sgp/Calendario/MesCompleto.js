import React, { useEffect, useState } from 'react';
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

  const mesesParam = meses.split(',');
  const mesesCalendario = useSelector(state => state.calendarioEscolar.meses);

  let mes = -1;
  let ultimoUsado = -1;

  const [weeks, setWeeks] = useState([]);
  const [estaAberto, setEstaAberto] = useState(false);

  useEffect(() => {
    mesesParam.forEach(mesParam => {
      if (mesesCalendario[mesParam].estaAberto) {
        setEstaAberto(true);
        mes = parseInt(mesParam, 10);
      }
    });

    let diasSemana = [];

    if (mes >= 0 && diasSemana[mes] === undefined) {
      const dataAtual = new Date();
      const data = new Date(dataAtual.getFullYear(), mes - 1, 1);
      data.setDate(data.getDate() - data.getDay() - 1);

      let diasDaSemana = [];
      for (var numSemanas = 0; numSemanas < 6; numSemanas += 1) {
        diasDaSemana[numSemanas] = [];
        for (var numDias = 0; numDias < 7; numDias += 1) {
          diasDaSemana[numSemanas].push(
            new Date(data.setDate(data.getDate() + 1))
          );
        }
        // diasSemana.push(diasDaSemana);
      }

      // console.log(diasDaSemana);

      // console.log(diasSemana);

      setWeeks(diasDaSemana);
    }

    //   diasSemana[mes] = weeks;

    // for (let numSemanas = 0; numSemanas < 6; numSemanas += 1) {
    //   const diasLista = [];
    //   for (let numDias = 0; numDias < 7; numDias += 1) {
    //     diasLista.push(new Date(data.setDate(data.getDate() + 1)));
    //   }
    //   setDias(diasLista);
    // }
    // }

    // if (mes === -1) setWeeks(diasSemana[ultimoUsado]);
    // else {
    //   // setWeeks(diasSemana[mes]);
    //   ultimoUsado = mes;
    // }

    // if (mes === -1) setDias(diasSemana[ultimoUsado]);
    // else {
    //   setDias(diasSemana[mes]);
    //   ultimoUsado = mes;
    // }
  }, [mesesCalendario]);

  useEffect(() => {
    // console.log(weeks);
  }, [weeks]);

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
      {toggle =>
        toggle &&
        (props => (
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
            {/* {console.log(weeks)} */}
            <Semana firstWeek days={weeks[0]} currentMonth={ultimoUsado} />
            <FullDay days={weeks[0]} />
            <Semana days={weeks[1]} currentMonth={ultimoUsado} />
            <FullDay days={weeks[1]} />
            <Semana days={weeks[2]} currentMonth={ultimoUsado} />
            <FullDay days={weeks[2]} />
            <Semana days={weeks[3]} currentMonth={ultimoUsado} />
            <FullDay days={weeks[3]} />
            <Semana days={weeks[4]} currentMonth={ultimoUsado} />
            <FullDay days={weeks[4]} />
            <Semana
              className="pb-4"
              days={weeks[5]}
              currentMonth={ultimoUsado}
            />
            <FullDay days={weeks[5]} />
          </animated.div>
        ))
      }
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
