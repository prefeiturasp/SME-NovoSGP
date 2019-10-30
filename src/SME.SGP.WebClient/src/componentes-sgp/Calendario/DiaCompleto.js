import React from 'react';
import { useSelector } from 'react-redux';
import { Transition } from 'react-spring/renderprops';
import { animated } from 'react-spring';
import { Base } from '~/componentes/colors';

const SemEvento = () => {
  return (
    <div
      className="d-flex w-100 h-100 justify-content-center d-flex align-items-center"
      style={{ fontSize: 25, color: Base.CinzaBotao }}
    >
      Sem eventos neste dia
    </div>
  );
};

const DiaCompleto = props => {
  const { days, calendar } = props;

  const diaSelecionado = useSelector(
    state => state.calendarioEscolar.diaSelecionado
  );

  let estaAberto = false;

  for (let i = 0; i < days.length; i += 1)
    if (days[i] === diaSelecionado) estaAberto = true;

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
        height: 212,
        overflow: 'hidden',
      }}
      leave={{
        display: 'none',
        height: 0,
        overflow: 'hidden',
      }}
    >
      {toggle =>
        toggle &&
        (props => (
          <animated.div className="border-bottom" style={props}>
            <SemEvento />
          </animated.div>
        ))
      }
    </Transition>
  );
};

export default DiaCompleto;
