import React, { useEffect, useState } from 'react';
import t from 'prop-types';

// Componentes
import { Loader, SelectComponent } from '~/componentes';

// Serviços
import { erro } from '~/servicos/alertas';
import EventosServico from '~/servicos/Paginas/CalendarioProfessor/Eventos';

function DropDownTipoEvento({ form, onChange, selecionouCalendario }) {
  const [carregandoTipos, setCarregandoTipos] = useState(false);
  const [listaTipos, setListaTipos] = useState([]);

  useEffect(() => {
    async function buscarTipos() {
      try {
        const { data, status } = await EventosServico.buscarTipoEventos();
        if (data && status === 200) {
          setListaTipos(data.items);
        }
      } catch (error) {
        setCarregandoTipos(false);
        erro(`Não foi possível encontrar Tipos de Eventos. ${error}`);
      }
    }
    buscarTipos();
  }, []);

  return (
    <Loader loading={carregandoTipos} tip="">
      <SelectComponent
        name="tipoEventoId"
        id="select-tipo-evento"
        lista={listaTipos}
        valueOption="id"
        valueText="descricao"
        onChange={valor => onChange(valor)}
        valueSelect={form.values.tipoEvento || undefined}
        placeholder="Selecione um tipo"
        disabled={!selecionouCalendario}
        form={form}
      />
    </Loader>
  );
}

DropDownTipoEvento.propTypes = {
  form: t.oneOfType([t.any]),
  onChange: t.func,
  selecionouCalendario: t.bool,
};

DropDownTipoEvento.defaultProps = {
  form: {},
  onChange: () => {},
  selecionouCalendario: false,
};

export default DropDownTipoEvento;
