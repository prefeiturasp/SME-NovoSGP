import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';
import { RotasDto } from '~/dtos';
import { setDesabilitarCamposPlanoAula } from '~/redux/modulos/frequenciaPlanoAula/actions';
import DesenvolvimentoDaAula from './CamposEditorPlanoAula/desenvolvimentoDaAula';
import LicaoDeCasa from './CamposEditorPlanoAula/licaoDeCasa';
import ObjetivosEspecificosParaAula from './CamposEditorPlanoAula/objetivosEspecificosParaAula';
import RecuperacaoContinua from './CamposEditorPlanoAula/recuperacaoContinua';
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

  useEffect(() => {
    if (dadosPlanoAula && dadosPlanoAula.id > 0) {
      const desabilitar = !permissoesTela.podeAlterar || somenteConsulta;
      dispatch(setDesabilitarCamposPlanoAula(desabilitar));
    } else {
      const desabilitar = !permissoesTela.podeIncluir || somenteConsulta;
      dispatch(setDesabilitarCamposPlanoAula(desabilitar));
    }
  }, [permissoesTela, somenteConsulta, dadosPlanoAula, dispatch]);

  return (
    <>
      {dadosPlanoAula ? (
        <>
          <div className="row mb-3">
            <div className="col-md-12">
              <span>Quantidade de aulas: {dadosPlanoAula.qtdAulas}</span>
            </div>
          </div>
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
