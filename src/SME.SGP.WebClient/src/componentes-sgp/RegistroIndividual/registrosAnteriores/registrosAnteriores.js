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
  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);

  const mostrarMensagemSemHistorico = useSelector(
    store => store.registroIndividual.mostrarMensagemSemHistorico
  );

  const idCollapse = shortid.generate();
  const dispatch = useDispatch();

  const expandirAlternado = useCallback(() => setExpandir(!expandir), [
    expandir,
  ]);

  const mensagemHistorico = mostrarMensagemSemHistorico
    ? ' - Sem histórico de registros'
    : '';
  const tituloCollapse = `Registros anteriores${mensagemHistorico}`;

  const { anoLetivo, consideraHistorico } = turmaSelecionada;
  const dataLimiteInferior = `${anoLetivo}-01-01`;
  const dataLimiteSuperior = consideraHistorico
    ? `${anoLetivo}-12-31`
    : window.moment().format('YYYY-MM-DD');

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
          titulo={tituloCollapse}
          indice={`${idCollapse}-collapse-indice`}
          alt={`${idCollapse}-alt`}
          show={expandir}
          onClick={expandirAlternado}
        >
          <RegistrosAnterioresConteudo
            permissoesTela={permissoesTela}
            periodoInicio={dataLimiteInferior}
            periodoFim={dataLimiteSuperior}
          />
        </CardCollapse>
      </div>
    </Loader>
  );
};

export default RegistrosAnteriores;
