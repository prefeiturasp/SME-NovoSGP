import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { store } from '~/redux';
import {
  selecionaMes,
  atribuiEventosMes,
} from '~/redux/modulos/calendarioEscolar/actions';
import { Base } from '~/componentes/colors';
import api from '~/servicos/api';
import { erro } from '~/servicos/alertas';

const Div = styled.div``;
const Icone = styled.i`
  cursor: pointer;
`;

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

const Mes = props => {
  const { numeroMes, filtros } = props;
  const [mesSelecionado, setMesSelecionado] = useState({});

  useEffect(() => {
    const dataAtual = new Date();
    if (numeroMes === (dataAtual.getMonth() + 1).toString())
      store.dispatch(selecionaMes(numeroMes));
  }, []);

  useEffect(() => {
    if (filtros && Object.entries(filtros).length > 0) {
      const {
        tipoCalendarioSelecionado = '',
        eventoSme = true,
        dreSelecionada = '',
        unidadeEscolarSelecionada = '',
      } = filtros;
      api
        .get(
          `v1/calendarios/eventos/meses?${dreSelecionada &&
            `DreId=${dreSelecionada}&`}${eventoSme &&
            `EhEventoSme=${eventoSme}&`}${tipoCalendarioSelecionado &&
            `IdTipoCalendario=${tipoCalendarioSelecionado}&`}${unidadeEscolarSelecionada &&
            `UeId=${unidadeEscolarSelecionada}`}`
        )
        .then(resposta => {
          if (resposta.data) {
            resposta.data.forEach(item => {
              store.dispatch(atribuiEventosMes(item.mes, item.eventos));
            });
          }
        })
        .catch(() => {
          erro('Não encontramos eventos para estes filtros!');
          store.dispatch(atribuiEventosMes(numeroMes, 0));
        });
    }
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

    if (mes.eventos > 0) mes.chevronColor = Base.AzulCalendario;

    setMesSelecionado(mes);
  }, [meses]);

  const abrirMes = () => {
    store.dispatch(selecionaMes(numeroMes));
  };

  return (
    <Div className="col-3 w-100 px-0">
      <Div className={mesSelecionado.className}>
        <Div
          className="d-flex align-items-center justify-content-center"
          onClick={abrirMes}
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
