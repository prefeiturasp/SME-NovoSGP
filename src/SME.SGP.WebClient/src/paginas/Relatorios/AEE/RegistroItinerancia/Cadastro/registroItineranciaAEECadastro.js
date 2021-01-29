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
  TabelaLinhaRemovivel,
} from './componentes';

const RegistroItineranciaAEECadastro = ({ match }) => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [dataVisita, setDataVisita] = useState();
  const [dataRetorno, setDataRetorno] = useState();
  const [objetivosSelecionados, setObjetivosSelecionados] = useState([
    {
      key: '1',
      objetivosSelecionados:
        'Mapeamento dos estudantes público da educação especial',
    },
    {
      key: '2',
      objetivosSelecionados: 'Reunião',
    },
  ]);
  const [unEscolaresSelecionados, setuUnEscolaresSelecionados] = useState([
    {
      key: '1',
      unidadeEscolares: 'CEU EMEF CESAR ARRUMADA CANTANHO, DEP.',
    },
    {
      key: '2',
      unidadeEscolares: 'CEU EMEF CESAR ARRUMADA CANTANHO, DEP. 2',
    },
  ]);

  const idRegistroItinerancia = match?.params?.id;

  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickSalvar = () => {};

  const mudarDataVisita = data => {
    setDataVisita(data);
  };

  const mudarDataRetorno = data => {
    setDataRetorno(data);
  };

  const removerUsuario = (text, funcao) => {
    funcao(estadoAntigo => estadoAntigo.filter(item => item.key !== text.key));
  };

  useEffect(() => {
    if (idRegistroItinerancia)
      setBreadcrumbManual(
        match?.url,
        'Alterar',
        RotasDto.RELATORIO_AEE_REGISTRO_ITINERANCIA
      );
  }, [match, idRegistroItinerancia]);

  return (
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
                // disabled={!modoEdicao || desabilitarCampos}
              />
              <Button
                id="btn-gerar-ata-diario-bordo"
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                onClick={onClickSalvar}
                // disabled={!modoEdicao || !turmaInfantil || desabilitarCampos}
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
              />
            </div>
          </div>
          <div className="row mb-4">
            <TabelaLinhaRemovivel
              bordered
              dataIndex="objetivosSelecionados"
              labelTabela="Objetivos da itinerância"
              tituloTabela="Objetivos selecionados"
              labelBotao="Novo objetivo"
              pagination={false}
              dadosTabela={objetivosSelecionados}
              removerUsuario={text =>
                removerUsuario(text, setObjetivosSelecionados)
              }
            />
          </div>
          <div className="row mb-4">
            <TabelaLinhaRemovivel
              bordered
              dataIndex="unidadeEscolares"
              labelTabela="Selecione as Unidades Escolares"
              tituloTabela="Unidades Escolares selecionadas"
              labelBotao="Adicionar nova unidade escolar"
              pagination={false}
              dadosTabela={unEscolaresSelecionados}
              removerUsuario={text =>
                removerUsuario(text, setuUnEscolaresSelecionados)
              }
            />
          </div>
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
                // onClick={() => onClickEditarCriancas()}
                icon="user-plus"
              />
            </div>
          </div>
          {!idRegistroItinerancia ? <CollapseAluno /> : <EditoresTexto />}
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
  );
};

RegistroItineranciaAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

RegistroItineranciaAEECadastro.defaultProps = {
  match: {},
};

export default RegistroItineranciaAEECadastro;
