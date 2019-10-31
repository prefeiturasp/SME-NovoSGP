import React from 'react';
import styled from 'styled-components';
import Mes from './Mes';
import MesCompleto from './MesCompleto';

const Div = styled.div``;

const Calendario = props => {
  const { filtros } = props;

  return (
    <Div>
      <Div className="d-flex">
        <Mes numeroMes="1" filtros={filtros} />
        <Mes numeroMes="2" filtros={filtros} />
        <Mes numeroMes="3" filtros={filtros} />
        <Mes numeroMes="4" filtros={filtros} />
      </Div>

      <MesCompleto meses="1,2,3,4" />

      <Div className="d-flex">
        <Mes numeroMes="5" filtros={filtros} />
        <Mes numeroMes="6" filtros={filtros} />
        <Mes numeroMes="7" filtros={filtros} />
        <Mes numeroMes="8" filtros={filtros} />
      </Div>

      <MesCompleto meses="5,6,7,8" />

      <Div className="d-flex">
        <Mes numeroMes="9" filtros={filtros} />
        <Mes numeroMes="10" filtros={filtros} />
        <Mes numeroMes="11" filtros={filtros} />
        <Mes numeroMes="12" filtros={filtros} />
      </Div>

      <MesCompleto meses="9,10,11,12" />
    </Div>
  );
};

export default Calendario;
