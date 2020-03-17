import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AbrangenciaServico from '~/servicos/Abrangencia';

import FiltroHelper from '~/componentes-sgp/filtro/helper';
import tipoEscolaDTO from '~/dtos/tipoEscolaDto';
import Loader from '~/componentes/loader';

function UeDropDown({
  form,
  onChange,
  dreId,
  label,
  url,
  desabilitado,
  opcaoTodas,
}) {
  const [carregando, setCarregando] = useState(false);
  const [listaUes, setListaUes] = useState([]);
  const [forcaDesabilitado, setForcaDesabilitado] = useState(false);

  useEffect(() => {
    async function buscarUes() {
      setCarregando(true);
      const { data } = await AbrangenciaServico.buscarUes(dreId, url);
      let lista = [];
      if (data) {
        lista = data
          .map(item => ({
            desc: `${tipoEscolaDTO[item.tipoEscola]} ${item.nome}`,
            valor: item.codigo,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
      }
      if (lista.length > 1) {
        if (opcaoTodas) {
          lista.unshift({ desc: 'Todas', valor: '0' });
          setForcaDesabilitado(true);
        }
      } else if (!lista.length) {
        if (opcaoTodas) {
          lista.unshift({ desc: 'Todas', valor: '0' });
          setForcaDesabilitado(true);
        }
      }
      setListaUes(lista);
      setCarregando(false);
    }
    if (dreId) {
      buscarUes();
    } else {
      setListaUes([]);
    }
  }, [dreId, opcaoTodas, url]);

  useEffect(() => {
    form.setFieldValue('ueId', undefined);

    if (listaUes.length === 1) {
      form.setFieldValue('ueId', listaUes[0].valor);
      onChange(listaUes[0].valor);
    }
  }, [listaUes]);

  return (
    <Loader loading={carregando} tip="">
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
        disabled={
          dreId === '0'
            ? forcaDesabilitado || desabilitado
            : listaUes.length === 0 || listaUes.length === 1 || desabilitado
        }
      />
    </Loader>
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
  opcaoTodas: PropTypes.bool,
};

UeDropDown.defaultProps = {
  form: {},
  onChange: () => {},
  dreId: '',
  label: null,
  url: '',
  desabilitado: false,
  opcaoTodas: false,
};

export default UeDropDown;
