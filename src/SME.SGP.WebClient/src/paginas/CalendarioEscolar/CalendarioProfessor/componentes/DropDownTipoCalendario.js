import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Componentes
import { Loader, SelectComponent } from '~/componentes';

// Serviços
import CalendarioProfessorServico from '~/servicos/Paginas/CalendarioProfessor';
import { erro } from '~/servicos/alertas';

// Utils
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function DropDownTipoCalendario({
  label,
  onChange,
  desabilitado,
  turmaSelecionada,
  valor,
}) {
  const [carregando, setCarregando] = useState(false);
  const [listaTipoCalendario, setListaTipoCalendario] = useState([]);

  useEffect(() => {
    async function buscarTiposCalendario() {
      try {
        if (valorNuloOuVazio(turmaSelecionada)) {
          setListaTipoCalendario([]);
        } else {
          setCarregando(true);
          const {
            data,
            status,
          } = await CalendarioProfessorServico.buscarTiposCalendario(
            turmaSelecionada
          );
          if (data && status === 200) {
            setListaTipoCalendario(
              [data].map(x => ({
                desc: x.nome,
                valor: String(x.id),
              }))
            );
            setCarregando(false);
          }
        }
      } catch (error) {
        setCarregando(false);
        erro(`Não foi possível buscar tipos de calendário.`);
      }
    }

    buscarTiposCalendario();
  }, [turmaSelecionada]);

  useEffect(() => {
    if (listaTipoCalendario.length === 1) {
      onChange(listaTipoCalendario[0].valor);
    }
  }, [listaTipoCalendario, onChange]);

  return (
    <Loader loading={carregando} tip="">
      <SelectComponent
        label={!label ? null : label}
        name="tipoCalendarioId"
        className="fonte-14"
        onChange={onChange}
        lista={listaTipoCalendario}
        valueSelect={(!valorNuloOuVazio(valor) && String(valor)) || undefined}
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
  turmaSelecionada: t.oneOfType([t.any]),
  valor: t.oneOfType([t.any]),
};

DropDownTipoCalendario.defaultProps = {
  label: '',
  onChange: () => {},
  desabilitado: false,
  turmaSelecionada: undefined,
  valor: undefined,
};

export default DropDownTipoCalendario;
