import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';

import { CardCollapse, Loader } from '~/componentes';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';
import { RotasDto } from '~/dtos';

import { setRecolherRegistrosAnteriores } from '~/redux/modulos/registroIndividual/actions';

import { RegistrosAnterioresConteudo } from './registrosAnterioresConteudo';

const RegistrosAnteriores = () => {
  const [expandir, setExpandir] = useState(false);

  const recolherRegistrosAnteriores = useSelector(
    store => store.registroIndividual.recolherRegistrosAnteriores
  );
  const exibirLoaderGeralRegistroAnteriores = useSelector(
    store => store.registroIndividual.exibirLoaderGeralRegistroAnteriores
  );

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.REGISTRO_INDIVIDUAL];

  const idCollapse = shortid.generate();
  const dispatch = useDispatch();

  const expandirAlternado = useCallback(() => setExpandir(!expandir), [
    expandir,
  ]);

  useEffect(() => {
    if (recolherRegistrosAnteriores && expandir) {
      expandirAlternado();
    }
    dispatch(setRecolherRegistrosAnteriores(false));
  }, [recolherRegistrosAnteriores, expandirAlternado, expandir, dispatch]);

  return (
    <Loader
      ignorarTip
      loading={exibirLoaderGeralRegistroAnteriores}
      className="w-100"
    >
      <div key={shortid.generate()} className="px-4 pt-4">
        <CardCollapse
          configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
          styleCardBody={{ paddingTop: 12 }}
          key={`${idCollapse}-collapse-key`}
          titulo="Registros anteriores"
          indice={`${idCollapse}-collapse-indice`}
          alt={`${idCollapse}-alt`}
          show={expandir}
          onClick={expandirAlternado}
        >
          <RegistrosAnterioresConteudo permissoesTela={permissoesTela} />
        </CardCollapse>
      </div>
    </Loader>
  );
};

export default RegistrosAnteriores;
