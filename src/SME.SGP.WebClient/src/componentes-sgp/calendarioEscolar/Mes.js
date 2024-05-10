import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { store } from '~/redux';
import {
  selecionaMes,
  selecionaDia,
} from '~/redux/modulos/calendarioEscolar/actions';
import { Base } from '~/componentes/colors';

const Div = styled.div``;
const Icone = styled.i``;

const Seta = props => {
  const { estaAberto } = props;

  return (
    <Icone
      className={`fas ${
        estaAberto ? 'fa-chevron-down' : 'fa-chevron-right text-white'
      } `}
    />
  );
};

Seta.propTypes = {
  estaAberto: PropTypes.bool,
};

Seta.defaultProps = {
  estaAberto: false,
};

const Mes = props => {
  const { numeroMes, filtros } = props;
  const { tipoCalendarioSelecionado } = filtros;
  const [mesSelecionado, setMesSelecionado] = useState({});

  const meses = useSelector(state => state.calendarioEscolar.meses);

  useEffect(() => {
    const mes = Object.assign({}, meses[numeroMes]);
    mes.style = { backgroundColor: Base.CinzaCalendario, color: Base.Preto };

    if (mes.estaAberto) {
      mes.chevronColor = Base.Branco;
      mes.className += ' border-bottom-0';
      mes.style = { color: Base.Preto };
    }

    mes.chevronColor = Base.AzulCalendario;
    if (mes.estaAberto) mes.chevronColor = Base.Branco;

    setMesSelecionado(mes);
  }, [meses, numeroMes]);

  const abrirMes = () => {
    if (tipoCalendarioSelecionado) store.dispatch(selecionaMes(numeroMes));
  };

  useEffect(() => {
    const encontrarMes = setTimeout(() => {
      const mes = document.querySelector(`.${meses[numeroMes].nome}`);
      if (mes) {
        if (meses[numeroMes].estaAberto) {
          mes.classList.remove('d-none');
          mes.classList.add('d-block', 'show');
        } else {
          mes.classList.remove('d-block', 'show');
          mes.classList.add('d-none');
          store.dispatch(selecionaDia(undefined));
        }
      }
    }, 500);
    return () => clearTimeout(encontrarMes);
  }, [meses, numeroMes]);

  return (
    <Div className="col-3 w-100 px-0">
      <Div
        className={mesSelecionado.className}
        onClick={abrirMes}
        style={{
          cursor: tipoCalendarioSelecionado ? 'pointer' : 'not-allowed',
        }}
      >
        <Div
          className="d-flex align-items-center justify-content-center position-relative"
          style={{
            backgroundColor: mesSelecionado.chevronColor,
            height: 75,
            width: 35,
          }}
        >
          <Seta estaAberto={mesSelecionado.estaAberto} />
        </Div>
        <Div
          className="d-flex align-items-center w-100"
          style={mesSelecionado.style}
        >
          <Div className="w-100 pl-2 fonte-16">{mesSelecionado.nome}</Div>
          <Div className="flex-shrink-1 d-flex align-items-center pr-3">
            <Div className="fonte-14">
              <Icone className="far fa-calendar-alt" />
            </Div>
          </Div>
        </Div>
      </Div>
    </Div>
  );
};

Mes.propTypes = {
  numeroMes: PropTypes.string,
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

Mes.defaultProps = {
  numeroMes: '',
  filtros: {},
};

export default Mes;
