import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria, Colors } from '~/componentes';
import { RegistroMigrado } from '~/componentes-sgp';
import Button from '~/componentes/button';
import { RotasDto } from '~/dtos';
import {
  setDesabilitarCamposPlanoAula,
  setExibirModalCopiarConteudoPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import BotaoGerarRelatorioPlanoAula from './BotaoGerarRelatorioPlanoAula/botaoGerarRelatorioPlanoAula';
import DesenvolvimentoDaAula from './CamposEditorPlanoAula/desenvolvimentoDaAula';
import LicaoDeCasa from './CamposEditorPlanoAula/licaoDeCasa';
import ObjetivosEspecificosParaAula from './CamposEditorPlanoAula/objetivosEspecificosParaAula';
import RecuperacaoContinua from './CamposEditorPlanoAula/recuperacaoContinua';
import ModalCopiarConteudoPlanoAula from './ModalCopiarConteudo/modalCopiarConteudoPlanoAula';
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
          <ModalCopiarConteudoPlanoAula />

          <div className="row mb-3">
            <div className="col-md-3">
              <span>Quantidade de aulas: {dadosPlanoAula.qtdAulas}</span>
            </div>
            <div className="col-md-9 d-flex justify-content-end ">
              <Button
                id="copiar-conteudo-plano-aula"
                label="Copiar Conteúdo"
                icon="clipboard"
                color={Colors.Azul}
                border
                className="mr-3"
                onClick={() =>
                  dispatch(setExibirModalCopiarConteudoPlanoAula(true))
                }
                disabled={!dadosPlanoAula.id}
              />
              <BotaoGerarRelatorioPlanoAula planoAulaId={dadosPlanoAula.id} />
              {dadosPlanoAula.migrado && (
                <RegistroMigrado className="align-self-center">
                  Registro Migrado
                </RegistroMigrado>
              )}
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
