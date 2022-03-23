variable "aws_region" {
  type    = string
  default = "us-east-2"
}

variable "release_version" {
  type    = string
  default = "v1.0.1"
}

variable "hosted_domain_name" {
  type    = string
  default = ""
}

variable "certificate_domain" {
  type    = string
}

variable "custom_domain" {
  type    = string
}

variable "github_oauth_client_id" {
  type    = string
  default = ""
}

variable "github_oauth_client_secret" {
  type    = string
  default = ""
}

variable "auth_token_secret_key" {
  type    = string
  default = ""
}

variable "auth_enabled" {
  type    = string
  default = "False"
}