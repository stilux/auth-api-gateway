{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "auth-gateway-chart.name" -}}
{{- default .Chart.Name | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "auth-gateway-chart.fullname" -}}
{{- $name := default .Chart.Name }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}

{{/*
Product api fullname.
*/}}
{{- define "product-api.fullname" -}}
{{- printf "%s-%s" (include "auth-gateway-chart.fullname" .) "product-api" }}
{{- end }}

{{/*
Authorization server fullname.
*/}}
{{- define "auth-server.fullname" -}}
{{- printf "%s-%s" (include "auth-gateway-chart.fullname" .) "auth-server" }}
{{- end }}

{{/*
Gateway fullname.
*/}}
{{- define "gateway.fullname" -}}
{{- printf "%s-%s" (include "auth-gateway-chart.fullname" .) "gateway" }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "auth-gateway-chart.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "auth-gateway-chart.labels" -}}
helm.sh/chart: {{ include "auth-gateway-chart.chart" . }}
{{ include "auth-gateway-chart.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Gateway common labels
*/}}
{{- define "gateway.labels" -}}
helm.sh/chart: {{ include "auth-gateway-chart.chart" . }}
{{ include "gateway.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Authorization server common labels
*/}}
{{- define "auth-server.labels" -}}
helm.sh/chart: {{ include "auth-gateway-chart.chart" . }}
{{ include "auth-server.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Product api common labels
*/}}
{{- define "product-api.labels" -}}
helm.sh/chart: {{ include "auth-gateway-chart.chart" . }}
{{ include "product-api.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "auth-gateway-chart.selectorLabels" -}}
app.kubernetes.io/name: {{ include "auth-gateway-chart.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Gateway selector labels
*/}}
{{- define "gateway.selectorLabels" -}}
app.kubernetes.io/name: {{ printf "%s-%s" (include "auth-gateway-chart.name" .) "gateway" }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Authorization server selector labels
*/}}
{{- define "auth-server.selectorLabels" -}}
app.kubernetes.io/name: {{ printf "%s-%s" (include "auth-gateway-chart.name" .) "auth-server" }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Product api selector labels
*/}}
{{- define "product-api.selectorLabels" -}}
app.kubernetes.io/name: {{ printf "%s-%s" (include "auth-gateway-chart.name" .) "product-api" }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Full api gateway image name
*/}}
{{- define "gateway.fullImageName" -}}
{{- printf "%s:v%s" .Values.apiGateway.image.repository .Chart.AppVersion }}
{{- end }}

{{/*
Full authorization server image name
*/}}
{{- define "auth-server.fullImageName" -}}
{{- printf "%s:v%s" .Values.authServer.image.repository .Chart.AppVersion }}
{{- end }}

{{/*
Full product api image name
*/}}
{{- define "product-api.fullImageName" -}}
{{- printf "%s:v%s" .Values.productApi.image.repository .Chart.AppVersion }}
{{- end }}

{{/*
Annotation for config change detection
*/}}
{{- define "auth-gateway-chart.configChangeDetection" -}}
{{- $secret := include (print $.Template.BasePath "/secret.yaml") . | sha256sum -}}
{{- $configmap := include (print $.Template.BasePath "/configmap.yaml") . | sha256sum -}}
checksum/config: {{ print $secret $configmap | sha256sum }}
{{- end }}

{{/*
Create postgresql fullname
*/}}
{{- define "auth-gateway-chart.postgresqlFullname" -}}
{{- printf "%s-%s" .Release.Name "postgresql" | trunc 63 | trimSuffix "-" -}}
{{- end -}}