import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { store } from '~/redux';
import { atribuiEventosMes } from '~/redux/modulos/calendarioProfessor/actions';
import Mes from './Mes';
import MesCompleto from './MesCompleto';
import { Base } from '~/componentes/colors';
import api from '~/servicos/api';
import { Loader } from '~/componentes';

const Div = styled.div`
  .border {
    border-color: ${Base.CinzaBordaCalendario} !important;
  }
  .badge-light {
    background-color: ${Base.CinzaBadge} !important;
  }
`;

const Calendario = props => {
  const { filtros } = props;
  const {
    tipoCalendarioSelecionado,
    eventoSme,
    dreSelecionada,
    unidadeEscolarSelecionada,
    turmaSelecionada,
    todasTurmas,
  } = filtros;

  const [carregandoMeses, setCarregandoMeses] = useState(false);

  useEffect(() => {
    let estado = true;
    if (estado) {
      if (
        tipoCalendarioSelecionado &&
        dreSelecionada &&
        unidadeEscolarSelecionada &&
        (turmaSelecionada || todasTurmas)
      ) {
        setCarregandoMeses(true);
        api
          .post('v1/calendarios/meses/eventos-aulas', {
            tipoCalendarioId: tipoCalendarioSelecionado,
            EhEventoSME: eventoSme,
            dreId: dreSelecionada,
            ueId: unidadeEscolarSelecionada,
            turmaId: turmaSelecionada,
            todasTurmas,
          })
          .then(resposta => {
            if (resposta.data) {
              resposta.data.forEach(item => {
                if (item && item.mes > 0) {
                  store.dispatch(
                    atribuiEventosMes(item.mes, item.eventosAulas)
                  );
                }
              });
            }
            setCarregandoMeses(false);
          });
      }
    }
    return () => {
      estado = false;
    };
  }, [filtros]);

  return (
    <Loader loading={carregandoMeses}>
      <Div>
        <Div className="d-flex">
          <Mes numeroMes="1" filtros={filtros} />
          <Mes numeroMes="2" filtros={filtros} />
          <Mes numeroMes="3" filtros={filtros} />
          <Mes numeroMes="4" filtros={filtros} />
        </Div>

        <MesCompleto meses="1,2,3,4" filtros={filtros} />

        <Div className="d-flex">
          <Mes numeroMes="5" filtros={filtros} />
          <Mes numeroMes="6" filtros={filtros} />
          <Mes numeroMes="7" filtros={filtros} />
          <Mes numeroMes="8" filtros={filtros} />
        </Div>

        <MesCompleto meses="5,6,7,8" filtros={filtros} />

        <Div className="d-flex">
          <Mes numeroMes="9" filtros={filtros} />
          <Mes numeroMes="10" filtros={filtros} />
          <Mes numeroMes="11" filtros={filtros} />
          <Mes numeroMes="12" filtros={filtros} />
        </Div>

        <MesCompleto meses="9,10,11,12" filtros={filtros} />
      </Div>
    </Loader>
  );
};

Calendario.propTypes = {
  filtros: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
};

Calendario.defaultProps = {
  filtros: {},
};

export default Calendario;
