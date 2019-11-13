import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { store } from '~/redux';
import {
  selecionaMes,
  atribuiEventosMes,
  selecionaDia,
} from '~/redux/modulos/calendarioEscolar/actions';
import { Base } from '~/componentes/colors';
import api from '~/servicos/api';

const Div = styled.div`
  ${props =>
    props.disabled &&
    `
    background: ${Base.CinzaBarras};
    opacity: 0.5;
    pointer-events: none;
  `}
`;
const Icone = styled.i`
  cursor: pointer;
  ${props =>
    props.disabled &&
    `
    cursor: not-allowed;
    pointer-events: none;
  `}
`;

const Seta = props => {
  const { estaAberto, disabled } = props;

  return (
    <Icone
      disabled={disabled}
      className={`stretched-link fas ${
        estaAberto ? 'fa-chevron-down' : 'fa-chevron-right text-white'
      } `}
    />
  );
};

Seta.propTypes = {
  estaAberto: PropTypes.bool,
  disabled: PropTypes.bool,
};

Seta.defaultProps = {
  estaAberto: false,
  disabled: true,
};

const Mes = props => {
  const { numeroMes, filtros } = props;
  const [mesSelecionado, setMesSelecionado] = useState({});

  useEffect(() => {
    let estado = true;
    if (estado) {
      if (filtros && Object.entries(filtros).length > 0) {
        const {
          tipoCalendarioSelecionado = '',
          eventoSme = true,
          dreSelecionada = '',
          unidadeEscolarSelecionada = '',
        } = filtros;
        if (tipoCalendarioSelecionado) {
          api
            .get(
              `v1/calendarios/eventos/meses?EhEventoSme=${eventoSme}&${dreSelecionada &&
                `DreId=${dreSelecionada}&`}${tipoCalendarioSelecionado &&
                `IdTipoCalendario=${tipoCalendarioSelecionado}&`}${unidadeEscolarSelecionada &&
                `UeId=${unidadeEscolarSelecionada}`}`
            )
            .then(resposta => {
              if (resposta.data) {
                resposta.data.forEach(item => {
                  if (item && item.mes > 0)
                    store.dispatch(atribuiEventosMes(item.mes, item.eventos));
                });
              } else store.dispatch(atribuiEventosMes(numeroMes, 0));
            })
            .catch(() => {
              store.dispatch(atribuiEventosMes(numeroMes, 0));
            });
        } else store.dispatch(atribuiEventosMes(numeroMes, 0));
      }
    }
    return () => (estado = false);
  }, [filtros]);

  const meses = useSelector(state => state.calendarioEscolar.meses);

  useEffect(() => {
    const mes = Object.assign({}, meses[numeroMes]);
    mes.style = { backgroundColor: Base.CinzaCalendario, color: Base.Preto };

    if (mes.estaAberto) {
      mes.chevronColor = Base.Branco;
      mes.className += ' border-bottom-0';
      mes.style = { color: Base.Preto };
    }

    if (mes.eventos > 0 && !mes.estaAberto)
      mes.chevronColor = Base.AzulCalendario;
    else if (mes.estaAberto) mes.chevronColor = Base.Branco;

    setMesSelecionado(mes);
  }, [meses]);

  const abrirMes = () => {
    if (
      filtros &&
      Object.entries(filtros).length > 0 &&
      meses[numeroMes].eventos > 0
    ) {
      const { tipoCalendarioSelecionado = '' } = filtros;
      if (tipoCalendarioSelecionado) store.dispatch(selecionaMes(numeroMes));
    }
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
  }, [meses[numeroMes].estaAberto]);

  return (
    <Div
      className="col-3 w-100 px-0"
      disabled={!mesSelecionado.estaAberto && mesSelecionado.eventos === 0}
    >
      <Div className={mesSelecionado.className}>
        <Div
          className="d-flex align-items-center justify-content-center position-relative"
          onClick={abrirMes}
          style={{
            backgroundColor: mesSelecionado.chevronColor,
            height: 75,
            width: 35,
          }}
        >
          <Seta
            estaAberto={mesSelecionado.estaAberto}
            disabled={
              !mesSelecionado.estaAberto && mesSelecionado.eventos === 0
            }
          />
        </Div>
        <Div
          className="d-flex align-items-center w-100"
          style={mesSelecionado.style}
        >
          <Div className="w-100 pl-2">{mesSelecionado.nome}</Div>
          <Div className="flex-shrink-1 d-flex align-items-center pr-3">
            <Div className="pr-2">{mesSelecionado.eventos}</Div>
            <Div>
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
