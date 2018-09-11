
.PHONY: docker-build
docker-build:
	dotnet publish ./Cogslite.csproj -c Release -o ./out
	docker build -t cogslite .

.PHONY: docker-run
docker-run:
	docker run -d -p 5000:80 --rm -e AWS_ACCESS_KEY_ID=AKIAICA3YK5TIK7O5JXA -e AWS_SECRET_ACCESS_KEY=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 --name cogslite cogslite

.PHONY docker-push
docker-push:
	dotnet publish ./Cogslite.csproj -c Release -o ./out
	docker build -t cogslite .
	@$(shell aws ecr get-login --no-include-email --profile $(profile) --region=eu-west-2)	
	docker push https://848502523218.dkr.ecr.eu-west-2.amazonaws.com/cogslite:$(tag)