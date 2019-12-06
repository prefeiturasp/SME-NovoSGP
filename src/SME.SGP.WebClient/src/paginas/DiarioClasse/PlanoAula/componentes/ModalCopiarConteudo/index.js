import React, { useState } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import {
  ModalConteudoHtml,
  Button,
  Colors,
  CampoData,
  Grid,
  Loader,
} from '~/componentes';
import TurmasDropDown from '~/componentes-sgp/TurmasDropDown';
import ListaCheckbox from './componentes/ListaCheckbox';

// Styles
import { Row } from './styles';

// Serviço
import PlanoAulaServico from '~/servicos/Paginas/PlanoAula';

function ModalCopiarConteudo({ show, disciplina, onClose, planoAula }) {
  const filtro = useSelector(store => store.usuario.turmaSelecionada);
  const carregando = useSelector(store => store.loader.loaderModal);
  const confirmado = useState(false);
  const [valoresCheckbox, setValoresCheckbox] = useState({
    objetivosAprendizagem: true,
    desenvolvimentoAula: false,
    recuperacaoContinua: false,
    licaoCasa: false,
  });
  const [turmas, setTurmas] = useState([]);

  const adicionarTurma = () => {
    setTurmas([
      ...turmas,
      {
        id: shortid.generate(),
        turmaId: '',
        data: '',
        temErro: false,
        mensagemErro: 'Data já possui conteúdo',
      },
    ]);
  };

  const onClickExcluir = item => {
    setTurmas(turmas.filter(x => x.id !== item.id));
  };

  const onChangeTurma = (turma, linha) => {
    setTurmas(
      turmas.map(x =>
        x.id === linha.id
          ? {
              ...linha,
              turmaId: turma,
            }
          : x
      )
    );
  };

  const onChangeData = async (dataSelecionada, linha) => {
    setTurmas(
      turmas.map(x =>
        x.id === linha.id
          ? {
              ...linha,
              data: dataSelecionada,
            }
          : x
      )
    );
  };

  const onChangeCheckbox = (evento, campo) => {
    setValoresCheckbox({
      ...valoresCheckbox,
      [campo]: evento.target.checked,
    });
  };

  const onClickSalvar = async () => {
    try {
      // const { data, status } = await PlanoAulaServico.migrarPlano({
      //   idsPlanoTurmasDestino: turmas.map(x => ({
      //     ...x,
      //     sobreescrever: true,
      //   })),
      //   planoAulaId: planoAula.id,
      //   disciplinaId: disciplina,
      //   migrarLicaoCasa: valoresCheckbox.licaoCasa,
      //   migrarRecuperacaoAula: valoresCheckbox.recuperacaoContinua,
      //   migrarObjetivos: valoresCheckbox.objetivosAprendizagem,
      // });
      // if (data && status === 200) {
      //   console.log(data);
      // }
      const { data, status } = await PlanoAulaServico.verificarSeExiste({
        planoAulaTurmaDatas: turmas.map(x => ({
          data: x.data,
          turmaId: x.turmaId,
          disciplinaId: disciplina,
        })),
      });

      if (data && status === 200) {
        const temErro = data.filter(x => x.existe === true);
        if (temErro.length > 0) {
          temErro.forEach(erro => {
            setTurmas(
              turmas.map(x =>
                x.turmaId === String(erro.turmaId)
                  ? {
                      ...x,
                      temErro: true,
                      mensagemErro: 'Data já possui conteúdo',
                    }
                  : x
              )
            );
          });
        }
      }
    } catch (error) {}
  };

  return (
    <ModalConteudoHtml
      titulo="Copiar conteúdo"
      visivel={show}
      closable
      onClose={onClose}
      onConfirmacaoSecundaria={() => null}
      onConfirmacaoPrincipal={() => onClickSalvar()}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Descartar"
      perguntaAtencao="Os planos de aula de algumas turmas, já possuem conteúdo que será sobrescrito. Deseja continuar?"
      tituloAtencao="Atenção"
      desabilitarBotaoPrincipal={turmas.length < 1}
    >
      <Loader loading={carregando}>
        {turmas.map(linha => (
          <Row key={shortid.generate()} className="row">
            <Grid cols={6}>
              <TurmasDropDown
                ueId={filtro.unidadeEscolar}
                modalidadeId={filtro.modalidade}
                valor={linha.turmaId}
                onChange={turma => onChangeTurma(turma, linha)}
              />
            </Grid>
            <Grid cols={4}>
              <CampoData
                valor={linha.data}
                onChange={data => onChangeData(data, linha)}
                name="dataInicio"
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                temErro={linha.temErro}
                mensagemErro={linha.mensagemErro}
              />
            </Grid>
            <Grid cols={2}>
              <Button
                label="Excluir"
                color={Colors.Roxo}
                border
                className="btnGroupItem"
                onClick={() => onClickExcluir(linha)}
              />
            </Grid>
          </Row>
        ))}
        <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
          <Button
            label="Adicionar turma"
            color={Colors.Roxo}
            border
            className="btnGroupItem"
            onClick={adicionarTurma}
          />
        </div>
        <div style={{ marginTop: '26px' }}>
          <ListaCheckbox
            valores={valoresCheckbox}
            onChange={(evento, campo) => onChangeCheckbox(evento, campo)}
          />
        </div>
      </Loader>
    </ModalConteudoHtml>
  );
}

ModalCopiarConteudo.propTypes = {
  show: t.bool,
  disciplina: t.string,
  onClose: t.func,
  planoAula: t.oneOfType([t.object]),
};

ModalCopiarConteudo.defaultProps = {
  show: false,
  disciplina: null,
  onClose: null,
  planoAula: null,
};

export default ModalCopiarConteudo;
