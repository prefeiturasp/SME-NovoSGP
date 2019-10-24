import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { useTransition, animated } from 'react-spring';
import { store } from '~/redux';
import { alternaMes } from '~/redux/modulos/calendarioEscolar/actions';

const Div = styled.div``;

const Seta = props => {
  const transitions = useTransition(props.estaAberto, item => item, {
    from: { display: 'none' },
    enter: { display: 'block' },
    leave: { display: 'none' },
  });

  console.log(props);

  return transitions.map(({ item, key, props }) =>
    item ? (
      <animated.div style={props} key={key}>
        <i className="fas fa-chevron-down" />
      </animated.div>
    ) : (
      <animated.div style={props} key={key}>
        <i className="fas fa-chevron-right text-white" />
      </animated.div>
    )
  );
};

const Mes = props => {
  const { mes } = props;
  let meses = [];

  useEffect(() => {
    const dataAtual = new Date();
    if (mes === (dataAtual.getMonth() + 1).toString())
      store.dispatch(alternaMes(mes));
  }, []);

  meses = useSelector(state => state.calendarioEscolar.meses);

  const mesSelecionado = Object.assign({}, meses[mes]);
  mesSelecionado.style = { backgroundColor: '#F7F9FA' };
  if (mesSelecionado.appointments > 0) mesSelecionado.chevronColor = '#10A3FB';

  useEffect(() => {
    if (mesSelecionado.estaAberto) {
      mesSelecionado.chevronColor = 'rgba(0,0,0,0)';
      mesSelecionado.className += ' border-bottom-0';
      mesSelecionado.style = {};
    }
  }, [meses[mes].estaAberto]);

  const abrirMes = () => {
    store.dispatch(alternaMes(mes));
  };

  return (
    <div className="col-3 w-100 px-0">
      <div className={mesSelecionado.className}>
        <Div
          className="d-flex align-items-center justify-content-center clickable"
          onClick={abrirMes}
          style={{
            backgroundColor: mesSelecionado.chevronColor,
            height: 75,
            width: 33,
          }}
        >
          <Seta estaAberto={mesSelecionado.estaAberto} />
        </Div>

        <div
          className="d-flex align-items-center w-100"
          style={mesSelecionado.style}
        >
          <div className="w-100 pl-2">{mesSelecionado.name}</div>
          <div className="flex-shrink-1 d-flex align-items-center pr-3">
            <div className="pr-2">{mesSelecionado.appointments}</div>
            <div>
              <i className="far fa-calendar-alt" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

Mes.propTypes = {
  mes: PropTypes.string,
};

Mes.defaultProps = {
  mes: '',
};

export default Mes;
