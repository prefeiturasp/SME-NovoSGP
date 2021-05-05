import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';

import {
  Button,
  CampoData,
  Colors,
  Label,
  ModalConteudoHtml,
} from '~/componentes';

import {
  setExibirLoaderGeralRegistroIndividual,
  setExibirModalImpressaoRegistroIndividual,
} from '~/redux/modulos/registroIndividual/actions';

import { erros, sucesso, ServicoRegistroIndividual } from '~/servicos';

const ModalImpressaoRegistroIndividual = () => {
  const [dataInicio, setDataInicio] = useState();
  const [dataFim, setDataFim] = useState(window.moment());

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);
  const { anoLetivo, consideraHistorico } = turmaSelecionada;

  const dispatch = useDispatch();

  const exibirModalImpressaoRegistroIndividual = useSelector(
    store => store.registroIndividual.exibirModalImpressaoRegistroIndividual
  );

  const onCloseModal = () => {
    dispatch(setExibirModalImpressaoRegistroIndividual(false));
  };

  const gerar = async () => {
    dispatch(setExibirLoaderGeralRegistroIndividual(true));
    onCloseModal();
    await ServicoRegistroIndividual.gerar({
      dataInicio,
      dataFim,
      turmaId: turmaSelecionada.id,
    })
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e))
      .finally(dispatch(setExibirLoaderGeralRegistroIndividual(false)));
  };

  const desabilitarData = dataCorrente => {
    const dataLimiteInferior = window.moment(`${anoLetivo}-01-01`);
    const dataLimiteSuperior = consideraHistorico
      ? window.moment(`${anoLetivo}-12-31`)
      : window.moment();

    return (
      dataCorrente < dataLimiteInferior || dataCorrente > dataLimiteSuperior
    );
  };

  const escolherData = useCallback(() => {
    const anoAtual = dataFim?.format('YYYY');
    const diferencaDias = dataFim?.diff(`${anoAtual}-01-01`, 'days');
    let dataInicioSelecionada = window.moment(`${anoAtual}-01-01`);

    if (Number(diferencaDias) > 60) {
      dataInicioSelecionada = window.moment().subtract(60, 'd');
    }

    if (Number(anoLetivo) !== Number(anoAtual)) {
      dataInicioSelecionada = window.moment(`${anoLetivo}-01-01`);
      setDataFim(window.moment(`${anoLetivo}-12-31`));
    }

    setDataInicio(dataInicioSelecionada);
  }, [dataFim, anoLetivo]);

  useEffect(() => {
    if (!consideraHistorico) {
      setDataFim(window.moment());
    }
    setDataInicio('');
  }, [consideraHistorico]);

  useEffect(() => {
    if (!dataInicio && dataFim) {
      escolherData();
    }
  }, [dataInicio, dataFim, escolherData]);

  const mudarDataInicio = data => {
    const dataEscolhida = data > dataFim ? dataFim : data;
    setDataInicio(dataEscolhida);
  };

  const mudarDataFim = data => {
    const dataEscolhida = data < dataInicio ? dataInicio : data;
    setDataFim(dataEscolhida);
  };

  return (
    <>
      {exibirModalImpressaoRegistroIndividual && (
        <ModalConteudoHtml
          id={shortid.generate()}
          key="impressao-relatorio-registro-individual"
          visivel={exibirModalImpressaoRegistroIndividual}
          titulo="Relatório de registro individual"
          onClose={onCloseModal}
          esconderBotaoPrincipal
          esconderBotaoSecundario
          closable
          paddingBottom
        >
          <Label text="Período a ser impresso" />
          <div className="row px-3 mb-4">
            <div className="col-6 p-0 pb-2 pr-3">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataInicio"
                valor={dataInicio}
                onChange={mudarDataInicio}
                placeholder="Data início"
                desabilitarData={desabilitarData}
              />
            </div>
            <div className="col-6 p-0 pb-2">
              <CampoData
                formatoData="DD/MM/YYYY"
                name="dataFim"
                valor={dataFim}
                onChange={mudarDataFim}
                placeholder="Data fim"
                desabilitarData={desabilitarData}
              />
            </div>
          </div>
          <div className="row mr-n2">
            <div className="col-md-12 d-flex justify-content-end p-0 mt-2">
              <Button
                key="btn-voltar"
                id="btn-voltar"
                label="Cancelar"
                color={Colors.Azul}
                border
                onClick={onCloseModal}
                className="mr-3 padding-btn-confirmacao"
              />
              <Button
                id="btn-gerar-rel"
                key="btn-gerar-rel"
                label="Gerar relatório"
                color={Colors.Roxo}
                bold
                className="padding-btn-confirmacao"
                onClick={gerar}
              />
            </div>
          </div>
        </ModalConteudoHtml>
      )}
    </>
  );
};

export default ModalImpressaoRegistroIndividual;
