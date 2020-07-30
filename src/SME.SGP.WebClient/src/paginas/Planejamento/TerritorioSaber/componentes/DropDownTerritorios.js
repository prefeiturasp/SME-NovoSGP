import React, { useEffect, useState } from 'react';
import t from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import { SelectComponent, Loader } from '~/componentes';

// Serviços
import AbrangenciaServico from '~/servicos/Abrangencia';
import modalidade from '~/dtos/modalidade';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

function DropDownTerritorios({
  onChangeTerritorio,
  territorioSelecionado,
  onBuscarTerritorios,
}) {
  const [carregando, setCarregando] = useState(false);
  const [listaTerritorios, setListaTerritorios] = useState([]);
  const [possuiUmUnicoTerritorio, setPossuiUmUnicoTerritorio] = useState(false);

  const { turmaSelecionada } = useSelector(state => state.usuario);
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  useEffect(() => {
    async function buscarTerritorios() {
      setCarregando(true);
      const { data } = await AbrangenciaServico.buscarDisciplinas(
        turmaSelecionada.turma
      );
      if (data) {
        const lista = data.filter(x => x.territorioSaber === true);

        setPossuiUmUnicoTerritorio(lista.length <= 1);

        setListaTerritorios(lista);

        onBuscarTerritorios(lista.length === 0);
      }
      setCarregando(false);
    }
    if (
      Object.keys(turmaSelecionada).length > 0 &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      onChangeTerritorio(undefined);
      buscarTerritorios();
    } else {
      setListaTerritorios([]);
      onBuscarTerritorios(false);
    }
  }, [onChangeTerritorio, turmaSelecionada, modalidadesFiltroPrincipal]);

  useEffect(() => {
    if (listaTerritorios.length === 1) {
      onChangeTerritorio(
        String(listaTerritorios[0].codigoComponenteCurricular)
      );
    }
  }, [listaTerritorios, onChangeTerritorio]);

  return (
    <Loader tip={false} loading={carregando}>
      <SelectComponent
        name="territorio"
        id="territorio"
        lista={listaTerritorios}
        valueOption="codigoComponenteCurricular"
        valueText="nome"
        onChange={valor => onChangeTerritorio(valor)}
        valueSelect={territorioSelecionado || undefined}
        placeholder="Selecione um território"
        disabled={possuiUmUnicoTerritorio}
      />
    </Loader>
  );
}

DropDownTerritorios.propTypes = {
  onChangeTerritorio: t.func,
  territorioSelecionado: t.oneOfType([t.any]),
  onBuscarTerritorios: t.func,
};

DropDownTerritorios.defaultProps = {
  onChangeTerritorio: () => null,
  territorioSelecionado: {},
  onBuscarTerritorios: () => null,
};

export default DropDownTerritorios;
