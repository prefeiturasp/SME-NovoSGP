import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';
import FiltroHelper from '~/componentes-sgp/filtro/helper';
import Loader from '~/componentes/loader';

function DreDropDown({
  form,
  onChange,
  label,
  url,
  desabilitado,
  opcaoTodas,
  temModalidade,
}) {
  const [carregando, setCarregando] = useState(false);
  const [listaDres, setListaDres] = useState([]);

  useEffect(() => {
    async function buscarDres() {
      setCarregando(true);
      const { data } = await AbrangenciaServico.buscarDres(url);
      if (data) {
        const lista = data
          .map(item => ({
            desc: item.nome,
            valor: item.codigo,
            abrev: item.abreviacao,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        if (opcaoTodas && lista.length > 1)
          lista.unshift({ desc: 'Todas', valor: '0' });
        setListaDres(lista);
      }
      setCarregando(false);
    }
    if (temModalidade) buscarDres();
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
    <Loader loading={carregando} tip="">
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
        disabled={listaDres.length === 1 || desabilitado}
      />
    </Loader>
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
  desabilitado: PropTypes.bool,
  opcaoTodas: PropTypes.bool,
  temModalidade: PropTypes.bool,
};

DreDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  label: null,
  url: null,
  desabilitado: false,
  opcaoTodas: false,
  temModalidade: true,
};

export default DreDropDown;
