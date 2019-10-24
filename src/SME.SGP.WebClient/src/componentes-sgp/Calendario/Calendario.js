import React from 'react';
import styled from 'styled-components';
import Mes from './Mes';
import MesCompleto from './MesCompleto';

const Div = styled.div``;

const Calendario = () => {
  return (
    <Div className="px-3 pt-3 pb-5">
      <Div className="d-flex">
        <Mes mes="1" />
        <Mes mes="2" />
        <Mes mes="3" />
        <Mes mes="4" />
      </Div>

      <MesCompleto meses="1,2,3,4" />

      <Div className="d-flex">
        <Mes mes="5" />
        <Mes mes="6" />
        <Mes mes="7" />
        <Mes mes="8" />
      </Div>

      <MesCompleto meses="5,6,7,8" />

      <Div className="d-flex">
        <Mes mes="9" />
        <Mes mes="10" />
        <Mes mes="11" />
        <Mes mes="12" />
      </Div>

      <MesCompleto meses="9,10,11,12" />
    </Div>
  );
};

export default Calendario;
