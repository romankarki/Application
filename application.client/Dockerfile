# Stage 1: Build the Angular application
FROM node:latest as build

WORKDIR /usr/local/app

COPY ./ /usr/local/app
RUN npm install
RUN npm run build --prod

# Stage 2: Serve the application with Nginx
FROM nginx:latest

COPY --from=build /usr/local/app/dist/application.client/browser /usr/share/nginx/html

EXPOSE 80