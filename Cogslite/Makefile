## Database

DOCKER-COMPOSE := docker-compose -f ./docker-compose.yml

.PHONY: build-image
build-image:
	dotnet publish ./Cogslite.Api/Cogslite.Api.csproj -c Release -o ./out
	cd Cogslite.Api && docker build -t cogs-api .

.PHONY: run
run:	
	$(DOCKER-COMPOSE) up -d --force-recreate db api

.PHONY: db
db:
	$(DOCKER-COMPOSE) up -d db

.PHONY: create-stack
create-stack:
	aws cloudformation create-stack --stack-name ccgworks-api --template-body file://Infrastructure/aws-ecr.yml --capabilities CAPABILITY_NAMED_IAM

.PHONY: update-stack
update-stack:
	aws cloudformation update-stack --stack-name ccgworks-api --template-body file://Infrastructure/aws-ecr.yml --capabilities CAPABILITY_NAMED_IAM

.PHONY: tear-down
tear-down:  ## Forcibly destroy all running containers, including volumes and networks
	$(DOCKER-COMPOSE) rm -sfv
	docker volume prune -f	