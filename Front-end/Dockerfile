FROM node:18-alpine as build
WORKDIR /app/src

# Instala o envsubst
RUN apk --no-cache add gettext

# Copia os arquivos necessários
COPY package*.json ./
RUN npm ci

COPY . ./

# Define o ARG e o ENV para a substituição do valor
ARG API_URL
ENV API_URL=${API_URL}

RUN sed -i "s|\${API_URL}|${API_URL}|g" src/environments/environment.ts
RUN sed -i "s|\${API_URL}|${API_URL}|g" src/environments/environment.prod.ts

# Construção do projeto Angular
RUN npm run build --prod

FROM node:18-alpine
WORKDIR /usr/app

# Copia os arquivos de build
COPY --from=build /app/src/dist/gerenciador-de-processos ./

CMD ["node", "server/server.mjs"]
EXPOSE 4000
