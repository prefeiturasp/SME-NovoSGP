import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';

import { RotasDto } from '~/dtos';
import modalidade from '~/dtos/modalidade';
import {
  setDesabilitarCamposPlanoAula,
  setExibirSwitchEscolhaObjetivos,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import CabecalhoDadosPlanoAula from './CabecalhoDadosPlanoAula/cabecalhoDadosPlanoAula';
import DesenvolvimentoDaAula from './CamposEditorPlanoAula/desenvolvimentoDaAula';
import LicaoDeCasa from './CamposEditorPlanoAula/licaoDeCasa';
import ObjetivosEspecificosParaAula from './CamposEditorPlanoAula/objetivosEspecificosParaAula';
import RecuperacaoContinua from './CamposEditorPlanoAula/recuperacaoContinua';
import ModalCopiarConteudoPlanoAula from './ModalCopiarConteudo/modalCopiarConteudoPlanoAula';
import ModalErrosPlanoAula from './ModalErros/modalErrosPlanoAula';
import ObjetivosAprendizagemDesenvolvimento from './ObjetivosAprendizagemDesenvolvimento/objetivosAprendizagemDesenvolvimento';

const DadosPlanoAula = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(state => state.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const dadosPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.dadosPlanoAula
  );

  const somenteConsulta = useSelector(
    state => state.frequenciaPlanoAula.somenteConsulta
  );

  const { ehProfessorCj, turmaSelecionada } = usuario;

  const componenteCurricular = useSelector(
    store => store.frequenciaPlanoAula.componenteCurricular
  );

  useEffect(() => {
    if (dadosPlanoAula && dadosPlanoAula.id > 0) {
      const desabilitar = !permissoesTela.podeAlterar || somenteConsulta;
      dispatch(setDesabilitarCamposPlanoAula(desabilitar));
    } else {
      const desabilitar = !permissoesTela.podeIncluir || somenteConsulta;
      dispatch(setDesabilitarCamposPlanoAula(desabilitar));
    }
  }, [permissoesTela, somenteConsulta, dadosPlanoAula, dispatch]);

  useEffect(() => {
    const ehEja = !!(
      turmaSelecionada &&
      String(turmaSelecionada.modalidade) === String(modalidade.EJA)
    );

    const ehMedio = !!(
      turmaSelecionada &&
      String(turmaSelecionada.modalidade) === String(modalidade.ENSINO_MEDIO)
    );

    const esconderSwitch =
      !(componenteCurricular && componenteCurricular.possuiObjetivos) ||
      !ehProfessorCj ||
      ehEja ||
      ehMedio;

    dispatch(setExibirSwitchEscolhaObjetivos(!esconderSwitch));
  }, [turmaSelecionada, ehProfessorCj, componenteCurricular, dispatch]);

  return (
    <>
      {dadosPlanoAula ? (
        <>
          <ModalErrosPlanoAula />
          <ModalCopiarConteudoPlanoAula />
          <CabecalhoDadosPlanoAula />
          <ObjetivosAprendizagemDesenvolvimento />
          <ObjetivosEspecificosParaAula />
          <DesenvolvimentoDaAula />
          <RecuperacaoContinua />
          <LicaoDeCasa />
          {dadosPlanoAula && dadosPlanoAula.id > 0 ? (
            <Auditoria
              className="mt-2"
              alteradoEm={dadosPlanoAula.alteradoEm}
              alteradoPor={dadosPlanoAula.alteradoPor}
              alteradoRf={dadosPlanoAula.alteradoRf}
              criadoEm={dadosPlanoAula.criadoEm}
              criadoPor={dadosPlanoAula.criadoPor}
              criadoRf={dadosPlanoAula.criadoRf}
            />
          ) : (
            ''
          )}
        </>
      ) : (
        ''
      )}
    </>
  );
};

export default DadosPlanoAula;
