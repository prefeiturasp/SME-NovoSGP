import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { Transition } from 'react-spring/renderprops';
import { animated } from 'react-spring';
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
  const { meses } = props;

  const mesesLista = meses.split(',');
  const mesesCalendario = useSelector(state => state.calendarioEscolar.meses);

  let mesSelecionado = -1;
  const [ultimoUsado, setUltimoUsado] = useState(-1);

  const [diasDaSemana, setDiasDaSemana] = useState([]);
  const [estaAberto, setEstaAberto] = useState(false);

  useEffect(() => {
    mesesLista.forEach(mes => {
      if (mesesCalendario[mes].estaAberto) {
        setEstaAberto(true);
        mesSelecionado = parseInt(mes, 10);
      }
    });

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
    }
  }, [mesesCalendario]);

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
        display: 'none',
        height: 0,
        overflow: 'hidden',
      }}
    >
      {exibir =>
        exibir &&
        (style => (
          <animated.div className="border border-top-0 w-100" style={style}>
            <Div className="w-100 d-flex py-3">
              <DiaDaSemana nomeDia="Domingo" />
              <DiaDaSemana nomeDia="Segunda" />
              <DiaDaSemana nomeDia="Terça" />
              <DiaDaSemana nomeDia="Quarta" />
              <DiaDaSemana nomeDia="Quinta" />
              <DiaDaSemana nomeDia="Sexta" />
              <DiaDaSemana nomeDia="Sábado" />
            </Div>
            <Semana inicial dias={diasDaSemana[0]} mesAtual={ultimoUsado} />
            {/* <DiaCompleto dias={diasDaSemana[0]} /> */}
            <Semana dias={diasDaSemana[1]} mesAtual={ultimoUsado} />
            {/* <DiaCompleto dias={diasDaSemana[1]} /> */}
            <Semana dias={diasDaSemana[2]} mesAtual={ultimoUsado} />
            {/* <DiaCompleto dias={diasDaSemana[2]} /> */}
            <Semana dias={diasDaSemana[3]} mesAtual={ultimoUsado} />
            {/* <DiaCompleto dias={diasDaSemana[3]} /> */}
            <Semana dias={diasDaSemana[4]} mesAtual={ultimoUsado} />
            {/* <DiaCompleto dias={diasDaSemana[4]} /> */}
            <Semana
              className="pb-4"
              dias={diasDaSemana[5]}
              mesAtual={ultimoUsado}
            />
            {/* <DiaCompleto dias={diasDaSemana[5]} /> */}
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
