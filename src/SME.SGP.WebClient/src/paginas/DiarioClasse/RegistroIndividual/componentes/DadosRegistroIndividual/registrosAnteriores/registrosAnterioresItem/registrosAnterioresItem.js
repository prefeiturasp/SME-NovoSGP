import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Pagination } from 'antd';

import { Auditoria, CampoData, Label } from '~/componentes';
import Editor from '~/componentes/editor/editor';

import { erros, ServicoRegistroIndividual } from '~/servicos';

import {
  setDadosPrincipaisRegistroIndividual,
  setExibirLoaderGeralRegistroIndividual,
} from '~/redux/modulos/registroIndividual/actions';

const RegistrosAnterioresItem = React.memo(() => {
  const [dataInicio, setDataInicio] = useState();
  const [dataFim, setDataFim] = useState();

  const {
    componenteCurricularSelecionado,
    dadosPrincipaisRegistroIndividual,
    dadosAlunoObjectCard,
    exibirLoaderGeralRegistroIndividual,
  } = useSelector(store => store.registroIndividual);
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaCodigo = turmaSelecionada?.id || 0;

  const dispatch = useDispatch();

  const obterRegistroIndividualPorData = useCallback(
    async (dataFormatadaInicio, dataFormatadaFim, codigoEOL) => {
      dispatch(setExibirLoaderGeralRegistroIndividual(true));
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorPeriodo(
        {
          alunoCodigo: codigoEOL,
          componenteCurricular: componenteCurricularSelecionado,
          dataInicio: dataFormatadaInicio,
          dataFim: dataFormatadaFim,
          turmaCodigo,
        }
      )
        .catch(e => erros(e))
        .finally(() => dispatch(setExibirLoaderGeralRegistroIndividual(false)));

      if (retorno?.data) {
        dispatch(setDadosPrincipaisRegistroIndividual(retorno.data));
      }
    },
    [dispatch, componenteCurricularSelecionado, turmaCodigo]
  );

  useEffect(() => {
    const dataFormatadaInicio = dataInicio?.format('MM-DD-YYYY');
    const dataFormatadaFim = dataFim?.format('MM-DD-YYYY');
    const temDadosAlunos = Object.keys(dadosAlunoObjectCard).length;
    const { codigoEOL } = dadosAlunoObjectCard;
    const temDadosRegistros = Object.keys(dadosPrincipaisRegistroIndividual)
      .length;

    if (
      temDadosAlunos &&
      !temDadosRegistros &&
      !exibirLoaderGeralRegistroIndividual &&
      dataInicio &&
      dataFim
    ) {
      obterRegistroIndividualPorData(
        dataFormatadaInicio,
        dataFormatadaFim,
        codigoEOL
      );
    }
  }, [
    obterRegistroIndividualPorData,
    dadosAlunoObjectCard,
    dataInicio,
    dataFim,
    dadosPrincipaisRegistroIndividual,
    exibirLoaderGeralRegistroIndividual,
  ]);

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
    if (!dataInicio && dataFim) {
      const anoAtual = dataFim.format('YYYY');
      const anoLetivo = turmaSelecionada?.anoLetivo;
      const diferencaDias = dataFim.diff(`${anoAtual}-01-01`, 'days');

      if (Number(diferencaDias) > 60) {
        setDataInicio(window.moment().subtract(60, 'd'));
        return;
      }

      if (Number(anoLetivo) !== Number(anoAtual)) {
        setDataInicio(window.moment(`${anoLetivo}-01-01`));
        setDataFim(window.moment(`${anoLetivo}-12-31`));
        return;
      }

      setDataInicio(window.moment(`${anoAtual}-01-01`));
    }
  }, [dataInicio, dataFim, turmaSelecionada]);

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
        <Pagination
          showSizeChanger
          onShowSizeChange={() => {}}
          defaultCurrent={3}
          total={500}
        >
          {dadosPrincipaisRegistroIndividual?.registrosIndividuais?.map(
            ({ auditoria, data, registro }) => (
              <>
                <div className="p-0 col-12">
                  <Editor
                    validarSeTemErro={validarSeTemErro}
                    mensagemErro="Campo obrigatório"
                    label={`Registro - ${window
                      .moment(data)
                      .format('DD/MM/YYYY')}`}
                    id="editor"
                    inicial={registro}
                    onChange={onChange}
                  />
                </div>
                {auditoria && (
                  <div className="mt-1 ml-n3 mb-2">
                    <Auditoria
                      ignorarMarginTop
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
            )
          )}
        </Pagination>
      </div>
    </>
  );
});

export default RegistrosAnterioresItem;
