import React, { useState } from 'react';
import { useSelector } from 'react-redux';

const WeekDay = props => {
  const [estaAberto, setEstaAberto] = useState(false);
  const [diaSelecionado, setDiaSelecionado] = useState();
  const [ultimoDiaSelecionado, setUltimoDiaSelecionado] = useState();

  const selectDayClick = () => {
    // this.props.selectDay(this.props.day);
    // ultimoDiaSelecionado = sd;
    setEstaAberto(!estaAberto);
  };

  let {
    className,
    day,
    currentMonth,
    selectDay,
    selectedDay,
    toggleMonth,
    ...rest
  } = props;

  let style = {};

  if (day === selectedDay) setUltimoDiaSelecionado(selectedDay);

  // if (
  //   ultimoDiaSelecionado !== selectedDay ||
  //   selectedDay === undefined ||
  //   ultimoDiaSelecionado === undefined ||
  //   ultimoDiaSelecionado.getMonth() !== currentMonth
  // )
  //   setEstaAberto(false);
  // else setEstaAberto(true);

  // if (style === undefined) {
  style.height = 61;
  style.cursor = 'pointer';
  // }

  // console.log(day);

  if (day.getDay() === 0) style.backgroundColor = '#FEE4E2';
  else if (day.getDay() === 6) style.backgroundColor = '#F7F9FA';

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

  if (currentMonth !== day.getMonth()) dayStyle.color = 'rgba(66, 71, 74, 0.3)';

  return (
    <div className={className} style={style} {...rest} onClick={selectDayClick}>
      <div className="w-100 h-100 d-flex">
        <div style={dayStyle}>{formatedDay}</div>
      </div>
    </div>
  );
};

const Semana = props => {
  const {
    calendar,
    firstWeek,
    days,
    currentMonth,
    selectDay,
    toggleMonth,
    ...rest
  } = props;

  // console.log(props);

  const diaSelecionado = useSelector(
    state => state.calendarioEscolar.diaSelecionado
  );

  const childProps = {
    selectDay,
    selectedDay: diaSelecionado,
    currentMonth,
  };

  return (
    <div {...rest}>
      {firstWeek ? (
        <div className="w-100 d-flex">
          <WeekDay day={days[0]} {...childProps} />
          <WeekDay day={days[1]} {...childProps} />
          <WeekDay day={days[2]} {...childProps} />
          <WeekDay day={days[3]} {...childProps} />
          <WeekDay day={days[4]} {...childProps} />
          <WeekDay day={days[5]} {...childProps} />
          <WeekDay className="border-right-0" day={days[6]} {...childProps} />
        </div>
      ) : (
        <div className="w-100 d-flex">
          <WeekDay className="border-top-0" day={days[0]} {...childProps} />
          <WeekDay className="border-top-0" day={days[1]} {...childProps} />
          <WeekDay className="border-top-0" day={days[2]} {...childProps} />
          <WeekDay className="border-top-0" day={days[3]} {...childProps} />
          <WeekDay className="border-top-0" day={days[4]} {...childProps} />
          <WeekDay className="border-top-0" day={days[5]} {...childProps} />
          <WeekDay
            className="border-top-0 border-right-0"
            day={days[6]}
            {...childProps}
          />
        </div>
      )}
    </div>
  );
};

export default Semana;
