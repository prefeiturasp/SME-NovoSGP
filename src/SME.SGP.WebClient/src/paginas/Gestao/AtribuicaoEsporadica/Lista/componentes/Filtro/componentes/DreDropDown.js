import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

function DreDropDown({ form, onChange }) {
  const [listaDres, setListaDres] = useState([]);

  useEffect(() => {
    async function buscarDres() {
      const { data } = await AtribuicaoEsporadicaServico.buscarDres();
      if (data) {
        setListaDres(
          data.map(item => ({
            desc: item.nome,
            valor: item.codigo,
            abrev: item.abreviacao,
          }))
        );
      }
    }
    buscarDres();
  }, []);

  useEffect(() => {
    if (listaDres.length === 1) {
      form.setFieldValue('dreId', listaDres[0].valor);
      onChange(listaDres[0].valor);
    }
  }, [listaDres]);

  return (
    <SelectComponent
      form={form}
      name="dreId"
      className="fonte-14"
      onChange={onChange}
      lista={listaDres}
      valueOption="valor"
      containerVinculoId="containerFiltro"
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
};

DreDropDown.defaultProps = {
  form: {},
  onChange: () => {},
};

export default DreDropDown;
