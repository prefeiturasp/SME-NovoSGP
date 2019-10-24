import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const DadosPerfil = styled.div`
display: flex;
justify-content: center;
flex-direction: column;
align-items: center;

@media (max-width: 850px) {
  img{
    width: 100px !important;
    height: 100px !important;
  }
}

@media (max-width: 700px) {
  img{
    width: 70px !important;
    height: 70px !important;
  }
}

.icone-perfil{  
  color: ${Base.CinzaBotao};
  font-size: 215px;
}

.img-edit{
    width: 215px !important;
    height: 215px !important;
}

.img-profile{
    width: 172px;
    height: 172px;
}

.icon-profile{
  font-size: 172px;
  color: ${Base.CinzaBotao};
}

img{
  border-radius: 50%;
}

span{
    font-size: 14px;
}
.nome{
    font-size: 18px !important;
    font-weight: bold !important;
    padding: 20px 0 40px 0 !important;
}
`;

export const Perfil = styled.div`
  padding: 0 !important;
  margin: 0 !important;
  border-right: 1px solid ${Base.CinzaBarras};
`;

export const Botao = styled.a`
display: flex !important;
text-align: center !important;
position: absolute;
top:76%;
left:65%;
`;

export const Icone = styled.div`
background: ${Base.Roxo};
color: ${Base.Branco};
font-size: 17px !important;
height: 35px !important;
width: 35px !important;
vertical-align: middle;
box-sizing: border-box;
align-items: center !important;
border-radius: 50%;
display: inline-block;
justify-content: center !important;
i{
    margin-top:25%;
}

@media (max-width: 850px) {
  font-size: 10px !important;
  height: 20px !important;
  width:20px !important;
}
`;

export const Conteudo = styled.div``;

export const SelecionarFoto = styled.a`
      color: ${Base.Roxo} !important;
      font-size: 14px !important;
      margin-top: 19px;
  `;

export const Topo = styled.div`
      padding-bottom: 30px;
  `;

export const MensagemAlerta = styled.div`
  border: 2px solid ${Base.VermelhoAlerta};
  border-radius: 4px;
  font-size: 16px;
  color: ${Base.VermelhoAlerta};
  padding: 15px;
  width: 100%;

  .titulo{
    font-size: 24px;
    margin-bottom: 10px;
  }
`;

export const BarraProgresso = styled.div`
  font-size: 14px;
  color: ${Base.CinzaMako};
`;
