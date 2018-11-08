.PHONY: publish
publish:
	dotnet publish ./Cogslite/Cogslite.csproj -c Release -o ./out
	cd Cogslite && docker build -t cogslite .

## Database

DOCKER-COMPOSE := docker-compose -f ./docker-compose.yml

db-up:  ## Start and migrate the local Postgres DB	
	$(DOCKER-COMPOSE) up -d --force-recreate db flyway

.PHONY: db-drop
db-drop:  ## Drop all migrations in the local Postgres DB
	$(DOCKER-COMPOSE) run --rm flyway clean

.PHONY: db-down
db-down:  ## Stop the local Postgres DB
	$(DOCKER-COMPOSE) rm -fs db

run-web:
	dotnet run --project ./Cogslite/Cogslite.csproj

.PHONY: tear-down
tear-down:  ## Forcibly destroy all running containers, including volumes and networks
	$(DOCKER-COMPOSE) rm -sfv
	docker volume prune -f	