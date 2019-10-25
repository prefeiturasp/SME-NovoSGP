import React from 'react';
import styled from 'styled-components';
import Mes from './Mes';
import MesCompleto from './MesCompleto';

const Div = styled.div``;

const Calendario = () => {
  return (
    <Div className="px-3 pt-3 pb-5">
      <Div className="d-flex">
        <Mes numeroMes="1" />
        <Mes numeroMes="2" />
        <Mes numeroMes="3" />
        <Mes numeroMes="4" />
      </Div>

      <MesCompleto meses="1,2,3,4" />

      <Div className="d-flex">
        <Mes numeroMes="5" />
        <Mes numeroMes="6" />
        <Mes numeroMes="7" />
        <Mes numeroMes="8" />
      </Div>

      <MesCompleto meses="5,6,7,8" />

      <Div className="d-flex">
        <Mes numeroMes="9" />
        <Mes numeroMes="10" />
        <Mes numeroMes="11" />
        <Mes numeroMes="12" />
      </Div>

      <MesCompleto meses="9,10,11,12" />
    </Div>
  );
};

export default Calendario;
