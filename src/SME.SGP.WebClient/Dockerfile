FROM node:carbon-alpine as build-deps
WORKDIR /usr/src/app
COPY src/SME.SGP.WebClient/package.json src/SME.SGP.WebClient/yarn.lock ./
RUN yarn
COPY /src/SME.SGP.WebClient/ .
RUN yarn build

FROM nginx:1.17-alpine
COPY src/SME.SGP.WebClient/configuracoes/default.conf /etc/nginx/conf.d/
COPY --from=build-deps /usr/src/app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
