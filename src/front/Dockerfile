# Stage 1: Build
FROM node:24-alpine AS build
WORKDIR /app

COPY package.json package-lock.json* ./
RUN npm ci --legacy-peer-deps

COPY . .
RUN npm run build

# Stage 2: Run SSR server
FROM node:24-alpine AS runtime
WORKDIR /app

# Copy the built output (browser + server)
COPY --from=build /app/dist/mariage-edwige-henri ./dist/mariage-edwige-henri

# Only production node_modules needed at runtime (express, zone.js, etc.)
COPY --from=build /app/node_modules ./node_modules

ENV PORT=4000
EXPOSE 4000

# Start the SSR server
CMD ["node", "dist/mariage-edwige-henri/server/server.mjs"]
