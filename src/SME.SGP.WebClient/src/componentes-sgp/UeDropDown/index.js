import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';

function UeDropDown({ form, onChange, dreId, label, url, desabilitado }) {
  const [listaUes, setListaUes] = useState([]);

  useEffect(() => {
    async function buscarUes() {
      const { data } = await AbrangenciaServico.buscarUes(dreId, url);
      if (data) {
        setListaUes(
          data
            .map(item => ({
              desc: `${tipoEscolaDTO[item.tipoEscola]} ${item.nome}`,
              valor: item.codigo,
            }))
            .sort(FiltroHelper.ordenarLista('desc'))
        );
      }
    }
    if (dreId) {
      buscarUes();
    } else {
      setListaUes([]);
    }
  }, [dreId, url]);

  useEffect(() => {
    if (listaUes.length === 1) {
      form.setFieldValue('ueId', listaUes[0].valor);
      onChange(listaUes[0].valor);
    }
  }, [listaUes]);

  return (
    <SelectComponent
      form={form}
      name="ueId"
      className="fonte-14"
      label={!label ? null : label}
      onChange={onChange}
      lista={listaUes}
      valueOption="valor"
      valueText="desc"
      placeholder="Unidade Escolar (UE)"
      disabled={listaUes.length === 0 || listaUes.length === 1 || desabilitado}
    />
  );
}

UeDropDown.propTypes = {
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
  dreId: PropTypes.string,
  label: PropTypes.string,
  url: PropTypes.string,
  desabilitado: PropTypes.bool,
};

UeDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  dreId: '',
  label: null,
  url: '',
  desabilitado: false,
};

export default UeDropDown;
