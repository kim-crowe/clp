.PHONY: publish
publish:
	dotnet publish ./Cogslite/Cogslite.csproj -c Release -o ./out
	cd Cogslite && docker build -t cogslite .