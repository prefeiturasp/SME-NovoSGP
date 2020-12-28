import React, { useCallback, useState } from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, CampoData } from '~/componentes';
import Editor from '~/componentes/editor/editor';

import {
  setDadosBimestresPlanoAnual,
  setPlanoAnualEmEdicao,
} from '~/redux/modulos/anual/actions';

const DescricaoPlanejamento = React.memo(props => {
  const [dataRegistro, setDataRegistro] = useState('');

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
      // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
      const dados = { ...dadosBimestrePlanoAnual };
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
    // if (servicoSalvarPlanoAnual.campoInvalido(valorEditado)) {
    //   return true;
    // }
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
      <div className="col-3 p-0 pb-2">
        <CampoData
          name="dataRegistro"
          placeholder="Selecione"
          valor={dataRegistro}
          formatoData="DD/MM/YYYY"
          onChange={e => setDataRegistro(e)}
        />
      </div>
      <div className="pt-1">
        <Editor
          validarSeTemErro={validarSeTemErro}
          mensagemErro="Campo obrigatório"
          id={`bimestre-${bimestre}-editor`}
          inicial={obterDadosComponenteAtual()?.descricao}
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
              componenteCurricular?.possuiObjetivos) ||
            !periodoAberto
          }
        />
        {obterAuditoria()}
      </div>
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
