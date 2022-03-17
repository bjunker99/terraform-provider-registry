terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.5.0"
    }
    download = {
      version = "~> 0.0.1"
      source  = "terraform-registry.bjunker99.com/bjunker99/download"
    }
  }
}

provider "aws" {
  region = var.aws_region
}

data "download_file" "release" {
  url         = "https://github.com/bjunker99/terraform-provider-registry/releases/download/${var.release_version}/terraform-provider-registry.zip"
  output_file = "terraform-provider-registry.zip"
}

resource "aws_lambda_function" "terraform-provider-registry" {
  architectures = [
    "x86_64",
  ]
  function_name                  = "TerraformProviderServer-AspNetCoreFunction"
  handler                        = "TerraformProviderRegistry"
  layers                         = []
  memory_size                    = 256
  package_type                   = "Zip"
  reserved_concurrent_executions = -1
  role                           = aws_iam_role.terraform-provider-registry-role.arn
  runtime                        = "dotnet6"
  filename                       = data.download_file.release.output_file
  source_code_hash               = data.download_file.release.output_base64sha256
  timeout                        = 30

  environment {
    variables = {
      "TERRAFORM_PROVIDER_BUCKET" = aws_s3_bucket.terraform-provider-registry.id
    }
  }

  timeouts {}

  tracing_config {
    mode = "PassThrough"
  }
}

resource "aws_iam_role" "terraform-provider-registry-role" {
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "lambda.amazonaws.com"
          }
        },
      ]
      Version = "2012-10-17"
    }
  )
  force_detach_policies = false
  managed_policy_arns = [
    "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole",
  ]
  max_session_duration = 3600
  name                 = "TerraformProviderServer-Role-${var.aws_region}"
  path                 = "/"

  inline_policy {
    name = "bucket-policy"
    policy = jsonencode(
      {
        Statement = [
          {
            Action   = "s3:GetObject"
            Effect   = "Allow"
            Resource = "arn:aws:s3:::${aws_s3_bucket.terraform-provider-registry.id}/*"
            Sid      = "VisualEditor0"
          },
        ]
        Version = "2012-10-17"
      }
    )
  }
}

resource "aws_api_gateway_rest_api" "terraform-provider-registry" {
  api_key_source               = "HEADER"
  binary_media_types           = []
  disable_execute_api_endpoint = false
  minimum_compression_size     = -1
  name                         = "terraform-provider-registry"
  tags                         = {}
  tags_all                     = {}

  endpoint_configuration {
    types = [
      "REGIONAL",
    ]
  }
}

resource "aws_api_gateway_resource" "proxy-resource" {
  parent_id   = aws_api_gateway_rest_api.terraform-provider-registry.root_resource_id
  path_part   = "{proxy+}"
  rest_api_id = aws_api_gateway_rest_api.terraform-provider-registry.id
}

resource "aws_api_gateway_method" "proxy-method" {
  api_key_required     = false
  authorization        = "NONE"
  authorization_scopes = []
  http_method          = "ANY"
  request_models       = {}
  request_parameters = {
    "method.request.path.proxy" = true
  }
  resource_id = aws_api_gateway_resource.proxy-resource.id
  rest_api_id = aws_api_gateway_rest_api.terraform-provider-registry.id
}

resource "aws_api_gateway_integration" "proxy-integration" {
  cache_key_parameters = [
    "method.request.path.proxy",
  ]
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  request_parameters      = {}
  request_templates       = {}
  resource_id             = aws_api_gateway_resource.proxy-resource.id
  rest_api_id             = aws_api_gateway_rest_api.terraform-provider-registry.id
  timeout_milliseconds    = 29000
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.terraform-provider-registry.invoke_arn
}

resource "aws_api_gateway_integration" "root-integration" {
  cache_key_parameters    = []
  connection_type         = "INTERNET"
  content_handling        = "CONVERT_TO_TEXT"
  http_method             = "ANY"
  integration_http_method = "POST"
  passthrough_behavior    = "WHEN_NO_MATCH"
  request_parameters      = {}
  request_templates       = {}
  resource_id             = aws_api_gateway_rest_api.terraform-provider-registry.root_resource_id
  rest_api_id             = aws_api_gateway_rest_api.terraform-provider-registry.id
  timeout_milliseconds    = 29000
  type                    = "AWS_PROXY"
  uri                     = aws_lambda_function.terraform-provider-registry.invoke_arn
}

resource "aws_api_gateway_method" "root-method" {
  api_key_required     = false
  authorization        = "NONE"
  authorization_scopes = []
  http_method          = "ANY"
  request_models       = {}
  request_parameters   = {}
  resource_id          = aws_api_gateway_rest_api.terraform-provider-registry.root_resource_id
  rest_api_id          = aws_api_gateway_rest_api.terraform-provider-registry.id
}

resource "aws_api_gateway_deployment" "prod" {
  rest_api_id = aws_api_gateway_rest_api.terraform-provider-registry.id

  triggers = {
    redeployment = sha1(jsonencode([
      aws_api_gateway_method.root-method.id,
      aws_api_gateway_integration.root-integration.id,
      aws_api_gateway_resource.proxy-resource.id,
      aws_api_gateway_method.proxy-method.id,
      aws_api_gateway_integration.proxy-integration.id
    ]))
  }

  lifecycle {
    create_before_destroy = true
  }
}

resource "aws_api_gateway_stage" "prod" {
  cache_cluster_enabled = false
  deployment_id         = aws_api_gateway_deployment.prod.id
  rest_api_id           = aws_api_gateway_rest_api.terraform-provider-registry.id
  stage_name            = "prod"
  xray_tracing_enabled  = false
}

resource "aws_lambda_permission" "api-gateway" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.terraform-provider-registry.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_api_gateway_deployment.prod.execution_arn}*/*"
}

resource "aws_s3_bucket" "terraform-provider-registry" {
  bucket_prefix = "terraform-provider-registry-"
  force_destroy = true
}

resource "aws_s3_bucket_acl" "terraform-provider-registry" {
  bucket = aws_s3_bucket.terraform-provider-registry.id
  acl    = "private"
}

resource "aws_api_gateway_domain_name" "custom_domain" {
  regional_certificate_arn = data.aws_acm_certificate.issued.arn
  domain_name              = var.custom_domain

  endpoint_configuration {
    types = ["REGIONAL"]
  }
}

data "aws_acm_certificate" "issued" {
  domain   = var.certificate_domain
  statuses = ["ISSUED"]
}

data "aws_route53_zone" "selected" {
  name = var.hosted_domain_name
}

resource "aws_api_gateway_base_path_mapping" "terraform-api-mapping" {
  api_id      = aws_api_gateway_rest_api.terraform-provider-registry.id
  stage_name  = aws_api_gateway_stage.prod.stage_name
  domain_name = aws_api_gateway_domain_name.custom_domain.domain_name
}

resource "aws_route53_record" "terraform-registry-dns-alias" {
  name    = aws_api_gateway_domain_name.custom_domain.domain_name
  type    = "A"
  zone_id = data.aws_route53_zone.selected.id

  alias {
    evaluate_target_health = false
    name                   = aws_api_gateway_domain_name.custom_domain.regional_domain_name
    zone_id                = aws_api_gateway_domain_name.custom_domain.regional_zone_id
  }
}