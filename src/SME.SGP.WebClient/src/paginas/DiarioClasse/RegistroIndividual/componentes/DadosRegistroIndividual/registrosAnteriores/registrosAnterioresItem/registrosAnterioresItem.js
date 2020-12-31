import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Auditoria, CampoData, Label } from '~/componentes';
import Editor from '~/componentes/editor/editor';

import { ServicoRegistroIndividual } from '~/servicos';

import { setDadosPrincipaisRegistroIndividual } from '~/redux/modulos/registroIndividual/actions';

const RegistrosAnterioresItem = React.memo(() => {
  const [dataInicio, setDataInicio] = useState();
  const [dataFim, setDataFim] = useState();

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const dadosPrincipaisRegistroIndividual = useSelector(
    store => store.registroIndividual.dadosPrincipaisRegistroIndividual
  );
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const { turma } = turmaSelecionada;
  const turmaCodigo = turma || 0;

  const auditoria =
    dadosPrincipaisRegistroIndividual?.registroIndividual?.auditoria;
  const dispatch = useDispatch();

  const obterRegistroIndividualPorData = useCallback(async () => {
    const dataFormatadaInicio = dataInicio?.format('MM-DD-YYYY');
    const dataFormatadaFim = dataFim?.format('MM-DD-YYYY');

    if (dataFormatadaInicio && dataFormatadaFim) {
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorPeriodo(
        {
          alunoCodigo: dadosAlunoObjectCard.codigoEOL,
          componenteCurricular: componenteCurricularSelecionado,
          dataInicio: dataFormatadaInicio,
          dataFim: dataFormatadaFim,
          turmaCodigo,
        }
      );
      if (retorno?.data) {
        dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
      }
    }
  }, [
    dispatch,
    componenteCurricularSelecionado,
    dadosAlunoObjectCard,
    dataInicio,
    dataFim,
    turmaCodigo,
  ]);

  useEffect(() => {
    if (Object.keys(dadosAlunoObjectCard).length) {
      obterRegistroIndividualPorData();
    }
  }, [obterRegistroIndividualPorData, dadosAlunoObjectCard]);

  const onChange = useCallback(valorNovo => {
    // TODO Verificar para salvar dados editados no redux separada do atual para melhorar a performance!
    // const dados = { ...dadosBimestrePlanoAnual };
    // dados.componentes.forEach(item => {
    //   if (
    //     String(item.componenteCurricularId) ===
    //     String(tabAtualComponenteCurricular.codigoComponenteCurricular)
    //   ) {
    //     item.descricao = valorNovo;
    //     item.emEdicao = true;
    //   }
    // });
    // dispatch(setDadosBimestresPlanoAnual(dados));
  }, []);

  const validarSeTemErro = valorEditado => {
    // if (servicoSalvarPlanoAnual.campoInvalido(valorEditado)) {
    //   return true;
    // }
    return false;
  };

  useEffect(() => {
    if (!dataInicio) {
      setDataInicio(window.moment().subtract(60, 'd'));
    }
  }, [dataInicio]);

  useEffect(() => {
    if (!dataFim) {
      setDataFim(window.moment());
    }
  }, [dataFim]);

  return (
    <>
      <div className="row px-3">
        <div className="col-12 pl-0">
          <Label text="Visualizar registros no período" />
        </div>
        <div className="col-3 p-0 pb-2 pr-3">
          <CampoData
            formatoData="DD/MM/YYYY"
            name="dataInicio"
            valor={dataInicio}
            onChange={data => setDataInicio(data)}
            placeholder="Data início"
          />
        </div>
        <div className="col-3 p-0 pb-2 mb-4">
          <CampoData
            formatoData="DD/MM/YYYY"
            name="dataFim"
            valor={dataFim}
            onChange={data => setDataFim(data)}
            placeholder="Data fim"
          />
        </div>
        <div className="p-0 col-12">
          <Editor
            validarSeTemErro={validarSeTemErro}
            mensagemErro="Campo obrigatório"
            id="editor"
            inicial={
              dadosPrincipaisRegistroIndividual?.registroIndividual || ''
            }
            onChange={v => {
              // if (
              //   !planoAnualSomenteConsulta &&
              //   periodoAberto
              // ) {
              //   dispatch(setPlanoAnualEmEdicao(true));
              //   onChange(v);
              // }
            }}
          />
        </div>
      </div>
      {auditoria && (
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
      )}
    </>
  );
});

export default RegistrosAnterioresItem;
