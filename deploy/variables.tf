variable "aws_region" {
  type    = string
  default = "us-east-2"
}

variable "release_version" {
  type    = string
  default = "v1.0.0"
}

variable "hosted_domain_name" {
  type = string
}

variable "certificate_domain" {
  type = string
}

variable "custom_domain" {
  type = string
}