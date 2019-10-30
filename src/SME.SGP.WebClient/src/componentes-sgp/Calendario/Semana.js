import React, { useState } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import { Base } from '~/componentes/colors';

const Div = styled.div``;

const WeekDay = props => {
  const [estaAberto, setEstaAberto] = useState(false);
  // const [diaSelecionado, setDiaSelecionado] = useState();
  const [ultimoDiaSelecionado, setUltimoDiaSelecionado] = useState();

  const selectDayClick = () => {
    setEstaAberto(!estaAberto);
  };

  let {
    className,
    day,
    mesSelecionado,
    selectDay,
    dSelecionado,
    toggleMonth,
    ...rest
  } = props;

  let style = {};

  if (day === dSelecionado) setUltimoDiaSelecionado(dSelecionado);

  style.height = 62;
  style.cursor = 'pointer';

  if (day.getDay() === 0) style.backgroundColor = Base.RosaCalendario;
  else if (day.getDay() === 6) style.backgroundColor = Base.CinzaCalendario;

  if (className !== undefined)
    className += ' col border border-left-0 border-bottom-0';
  else className = 'col border border-left-0 border-bottom-0';

  if (!estaAberto)
    className = className.replace(' border-bottom-0', ' border-bottom');

  let formatedDay = day.getDate();

  if (formatedDay < 10) formatedDay = `0${formatedDay}`;

  const dayStyle = {
    position: 'relative',
    top: 30,
    cursor: 'pointer',
  };

  if (mesSelecionado !== day.getMonth())
    dayStyle.color = 'rgba(66, 71, 74, 0.3)';

  return (
    <div className={className} style={style} {...rest} onClick={selectDayClick}>
      <div className="w-100 h-100 d-flex">
        <div style={dayStyle}>{formatedDay}</div>
      </div>
    </div>
  );
};

const Semana = props => {
  console.log(props);

  const { inicial, dias, mesAtual } = props;

  const diaSelecionado = useSelector(
    state => state.calendarioEscolar.diaSelecionado
  );

  const childProps = {
    diaSelecionado,
    mesAtual,
  };

  return (
    <Div>
      {inicial ? (
        <Div className="w-100 d-flex">
          <WeekDay day={dias[0]} {...childProps} />
          <WeekDay day={dias[1]} {...childProps} />
          <WeekDay day={dias[2]} {...childProps} />
          <WeekDay day={dias[3]} {...childProps} />
          <WeekDay day={dias[4]} {...childProps} />
          <WeekDay day={dias[5]} {...childProps} />
          <WeekDay className="border-right-0" day={dias[6]} {...childProps} />
        </Div>
      ) : (
        <Div className="w-100 d-flex">
          <WeekDay className="border-top-0" day={dias[0]} {...childProps} />
          <WeekDay className="border-top-0" day={dias[1]} {...childProps} />
          <WeekDay className="border-top-0" day={dias[2]} {...childProps} />
          <WeekDay className="border-top-0" day={dias[3]} {...childProps} />
          <WeekDay className="border-top-0" day={dias[4]} {...childProps} />
          <WeekDay className="border-top-0" day={dias[5]} {...childProps} />
          <WeekDay
            className="border-top-0 border-right-0"
            day={dias[6]}
            {...childProps}
          />
        </Div>
      )}
    </Div>
  );
};

Semana.propTypes = {
  inicial: PropTypes.bool,
  dias: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  mesAtual: PropTypes.number,
};

Semana.defaultProps = {
  inicial: false,
  dias: [],
  mesAtual: 0,
};

export default Semana;
