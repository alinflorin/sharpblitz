FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine AS builder
WORKDIR /
RUN apk add --update nodejs npm
COPY SharpBlitz.sln ./
COPY SharpBlitz/*.csproj ./SharpBlitz/
COPY SharpBlitz.Compiler.Abstractions/*.csproj ./SharpBlitz.Compiler.Abstractions/
COPY SharpBlitz.Compiler.Roslyn/*.csproj ./SharpBlitz.Compiler.Roslyn/
COPY SharpBlitz.Runner.Abstractions/*.csproj ./SharpBlitz.Runner.Abstractions/
COPY SharpBlitz.Runner.Roslyn/*.csproj ./SharpBlitz.Runner.Roslyn/
COPY SharpBlitz.CodeAnalysis.Abstractions/*.csproj ./SharpBlitz.CodeAnalysis.Abstractions/
COPY SharpBlitz.CodeAnalysis.Roslyn/*.csproj ./SharpBlitz.CodeAnalysis.Roslyn/
COPY SharpBlitz.Common/*.csproj ./SharpBlitz.Common/
RUN dotnet restore
COPY SharpBlitz-UI/package-lock.json ./SharpBlitz-UI/
COPY SharpBlitz-UI/package.json ./SharpBlitz-UI/
WORKDIR /SharpBlitz-UI
RUN npm install
WORKDIR /
COPY . .
WORKDIR /SharpBlitz
RUN dotnet build --configuration Release --no-restore
WORKDIR /SharpBlitz-UI
RUN npm run version
RUN npm run ng build -- --prod

FROM duluca/minimal-node-web-server AS runner
WORKDIR /usr/src/app
COPY --from=builder /dist public