# src/ClientApp/Dockerfile.dev
FROM node:20
WORKDIR /app

# Copy package.json and install dependencies
COPY package*.json ./
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm install

# Mount source code (do not copy; mount in docker-compose)
# COPY . .

# Expose dev server port
EXPOSE 5173

# Start Vite dev server, bind to 0.0.0.0 so host can access
CMD ["npm", "run", "dev", "--", "--host"]