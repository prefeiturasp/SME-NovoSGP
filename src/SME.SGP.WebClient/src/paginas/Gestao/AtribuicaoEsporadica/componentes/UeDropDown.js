import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';

function UeDropDown({ form, onChange, dreId, label, desabilitado }) {
  const [listaUes, setListaUes] = useState([]);

  async function buscarUes() {
    const { data } = await AtribuicaoEsporadicaServico.buscarUes(dreId);
    if (data) {
      setListaUes(
        data.map(item => ({
          desc: item.nome,
          valor: item.codigo,
        }))
      );
    }
  }

  useEffect(() => {
    if (dreId) {
      buscarUes();
    } else {
      setListaUes([]);
    }
  }, [dreId]);

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
      disabled={listaUes.length === 1 || desabilitado}
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
  desabilitado: PropTypes.bool,
};

UeDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  dreId: '',
  label: null,
  desabilitado: null,
};

export default UeDropDown;
