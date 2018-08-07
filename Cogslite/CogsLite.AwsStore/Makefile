.PHONY: create-schema
create-schema:
	aws cloudformation create-stack \
		--stack-name CogsDb \
		--template-body file://./DatabaseSchema.yml \
		--capabilities CAPABILITY_NAMED_IAM \
		--profile $(profile) \
		--region $(region)