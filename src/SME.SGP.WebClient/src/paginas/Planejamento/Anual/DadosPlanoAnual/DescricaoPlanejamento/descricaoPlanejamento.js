import PropTypes from 'prop-types';
import React, { useCallback } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria, Label } from '~/componentes';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setDadosBimestresPlanoAnual,
  setPlanoAnualEmEdicao,
} from '~/redux/modulos/anual/actions';
import { DescItensAutoraisProfessor } from '../../planoAnual.css';
import servicoSalvarPlanoAnual from '../../servicoSalvarPlanoAnual';

const DescricaoPlanejamento = React.memo(props => {
  const { dadosBimestre, tabAtualComponenteCurricular } = props;
  const { bimestre, periodoAberto } = dadosBimestre;

  const dispatch = useDispatch();

  const dadosBimestrePlanoAnual = useSelector(
    store => store.planoAnual.dadosBimestresPlanoAnual[bimestre]
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  const planoAnualSomenteConsulta = useSelector(
    store => store.planoAnual.planoAnualSomenteConsulta
  );

  const onChange = useCallback(
    valorNovo => {
      const dados = dadosBimestrePlanoAnual;
      dados.componentes.forEach(item => {
        if (
          String(item.componenteCurricularId) ===
          String(tabAtualComponenteCurricular.codigoComponenteCurricular)
        ) {
          item.descricao = valorNovo;
          item.emEdicao = true;
        }
      });
      dispatch(setDadosBimestresPlanoAnual(dados));
    },
    [dispatch, dadosBimestrePlanoAnual, tabAtualComponenteCurricular]
  );

  const obterDadosComponenteAtual = () => {
    return dadosBimestrePlanoAnual?.componentes.find(
      item =>
        String(item.componenteCurricularId) ===
        String(tabAtualComponenteCurricular.codigoComponenteCurricular)
    );
  };

  const validarSeTemErro = valorEditado => {
    if (servicoSalvarPlanoAnual.campoInvalido(valorEditado)) {
      return true;
    }
    return false;
  };

  const obterAuditoria = () => {
    const auditoria = obterDadosComponenteAtual()?.auditoria;
    if (auditoria) {
      return (
        <div className="row">
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRF}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRF}
          />
        </div>
      );
    }
    return '';
  };

  return (
    <>
      {dadosBimestrePlanoAnual &&
      dadosBimestrePlanoAnual?.componentes.length &&
      obterDadosComponenteAtual() ? (
        <div className="mt-3">
          <span className="d-flex align-items-baseline">
            <Label text="Descrição do planejamento" />
            <DescItensAutoraisProfessor>
              Itens autorais do professor
            </DescItensAutoraisProfessor>
          </span>
          <JoditEditor
            validarSeTemErro={validarSeTemErro}
            mensagemErro="Campo obrigatório"
            id={`bimestre-${bimestre}-editor`}
            value={obterDadosComponenteAtual()?.descricao}
            onChange={v => {
              if (
                !planoAnualSomenteConsulta &&
                periodoAberto &&
                obterDadosComponenteAtual()?.descricao !== v
              ) {
                dispatch(setPlanoAnualEmEdicao(true));
                onChange(v);
              }
            }}
            desabilitar={
              planoAnualSomenteConsulta ||
              (!obterDadosComponenteAtual()?.objetivosAprendizagemId?.length &&
                componenteCurricular.possuiObjetivos) ||
              !periodoAberto
            }
          />
          {obterAuditoria()}
        </div>
      ) : (
        ''
      )}
    </>
  );
});

DescricaoPlanejamento.propTypes = {
  dadosBimestre: PropTypes.oneOfType([PropTypes.object]),
  tabAtualComponenteCurricular: PropTypes.oneOfType([PropTypes.object]),
};

DescricaoPlanejamento.defaultProps = {
  dadosBimestre: {},
  tabAtualComponenteCurricular: {},
};

export default DescricaoPlanejamento;
