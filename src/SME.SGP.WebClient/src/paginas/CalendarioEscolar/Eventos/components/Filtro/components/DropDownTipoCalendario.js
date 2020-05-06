import React, { useEffect, useState, useMemo } from 'react';
import t from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent, Loader } from '~/componentes';

// Serviços
import EventosServico from '~/servicos/Paginas/CalendarioProfessor/Eventos';
import { erro } from '~/servicos/alertas';

// DTOs
import ModalidadeDTO from '~/dtos/modalidade';

function DropDownTipoCalendario({ form, onChange }) {
  const { turmaSelecionada } = useSelector(x => x.usuario);
  const [listaTipos, setListaTipos] = useState([]);
  const [carregandoTipos, setCarregandoTipos] = useState(false);

  const listaTiposCalendario = useMemo(() => {
    let retorno = listaTipos;
    if (Object.entries(turmaSelecionada).length > 0) {
      retorno = listaTipos.filter(
        x =>
          x.modalidade ===
          (String(turmaSelecionada.modalidade) === String(ModalidadeDTO.EJA)
            ? 2
            : 1)
      );
    }

    return retorno;
  }, [listaTipos, turmaSelecionada]);

  useEffect(() => {
    async function buscarTipos() {
      try {
        setCarregandoTipos(true);
        const { data, status } = await EventosServico.buscarTiposCalendario(
          turmaSelecionada.anoLetivo
        );
        if (data && status === 200) {
          setListaTipos(
            data.map(tipo => ({
              id: String(tipo.id),
              descricaoTipoCalendario: `${tipo.anoLetivo} - ${tipo.nome} - ${tipo.descricaoPeriodo}`,
              ...tipo,
            }))
          );
          setCarregandoTipos(false);
        }
      } catch (error) {
        setCarregandoTipos(false);
        erro(`Não foi possível obter os tipos de calendário. ${error}`);
      }
    }
    buscarTipos();
  }, [turmaSelecionada]);

  return (
    <Loader loading={carregandoTipos} tip="">
      <SelectComponent
        name="tipoCalendarioId"
        id="select-tipo-calendario"
        lista={listaTiposCalendario}
        valueOption="id"
        valueText="descricaoTipoCalendario"
        onChange={valor => onChange(valor)}
        placeholder="Selecione um calendário"
        form={form}
      />
    </Loader>
  );
}

DropDownTipoCalendario.propTypes = {
  form: t.oneOfType([t.any]),
  onChange: t.func,
};

DropDownTipoCalendario.defaultProps = {
  form: {},
  onChange: () => {},
};

export default DropDownTipoCalendario;
