import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';
import FiltroHelper from '~/componentes-sgp/filtro/helper';

function DreDropDown({ form, onChange, label, url }) {
  const [listaDres, setListaDres] = useState([]);

  useEffect(() => {
    async function buscarDres() {
      const { data } = await AbrangenciaServico.buscarDres(url);
      if (data) {
        setListaDres(
          data
            .map(item => ({
              desc: item.nome,
              valor: item.codigo,
              abrev: item.abreviacao,
            }))
            .sort(FiltroHelper.ordenarLista('desc'))
        );
      }
    }
    buscarDres();
  }, [url]);

  useEffect(() => {
    if (listaDres.length === 1) {
      form.setFieldValue('dreId', listaDres[0].valor);
      onChange(listaDres[0].valor);
    }
  }, [listaDres]);

  useEffect(() => {
    if (!valorNuloOuVazio(form.values.dreId)) {
      onChange(form.values.dreId);
    }
  }, [form.values.dreId, onChange]);

  return (
    <SelectComponent
      label={!label ? null : label}
      form={form}
      name="dreId"
      className="fonte-14"
      onChange={onChange}
      lista={listaDres}
      valueOption="valor"
      valueText="desc"
      placeholder="Diretoria Regional De Educação (DRE)"
      disabled={listaDres.length === 1}
    />
  );
}

DreDropDown.propTypes = {
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onChange: PropTypes.func,
  label: PropTypes.string,
  url: PropTypes.string,
};

DreDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  label: null,
  url: null,
};

export default DreDropDown;
