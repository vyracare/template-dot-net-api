#!/bin/bash
set -euo pipefail

if [ "$#" -lt 3 ]; then
  echo "Uso: ./rename-dotnet-project.sh <repo-name> <database-name> <table-name>"
  exit 1
fi

REPO_NAME_RAW="$1"
DATABASE_NAME_RAW="$2"
TABLE_NAME_RAW="$3"
API_DESCRIPTION="${API_DESCRIPTION:-}"

normalize_kebab() {
  echo "$1" | tr '[:upper:]' '[:lower:]' | sed -E 's/[^a-z0-9]+/-/g; s/^-+|-+$//g; s/-+/-/g'
}

normalize_snake() {
  echo "$1" | tr '[:upper:]' '[:lower:]' | sed -E 's/[^a-z0-9]+/_/g; s/^_+|_+$//g; s/_+/_/g'
}

to_pascal_case() {
  local input="$1"
  input="${input//-/ }"
  input="${input//_/ }"

  local result=""
  for word in $input; do
    local first="${word:0:1}"
    local rest="${word:1}"
    result+="${first^^}${rest,,}"
  done
  echo "$result"
}

REPO_NAME="$(normalize_kebab "$REPO_NAME_RAW")"
if [[ "$REPO_NAME" != vyracare-api-* ]]; then
  REPO_NAME="vyracare-api-${REPO_NAME}"
fi

DATABASE_NAME="$(normalize_snake "$DATABASE_NAME_RAW")"
TABLE_NAME="$(normalize_snake "$TABLE_NAME_RAW")"
TABLE_ROUTE="${TABLE_NAME//_/-}"
API_SUFFIX="${REPO_NAME#vyracare-api-}"
PROJECT_SUFFIX_PASCAL="$(to_pascal_case "$API_SUFFIX")"
RESOURCE_NAME_PASCAL="$(to_pascal_case "$TABLE_NAME")"
ASSEMBLY_NAME="Vyracare.Api.${PROJECT_SUFFIX_PASCAL}"
PROJECT_FILE="${ASSEMBLY_NAME}.csproj"
LAMBDA_FUNCTION_NAME="${REPO_NAME}-dev"

export REPO_NAME DATABASE_NAME TABLE_NAME TABLE_ROUTE PROJECT_SUFFIX_PASCAL RESOURCE_NAME_PASCAL ASSEMBLY_NAME PROJECT_FILE LAMBDA_FUNCTION_NAME API_DESCRIPTION

python <<'PY'
import os
from pathlib import Path

replacements = {
    "[repo-generic]": os.environ["REPO_NAME"],
    "[name-generic]": os.environ["PROJECT_SUFFIX_PASCAL"],
    "[assembly-generic]": os.environ["ASSEMBLY_NAME"],
    "[project-file-generic]": os.environ["PROJECT_FILE"],
    "[database-generic]": os.environ["DATABASE_NAME"],
    "[table-generic]": os.environ["TABLE_NAME"],
    "[table-route-generic]": os.environ["TABLE_ROUTE"],
    "[resource-generic]": os.environ["RESOURCE_NAME_PASCAL"],
    "[lambda-name-generic]": os.environ["LAMBDA_FUNCTION_NAME"],
    "[description-generic]": os.environ["API_DESCRIPTION"],
}

def read_text_with_fallback(path: Path) -> str | None:
    for encoding in ("utf-8", "utf-8-sig", "cp1252", "latin-1"):
        try:
            return path.read_text(encoding=encoding)
        except UnicodeDecodeError:
            continue
    return None

for path in Path(".").rglob("*"):
    if path.is_file():
        text = read_text_with_fallback(path)
        if text is None:
            continue
        updated = text
        for source, target in replacements.items():
            updated = updated.replace(source, target)
        if updated != text:
            path.write_text(updated, encoding="utf-8")
PY

mv "Vyracare.Api.[name-generic].csproj" "${PROJECT_FILE}"
mv "Controllers/[resource-generic]Controller.cs" "Controllers/${RESOURCE_NAME_PASCAL}Controller.cs"
mv "DTOS/[resource-generic]Dto.cs" "DTOS/${RESOURCE_NAME_PASCAL}Dto.cs"
mv "Models/[resource-generic]Model.cs" "Models/${RESOURCE_NAME_PASCAL}Model.cs"
mv "Services/[resource-generic]Service.cs" "Services/${RESOURCE_NAME_PASCAL}Service.cs"

echo "Projeto .NET renomeado com sucesso para ${REPO_NAME}"
