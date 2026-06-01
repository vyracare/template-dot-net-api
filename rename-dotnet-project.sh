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
export CONSUMER_MFE_REPOSITORY_RAW="${CONSUMER_MFE_REPOSITORY:-}"
export MONGO_CONNECTION_STRING_RAW="${MONGO_CONNECTION_STRING:-}"
export JWT_KEY_RAW="${JWT_KEY:-}"
export JWT_ISSUER_RAW="${JWT_ISSUER:-}"
export JWT_AUDIENCE_RAW="${JWT_AUDIENCE:-}"
export JWT_EXPIRY_MINUTES_RAW="${JWT_EXPIRY_MINUTES:-}"

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


def normalize_repo_full_name(value: str) -> str:
    value = value.strip()
    if not value:
        return ""
    if "/" in value:
        owner, repo = value.split("/", 1)
        return f"{owner.strip().lower()}/{normalize_kebab(repo)}"
    return f"vyracare/{normalize_kebab(value)}"


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
consumer_mfe_repository = normalize_repo_full_name(os.environ.get("CONSUMER_MFE_REPOSITORY_RAW", ""))
mongo_connection_string = os.environ.get("MONGO_CONNECTION_STRING_RAW", "").strip() or "[mongo-connection-string-generic]"
jwt_key = os.environ.get("JWT_KEY_RAW", "").strip() or "[jwt-key-generic]"
jwt_issuer = os.environ.get("JWT_ISSUER_RAW", "").strip() or "[jwt-issuer-generic]"
jwt_audience = os.environ.get("JWT_AUDIENCE_RAW", "").strip() or "[jwt-audience-generic]"
jwt_expiry_minutes = os.environ.get("JWT_EXPIRY_MINUTES_RAW", "").strip() or "[jwt-expiry-minutes-generic]"
mongo_secret_name = "vyracare/shared/mongo-prod"
jwt_secret_name = "vyracare/shared/jwt-signing-prod"

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
    "[consumer-mfe-full-name-generic]": consumer_mfe_repository,
    "[mongo-connection-string-generic]": mongo_connection_string,
    "[jwt-key-generic]": jwt_key,
    "[jwt-issuer-generic]": jwt_issuer,
    "[jwt-audience-generic]": jwt_audience,
    "[jwt-expiry-minutes-generic]": jwt_expiry_minutes,
    "[mongo-secret-name-generic]": mongo_secret_name,
    "[jwt-secret-name-generic]": jwt_secret_name,
}

for path in Path(".").rglob("*"):
    if not path.is_file():
        continue

    if ".git" in path.parts:
        continue

    if path.name == "rename-dotnet-project.sh":
        continue

    text = read_text_with_fallback(path)
    if text is None:
        continue

    updated = text
    for source, target in replacements.items():
        updated = updated.replace(source, target)

    if updated != text:
        path.write_text(updated, encoding="utf-8")

paths_to_rename = sorted(
    (path for path in Path(".").rglob("*") if ".git" not in path.parts),
    key=lambda item: len(item.parts),
    reverse=True,
)

for source in paths_to_rename:
    target_name = source.name
    for placeholder, replacement in replacements.items():
        target_name = target_name.replace(placeholder, replacement)

    if target_name == source.name:
        continue

    source.rename(source.with_name(target_name))

publish_workflow_path = Path(".github/workflows/publish.yml")
publish_workflow_path.write_text(
    "\n".join([
        "name: PUBLISH",
        "",
        "on:",
        "  push:",
        "    branches:",
        "      - develop",
        "      - release",
        "      - 'release/**'",
        "      - main",
        "",
        "jobs:",
        "  cd-dot-net:",
        "    uses: vyracare/vyracare-infra-pipes-dot-net/.github/workflows/cd-generic-dot-net.yml@main",
        "    with:",
        "      branch-name: ${{ github.ref_name }}",
        "      backend-repo: ${{ github.repository }}",
        f"      project-path: {project_file}",
        "      output-dir: backend-publish",
        f"      api-gateway-name: {repo_name}",
        f"      lambda-function-name: {repo_name}",
        f"      lambda-handler: {assembly_name}",
        f"      route-base-path: api/{table_route}",
        "    secrets: inherit",
        "",
        "  create-release-branch:",
        "    name: Create versioned release branch",
        "    runs-on: ubuntu-latest",
        "    needs: cd-dot-net",
        "    if: github.ref == 'refs/heads/develop'",
        "    outputs:",
        "      branch: ${{ steps.release_branch.outputs.branch }}",
        "    steps:",
        "      - name: Checkout repository",
        "        uses: actions/checkout@v4",
        "        with:",
        "          fetch-depth: 0",
        "",
        "      - name: Create or reuse release branch",
        "        id: release_branch",
        "        run: |",
        "          set -euo pipefail",
        "          RELEASE_BRANCH=\"release/v$(date -u +%Y.%m.%d).${GITHUB_RUN_NUMBER}\"",
        "          git config user.name \"github-actions[bot]\"",
        "          git config user.email \"41898282+github-actions[bot]@users.noreply.github.com\"",
        "          git remote set-url origin \"https://x-access-token:${{ secrets.PAT_TOKEN }}@github.com/${GITHUB_REPOSITORY}.git\"",
        "          git fetch origin main develop",
        "          if git ls-remote --heads origin \"$RELEASE_BRANCH\" | grep -q \"$RELEASE_BRANCH\"; then",
        "            git checkout -B \"$RELEASE_BRANCH\" \"origin/$RELEASE_BRANCH\"",
        "          else",
        "            git checkout -B \"$RELEASE_BRANCH\" \"origin/main\"",
        "            git push -u origin \"$RELEASE_BRANCH\"",
        "          fi",
        "          echo \"branch=$RELEASE_BRANCH\" >> $GITHUB_OUTPUT",
        "",
        "  open-pr-release:",
        "    name: Open Pull Request to versioned release",
        "    runs-on: ubuntu-latest",
        "    needs: create-release-branch",
        "    if: github.ref == 'refs/heads/develop'",
        "    steps:",
        "      - name: Open Pull Request to release",
        "        uses: repo-sync/pull-request@v2",
        "        with:",
        "          github_token: ${{ secrets.PAT_TOKEN }}",
        "          source_branch: develop",
        "          destination_branch: ${{ needs.create-release-branch.outputs.branch }}",
        "          pr_title: \"PR automatic develop into ${{ needs.create-release-branch.outputs.branch }}\"",
        "          pr_body: |",
        "            PR automatica criada pelo pipeline.",
        "            Branch de origem: `develop`",
        "            Destino: `${{ needs.create-release-branch.outputs.branch }}`",
        "",
        "  open-pr-main:",
        "    name: Open Pull Request to main",
        "    runs-on: ubuntu-latest",
        "    needs: cd-dot-net",
        "    if: github.ref == 'refs/heads/release' || startsWith(github.ref, 'refs/heads/release/')",
        "    steps:",
        "      - name: Checkout repository",
        "        uses: actions/checkout@v4",
        "        with:",
        "          fetch-depth: 0",
        "",
        "      - name: Detect whether release branch is ahead of main",
        "        id: release_diff",
        "        run: |",
        "          set -euo pipefail",
        "          git fetch origin main \"${{ github.ref_name }}\"",
        "          git checkout -B \"${{ github.ref_name }}\" \"origin/${{ github.ref_name }}\"",
        "          ahead_count=$(git rev-list --count origin/main..HEAD)",
        "          echo \"ahead_count=$ahead_count\" >> $GITHUB_OUTPUT",
        "          if [ \"$ahead_count\" -gt 0 ]; then",
        "            echo \"should_open=true\" >> $GITHUB_OUTPUT",
        "          else",
        "            echo \"should_open=false\" >> $GITHUB_OUTPUT",
        "          fi",
        "",
        "      - name: Open Pull Request to main",
        "        if: steps.release_diff.outputs.should_open == 'true'",
        "        uses: repo-sync/pull-request@v2",
        "        with:",
        "          github_token: ${{ secrets.PAT_TOKEN }}",
        "          source_branch: ${{ github.ref_name }}",
        "          destination_branch: main",
        "          pr_title: \"PR automatic ${{ github.ref_name }} into main\"",
        "          pr_body: |",
        "            PR automatica criada pelo pipeline.",
        "            Branch de origem: `${{ github.ref_name }}`",
        "            Destino: `main`",
        "",
    ]) + "\n",
    encoding="utf-8",
)

print(f"Projeto .NET renomeado com sucesso para {repo_name}")
PY
