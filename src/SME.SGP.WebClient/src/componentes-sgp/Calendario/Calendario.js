import React from 'react';
import PropTypes from 'prop-types';
import Month from './Mes';
import FullMonth from './MesCompleto';

const Calendario = props => {
  const { nome } = props;

  return (
    <div className="px-3 pt-3 pb-5">
      <div className="col-2 px-0 pb-4">
        <input
          type="text"
          className="form-control form-control-sm"
          value={nome}
          readOnly
          style={{ backgroundColor: 'rgba(0,0,0,0)' }}
        />
      </div>

      <div className="d-flex">
        <Month mes="1" />
        <Month mes="2" />
        <Month mes="3" />
        <Month mes="4" />
      </div>

      <FullMonth months="1,2,3,4" />

      <div className="d-flex">
        <Month mes="5" />
        <Month mes="6" />
        <Month mes="7" />
        <Month mes="8" />
      </div>

      <FullMonth months="5,6,7,8" />

      <div className="d-flex">
        <Month mes="9" />
        <Month mes="10" />
        <Month mes="11" />
        <Month mes="12" />
      </div>

      <FullMonth months="9,10,11,12" />
    </div>
  );
};

Calendario.propTypes = {
  nome: PropTypes.string,
};

Calendario.defaultProps = {
  nome: 'Calendário',
};

export default Calendario;
