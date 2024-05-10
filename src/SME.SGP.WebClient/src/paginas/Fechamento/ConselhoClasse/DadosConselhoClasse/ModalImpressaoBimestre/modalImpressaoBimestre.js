import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { Colors, ModalConteudoHtml } from '~/componentes';
import Button from '~/componentes/button';
import SelectComponent from '~/componentes/select';
import {
  setExibirLoaderGeralConselhoClasse,
  setExibirModalImpressaoConselhoClasse,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros, sucesso } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const ModalImpressaoBimestre = () => {
  const dispatch = useDispatch();

  const exibirModalImpressaoConselhoClasse = useSelector(
    store => store.conselhoClasse.exibirModalImpressaoConselhoClasse
  );

  const dadosBimestresConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosBimestresConselhoClasse
  );

  const [bimestreSelecionado, setBimestreSelecionado] = useState(undefined);
  const [listaBimestres, setListaBimestres] = useState([]);

  const onCloseModal = () => {
    setBimestreSelecionado(undefined);
    setListaBimestres([]);
    dispatch(setExibirModalImpressaoConselhoClasse(false));
  };

  useEffect(() => {
    if (
      exibirModalImpressaoConselhoClasse &&
      dadosBimestresConselhoClasse &&
      dadosBimestresConselhoClasse.length
    ) {
      const bimestres = dadosBimestresConselhoClasse.map(item => {
        const obj = {
          valor: String(item.bimestre),
          descricao:
            item.bimestre === 0 ? 'Final' : `${item.bimestre}º Bimestre`,
        };
        return obj;
      });
      if (bimestres && bimestres.length === 1) {
        setBimestreSelecionado(String(bimestres[0].valor));
      }
      setListaBimestres(bimestres);
    }
  }, [dadosBimestresConselhoClasse, exibirModalImpressaoConselhoClasse]);

  const gerarConselhoClasseTurma = async () => {
    const bimestre = dadosBimestresConselhoClasse.find(
      item => String(item.bimestre) === String(bimestreSelecionado)
    );
    const { conselhoClasseId, fechamentoTurmaId } = bimestre;

    dispatch(setExibirLoaderGeralConselhoClasse(true));
    onCloseModal();
    await ServicoConselhoClasse.gerarConselhoClasseTurma(
      conselhoClasseId || 0,
      fechamentoTurmaId || 0
    )
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e))
      .finally(dispatch(setExibirLoaderGeralConselhoClasse(false)));
  };

  return (
    <>
      {dadosBimestresConselhoClasse ? (
        <ModalConteudoHtml
          id={shortid.generate()}
          key="impressao-relatorio-conselho-classe"
          visivel={exibirModalImpressaoConselhoClasse}
          titulo="Relatório"
          onClose={onCloseModal}
          esconderBotaoPrincipal
          esconderBotaoSecundario
          closable
        >
          <div className="col-md-12 mt-2">
            <SelectComponent
              label="Selecione o bimestre em que deseja realizar a impressão do conselho
            de classe de todos os estudantes"
              id="bimestres-conselho-classe"
              lista={listaBimestres}
              valueOption="valor"
              valueText="descricao"
              placeholder="Selecione um bimestre"
              disabled={listaBimestres && listaBimestres.length === 1}
              valueSelect={bimestreSelecionado}
              onChange={setBimestreSelecionado}
            />
          </div>

          <div className="row">
            <div className="col-md-12 d-flex justify-content-end">
              <Button
                key="btn-voltar"
                id="btn-voltar"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                onClick={onCloseModal}
                className="mr-3 mt-2 padding-btn-confirmacao"
              />
              <Button
                id="btn-gerar-rel"
                key="btn-gerar-rel"
                label="Gerar"
                color={Colors.Roxo}
                bold
                border
                className="mr-3 mt-2 padding-btn-confirmacao"
                onClick={gerarConselhoClasseTurma}
                disabled={!bimestreSelecionado}
              />
            </div>
          </div>
        </ModalConteudoHtml>
      ) : (
        ''
      )}
    </>
  );
};

export default ModalImpressaoBimestre;
