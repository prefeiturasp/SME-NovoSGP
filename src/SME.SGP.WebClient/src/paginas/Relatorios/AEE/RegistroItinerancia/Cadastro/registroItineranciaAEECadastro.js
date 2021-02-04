import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import { Base, Button, CampoData, Card, Colors, Loader } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import { RotasDto } from '~/dtos';
import { setBreadcrumbManual } from '~/servicos';
import {
  CollapseAluno,
  EditoresTexto,
  ModalAlunos,
  ModalObjetivos,
  ModalUE,
  TabelaLinhaRemovivel,
} from './componentes';

const RegistroItineranciaAEECadastro = ({ match }) => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [dataVisita, setDataVisita] = useState();
  const [dataRetorno, setDataRetorno] = useState();
  const [modalVisivelUES, setModalVisivelUES] = useState(false);
  const [modalVisivelObjetivos, setModalVisivelObjetivos] = useState(false);
  const [modalVisivelAlunos, setModalVisivelAlunos] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState();
  const [alunosSelecionados, setAlunosSelecionados] = useState();
  const [unEscolaresSelecionados, setUnEscolaresSelecionados] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [apenasUmaUe, setApenasUmaUe] = useState(false);
  const [variasUesSelecionadas, setVariasUesSelecionadas] = useState(false);

  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickSalvar = () => {};

  const mudarDataVisita = data => {
    setDataVisita(data);
  };

  const mudarDataRetorno = data => {
    setDataRetorno(data);
  };

  const removerItemSelecionado = (text, funcao) => {
    funcao(estadoAntigo => estadoAntigo.filter(item => item.key !== text.key));
  };

  useEffect(() => {
    if (match?.url)
      setBreadcrumbManual(
        match?.url,
        'Alterar',
        RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA
      );
  }, [match]);

  const removerAlunos = alunoCodigo => {
    setAlunosSelecionados(estadoAntigo =>
      estadoAntigo.filter(item => item.alunoCodigo !== alunoCodigo)
    );
  };

  const desabilitarData = dataCorrente => {
    return (
      dataCorrente > window.moment() ||
      dataCorrente < window.moment().startOf('year')
    );
  };

  useEffect(() => {
    if (dataVisita && objetivosSelecionados?.length) {
      setDesabilitarCampos(true);
    }
  }, [dataVisita, objetivosSelecionados]);

  useEffect(() => {
    if (objetivosSelecionados?.length) {
      let umaUe = false;
      objetivosSelecionados.map(objetivo => {
        if (!objetivo.permiteVariasUes) {
          umaUe = true;
        }
      });
      setApenasUmaUe(umaUe);
    }
    if (!objetivosSelecionados?.length) {
      setApenasUmaUe(false);
    }
  }, [objetivosSelecionados]);

  useEffect(() => {
    if (unEscolaresSelecionados?.length) {
      let variasUes = false;
      unEscolaresSelecionados.map(objetivo => {
        if (!objetivo.permiteVariasUes) {
          variasUes = true;
        }
      });
      setVariasUesSelecionadas(variasUes);
    }
    if (!unEscolaresSelecionados?.length) {
      setVariasUesSelecionadas(false);
    }
  }, [unEscolaresSelecionados]);

  return (
    <>
      <Loader loading={carregandoGeral} className="w-100">
        <Cabecalho pagina="Registro de itinerância" />
        <Card>
          <div className="col-12 p-0">
            <div className="row mb-5">
              <div className="col-md-12 d-flex justify-content-end">
                <Button
                  id="btn-voltar-ata-diario-bordo"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-3"
                  onClick={onClickVoltar}
                />
                <Button
                  id="btn-cancelar-ata-diario-bordo"
                  label="Cancelar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-3"
                  onClick={onClickCancelar}
                  disabled={!desabilitarCampos}
                />
                <Button
                  id="btn-gerar-ata-diario-bordo"
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  onClick={onClickSalvar}
                  disabled={!desabilitarCampos}
                />
              </div>
            </div>
            <div className="row mb-4">
              <div className="col-3">
                <CampoData
                  name="dataVisita"
                  formatoData="DD/MM/YYYY"
                  valor={dataVisita}
                  label="Data da visita"
                  placeholder="Selecione a data"
                  onChange={mudarDataVisita}
                  desabilitarData={desabilitarData}
                />
              </div>
            </div>
            <div className="row mb-4">
              <TabelaLinhaRemovivel
                bordered
                ordenacao
                dataIndex="nome"
                labelTabela="Objetivos da itinerância"
                tituloTabela="Objetivos selecionados"
                labelBotao="Novo objetivo"
                pagination={false}
                dadosTabela={objetivosSelecionados}
                removerUsuario={text =>
                  removerItemSelecionado(text, setObjetivosSelecionados)
                }
                botaoAdicionar={() => setModalVisivelObjetivos(true)}
              />
            </div>
            <div className="row mb-4">
              <TabelaLinhaRemovivel
                bordered
                dataIndex="unidadeEscolar"
                labelTabela="Selecione as Unidades Escolares"
                tituloTabela="Unidades Escolares selecionadas"
                labelBotao="Adicionar nova unidade escolar"
                pagination={false}
                dadosTabela={unEscolaresSelecionados}
                removerUsuario={text =>
                  removerItemSelecionado(text, setUnEscolaresSelecionados)
                }
                botaoAdicionar={() => setModalVisivelUES(true)}
              />
            </div>
            {unEscolaresSelecionados?.length === 1 && (
              <div className="row mb-4">
                <div className="col-12 font-weight-bold mb-2">
                  <span style={{ color: Base.CinzaMako }}>Estudantes</span>
                </div>
                <div className="col-12">
                  <Button
                    id={shortid.generate()}
                    label="Adicionar novo estudante"
                    color={Colors.Azul}
                    border
                    className="mr-2"
                    onClick={() => setModalVisivelAlunos(true)}
                    icon="user-plus"
                  />
                </div>
              </div>
            )}
            {alunosSelecionados?.length ? (
              alunosSelecionados.map(aluno => (
                <CollapseAluno
                  key={aluno.alunoCodigo}
                  aluno={aluno}
                  removerAlunos={() => removerAlunos(aluno.alunoCodigo)}
                />
              ))
            ) : (
              <EditoresTexto />
            )}
            <div className="row mb-4">
              <div className="col-3">
                <CampoData
                  name="dataRetorno"
                  formatoData="DD/MM/YYYY"
                  valor={dataRetorno}
                  label="Data para retorno/verificação"
                  placeholder="Selecione a data"
                  onChange={mudarDataRetorno}
                />
              </div>
            </div>
          </div>
        </Card>
      </Loader>
      {modalVisivelUES && (
        <ModalUE
          modalVisivel={modalVisivelUES}
          setModalVisivel={setModalVisivelUES}
          unEscolaresSelecionados={unEscolaresSelecionados}
          setUnEscolaresSelecionados={setUnEscolaresSelecionados}
          permiteApenasUmaUe={apenasUmaUe}
        />
      )}
      {modalVisivelObjetivos && (
        <ModalObjetivos
          modalVisivel={modalVisivelObjetivos}
          setModalVisivel={setModalVisivelObjetivos}
          objetivosSelecionados={objetivosSelecionados}
          setObjetivosSelecionados={setObjetivosSelecionados}
          variasUesSelecionadas={variasUesSelecionadas}
        />
      )}
      {modalVisivelAlunos && (
        <ModalAlunos
          modalVisivel={modalVisivelAlunos}
          setModalVisivel={setModalVisivelAlunos}
          alunosSelecionados={alunosSelecionados}
          setAlunosSelecionados={setAlunosSelecionados}
          codigoUe={
            unEscolaresSelecionados.length &&
            unEscolaresSelecionados[0].codigoUe
          }
        />
      )}
    </>
  );
};

RegistroItineranciaAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

RegistroItineranciaAEECadastro.defaultProps = {
  match: {},
};

export default RegistroItineranciaAEECadastro;
