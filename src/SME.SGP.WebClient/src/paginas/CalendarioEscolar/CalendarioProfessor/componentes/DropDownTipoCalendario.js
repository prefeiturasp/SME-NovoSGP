import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { Loader, SelectComponent } from '~/componentes';

function DropDownTipoCalendario({ label, onChange, desabilitado }) {
  const [carregando, setCarregando] = useState(false);
  const [listaTipoCalendario, setListaTipoCalendario] = useState([]);

  // TODO: Fazer a busca ao novo endpoint de tipos de calendario para calendario do professor
  useEffect(() => {}, []);

  return (
    <Loader loading={carregando}>
      <SelectComponent
        label={!label ? null : label}
        name="tipoCalendarioId"
        className="fonte-14"
        onChange={onChange}
        lista={listaTipoCalendario}
        valueOption="valor"
        valueText="desc"
        placeholder="Selecione o tipo de calendário..."
        disabled={listaTipoCalendario.length === 1 || desabilitado}
      />
    </Loader>
  );
}

DropDownTipoCalendario.propTypes = {
  label: t.string,
  onChange: t.func,
  desabilitado: t.bool,
};

DropDownTipoCalendario.defaultProps = {
  label: '',
  onChange: () => {},
  desabilitado: false,
};

export default DropDownTipoCalendario;
