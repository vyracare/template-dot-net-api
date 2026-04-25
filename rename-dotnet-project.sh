#!/bin/bash
set -euo pipefail

if [ "$#" -lt 3 ]; then
  echo "Uso: ./rename-dotnet-project.sh <repo-name> <database-name> <table-name>"
  exit 1
fi

export REPO_NAME_RAW="$1"
export DATABASE_NAME_RAW="$2"
export TABLE_NAME_RAW="$3"
export API_DESCRIPTION="${API_DESCRIPTION:-}"

python3 <<'PY'
import os
import re
from pathlib import Path


def normalize_kebab(value: str) -> str:
    value = value.strip().lower()
    value = re.sub(r"[^a-z0-9]+", "-", value)
    value = re.sub(r"-{2,}", "-", value).strip("-")
    return value


def normalize_snake(value: str) -> str:
    value = value.strip().lower()
    value = re.sub(r"[^a-z0-9]+", "_", value)
    value = re.sub(r"_{2,}", "_", value).strip("_")
    return value


def to_pascal_case(value: str) -> str:
    words = re.split(r"[-_]+", value.strip())
    return "".join(word[:1].upper() + word[1:].lower() for word in words if word)


def read_text_with_fallback(path: Path) -> str | None:
    for encoding in ("utf-8", "utf-8-sig", "cp1252", "latin-1"):
        try:
            return path.read_text(encoding=encoding)
        except UnicodeDecodeError:
            continue
    return None


repo_name = normalize_kebab(os.environ["REPO_NAME_RAW"])
if not repo_name.startswith("vyracare-api-"):
    repo_name = f"vyracare-api-{repo_name}"

database_name = normalize_snake(os.environ["DATABASE_NAME_RAW"])
table_name = normalize_snake(os.environ["TABLE_NAME_RAW"])
table_route = table_name.replace("_", "-")
api_suffix = repo_name.removeprefix("vyracare-api-")
project_suffix_pascal = to_pascal_case(api_suffix)
resource_name_pascal = to_pascal_case(table_name)
assembly_name = f"Vyracare.Api.{project_suffix_pascal}"
project_file = f"{assembly_name}.csproj"
lambda_function_name = f"{repo_name}-dev"
api_description = os.environ.get("API_DESCRIPTION", "")

replacements = {
    "[repo-generic]": repo_name,
    "[name-generic]": project_suffix_pascal,
    "[assembly-generic]": assembly_name,
    "[project-file-generic]": project_file,
    "[database-generic]": database_name,
    "[table-generic]": table_name,
    "[table-route-generic]": table_route,
    "[resource-generic]": resource_name_pascal,
    "[lambda-name-generic]": lambda_function_name,
    "[description-generic]": api_description,
}

for path in Path(".").rglob("*"):
    if not path.is_file():
        continue

    if ".git" in path.parts:
        continue

    text = read_text_with_fallback(path)
    if text is None:
        continue

    updated = text
    for source, target in replacements.items():
        updated = updated.replace(source, target)

    if updated != text:
        path.write_text(updated, encoding="utf-8")

renames = {
    Path("Vyracare.Api.[name-generic].csproj"): Path(project_file),
    Path("Controllers/[resource-generic]Controller.cs"): Path(f"Controllers/{resource_name_pascal}Controller.cs"),
    Path("DTOS/[resource-generic]Dto.cs"): Path(f"DTOS/{resource_name_pascal}Dto.cs"),
    Path("Models/[resource-generic]Model.cs"): Path(f"Models/{resource_name_pascal}Model.cs"),
    Path("Services/[resource-generic]Service.cs"): Path(f"Services/{resource_name_pascal}Service.cs"),
}

for source, target in renames.items():
    if source.exists():
        source.rename(target)

print(f"Projeto .NET renomeado com sucesso para {repo_name}")
PY
