import React, { useState, useEffect } from 'react';

// Componentes
import { SelectComponent } from '~/componentes';

// Servicos
import AtribuicaoEsporadicaServico from '~/servicos/Paginas/AtribuicaoEsporadica';

function UeDropDown({ form, onChange, dreId }) {
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
    }
  }, [listaUes]);

  return (
    <SelectComponent
      form={form}
      name="ueId"
      className="fonte-14"
      onChange={onChange}
      lista={listaUes}
      valueOption="valor"
      valueText="desc"
      placeholder="Unidade Escolar (UE)"
      disabled={listaUes.length === 0 || listaUes.length === 1}
    />
  );
}

export default UeDropDown;
